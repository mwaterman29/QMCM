using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {   
        /*
         * [https://www.tutorialspoint.com/digital_circuits/digital_circuits_quine_mccluskey_tabular_method.htm]
        Step 1 − Arrange the given min terms in an ascending order and make the groups based on the number of ones present in their binary representations. So, there will be at most ‘n+1’ groups if there are ‘n’ Boolean variables in a Boolean function or ‘n’ bits in the binary equivalent of min terms.

        Step 2 − Compare the min terms present in successive groups. If there is a change in only one-bit position, then take the pair of those two min terms. Place this symbol '_' in the differed bit position and keep the remaining bits as it is.

        Step 3 − Repeat step2 with newly formed terms till we get all prime implicants.

        Step 4 − Formulate the prime implicant table. It consists of set of rows and columns. Prime implicants can be placed in row wise and min terms can be placed in column wise. Place ‘1’ in the cells corresponding to the min terms that are covered in each prime implicant.

        Step 5 − Find the essential prime implicants by observing each column. If the min term is covered only by one prime implicant, then it is essential prime implicant. Those essential prime implicants will be part of the simplified Boolean function.

        Step 6 − Reduce the prime implicant table by removing the row of each essential prime implicant and the columns corresponding to the min terms that are covered in that essential prime implicant. Repeat step 5 for Reduced prime implicant table. Stop this process when all min terms of given Boolean function are over.
        */

        //Read line of comma-separated variables
        Console.WriteLine($"input variables on one line then minterms on the next line");
        string line = Console.ReadLine();
        List<string> vars = line.Split(',').ToList();
        int varCount = vars.Count;

        //Read line of comma-separated minterms
        line = Console.ReadLine();
        List<Minterm> minterms = line.Split(',')
            .Select(x => new Minterm(int.Parse(x), varCount))
            .ToList();

        //Make n groups
        List<Group> groups = new List<Group>();
        for (int i = 0; i <= varCount; i++)
            groups.Add(new Group(i));

        //Sort minterms into groups
        minterms.ForEach(m => groups[m.Ones].Members.Add(m));

        //list of tables
        List<Table> TableList = new List<Table>();
        //value of next table returning more minterms
        bool more_minterms = true;  // goes false if there are not any minterms left to solve
        int indexer = 0;    // while loop index

        //ballences a table then makes another table with the new minterms until no more can be made.
        while(more_minterms)
        {
            Table tempTable = new Table(groups, varCount - indexer);// creates new table with the user input group list
            tempTable.eGroup = tempTable.balanceTables();//sets the ending groups as the ballenced group from the table before
            groups = tempTable.eGroup;//sets the groups variable as the last balenced group list
            TableList.Add(tempTable);//adds the table to the table list
            more_minterms = tempTable.Has_Answers;//checks if we need to run through the loop again
            indexer++;
        }

        Console.WriteLine($"\nEach table in succesion"); 

        foreach(Table e in TableList)       //itterates through every table in the list and prints it
        {
            foreach(Group d in e.sGroup)
            {
                foreach(Minterm a in d.Members)
                {
                    Console.WriteLine($"binary {a.Binary}");
                }
            }
            Console.WriteLine($"\n");
        }



        List<Minterm> primeImplicants = new List<Minterm>();//new minterm list for prime implicants
        List<Minterm> essencialPrimeImplicants = new List<Minterm>();//new minterm list for essencial prime implicants
        List<Minterm> keeperPrimeImplicants = new List<Minterm>();
        List<List<Minterm>> answers = new List<List<Minterm>>();
        foreach(Table e in TableList)//iterate over the table list to find tables
        {
            primeImplicants.AddRange(e.possible_answers);//add their possibble
        }

        Console.WriteLine($"Lists of primes:");

        primeImplicants = reduceAnswerString(primeImplicants);//take out any duplicate implicants
        Console.WriteLine($"\n");

        Console.WriteLine($"pime");
        foreach (Minterm s in primeImplicants)//iterate over prime implicants and print
        {
            Console.WriteLine($"binary = {s.Binary}");
        }
        Console.Write($"\n");
        primeImplicantsTable PImp = new primeImplicantsTable(primeImplicants, vars, minterms);//get prime implicants
        essencialPrimeImplicants = PImp.findEPI();                                            //get essencial prime implicants
        keeperPrimeImplicants = reduceAnswerString(PImp.findKeepers());                       //get prime implicants we need to keep

      

        Console.WriteLine($"essencial");    //itterate over essencial prime implicants and print
        foreach (Minterm s in essencialPrimeImplicants)
        {
            Console.WriteLine($"binary = {s.Binary}");
        }
        Console.Write($"\n");

        Console.WriteLine($"keepers pime");//itterate over keeperPrimeImplicants and print
        foreach (Minterm s in keeperPrimeImplicants)
        {
            Console.WriteLine($"binary = {s.Binary}");
            
        }
        Console.Write($"\n");

        answers = PImp.getAnswerList(essencialPrimeImplicants, keeperPrimeImplicants);

        foreach (List<Minterm> x in answers)
        {
            foreach (Minterm c in x)
            {
                Console.Write($" {printBinaryToString(c.Binary, vars)} "); 
            }
            Console.WriteLine();
        }

    }



    //take out any duplicate implicants
    public static List<Minterm> reduceAnswerString(List<Minterm> primeImplicants)
    {
        for(int i = 0; i < primeImplicants.Count; i++)
        {
            for(int j = i+1; j < primeImplicants.Count; j++)
            {
                if(primeImplicants[i].Binary == primeImplicants[j].Binary)
                {
                    primeImplicants.RemoveAt(j);
                }
            }
        }
        return primeImplicants;
    }

    //converts binary representation of minvalues to variable strings
    public static string printBinaryToString(string binary, List<string> variabls)
    {
        string finalString = "";

        for(int i = 0; i < variabls.Count; i++)             //for the amout of variabls in this problem
        {
            if(binary[i] == '_')                            //if binary[i] is a _ dont add anything to final string and continue
            {
                continue;
            }
            switch (binary[i])                              //switch statement for checking if its a 1 or 0
            {
                case '1':
                    finalString += variabls[i].Trim();            
                    break;
                case '0':
                    finalString += variabls[i].Trim() + "'";
                    break;
                default:
                    finalString += "E";
                    break;
            }

        }
        

        return finalString;
    }

}

/*
 * 
 * 

        ////List out groups
        //groups.ForEach(g => Console.WriteLine($"Group {g.Key} has {g.Members.Count} members"));

        ////Pare down groups, remove groups with no members
        //groups = groups.Where(g => g.Members.Count != 0).ToList();

        ////List out groups
        //groups.ForEach(g => Console.WriteLine($"Group {g.Key} has {g.Members.Count} members"));


        //minterms.ForEach(m => Console.WriteLine($"Minterm {m.Value} is binary {m.Binary} with {m.Ones} ones and {m.Zeroes} zeroes"));
*/