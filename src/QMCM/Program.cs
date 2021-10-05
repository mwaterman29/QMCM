using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {

        //Prompt user
        Console.WriteLine($"Enter a file path to read from a file, two comma separated files paths for testing, or a list of variables then a list of minterms to run the program.");

        //Read line in, decide on run mode based on properties of read line
        string line = Console.ReadLine();
        if (line.Contains("/") || line.Contains(".") || line.Contains("\\")) // if line is a file path
        {
            if (line.Split(',').Length == 1) //if one file
            {
                OneFile(line);
            }
            else if (line.Split(',').Length == 2)
            {
                Console.WriteLine($"Running tests from input file {line.Split(',')[0]}, to match output file {line.Split(',')[1].Trim()}");
                TwoFiles(line);
            }
            else
            {
                Console.WriteLine($"Sorry, no more than two files are accepted. File paths cannot contain commas.");
                return;
            }
        }
        else
        {
            DirectInput(line);
        }

    }

    public static void DirectInput(string line)
    {
        //Read line of comma-separated variables
        List<string> vars = line.Split(',').ToList();
        int varCount = vars.Count;

        //Read second line of comma-separated minterms
        line = Console.ReadLine();
        List<Minterm> minterms =
            line.Split(',')
            .Select(x => new Minterm(int.Parse(x), varCount))
            .ToList();

        //run on those variables
        Run(minterms, vars);
    }

    public static void OneFile(string line)
    {
        //list init
        List<Minterm> minterms = new List<Minterm>();
        List<string> vars = new List<string>();

        Console.WriteLine($"Running on input from file {line}");

        Console.WriteLine($"Reading next pair of lines...");

        //Read lines from file, run input
        if (!ropen(line, out StreamReader reader))
            return;

        while (!reader.EndOfStream)
        {
            minterms.Clear();
            vars.Clear();

            //Read line of comma-separated variables
            line = reader.ReadLine();
            vars.AddRange(line.Split(',').ToList());
            int varCount = vars.Count;

            //Read line of comma-separated minterms
            line = reader.ReadLine();
            minterms.AddRange(
                line.Split(',')
                .Select(x => new Minterm(int.Parse(x), varCount))
                .ToList()
                );

            //run on those variables
            Run(minterms, vars, 0);
        }
    }

    public static void TwoFiles(string line)
    {
        //list init
        List<Minterm> minterms = new List<Minterm>();
        List<string> vars = new List<string>();


        string inPath = line.Split(',')[0].Trim();
        string outPath = line.Split(',')[1].Trim();
        Console.WriteLine($"Reading from input {inPath}, expected output {outPath}");
        if (!ropen(inPath, out StreamReader inreader))
            return;
        if (!ropen(outPath, out StreamReader outreader))
            return;

        int successes = 0;
        List<string> failures = new List<string>();

        while (!inreader.EndOfStream)
        {
            minterms.Clear();
            vars.Clear();

            Console.WriteLine($"Reading next pair of lines...");

            //Read line of comma-separated variables
            line = inreader.ReadLine();
            Console.WriteLine(line);
            string varscopy = line;
            vars.AddRange(line.Split(',').ToList());
            int varCount = vars.Count;

            //Read line of comma-separated minterms
            line = inreader.ReadLine();
            string mintermscopy = line;
            Console.WriteLine(line);
            minterms.AddRange(
                line.Split(',')
                .Select(x => new Minterm(int.Parse(x), varCount))
                .ToList()
                );

            //run on those variables
            string output = Run(minterms, vars);
            string expected = outreader.ReadLine();
            if (output != expected)
                failures.Add($"FAILURE: Output {output} did NOT match expected output {expected}. \nOriginal inputs: \n{varscopy}\n{mintermscopy}");
            else
                successes++;

            //read newline
            outreader.ReadLine();
        }

        Console.WriteLine($"Of {successes + failures.Count} total tests, {failures.Count} failed.");
        failures.ForEach(fail => Console.WriteLine(fail));

    }

    //Helper method to open file from path, or parse a non-absolute path.
    private static bool ropen(string fname, out StreamReader reader)
    {
        string path = fname.Trim();

        //if not an absolute path
        if (!path.Contains(@":\"))
        {
            string dir = Directory.GetCurrentDirectory();
            string topdir = dir.Substring(0, dir.IndexOf(@"src\QMCM\") + 9);
            path = topdir + fname.Trim();
        }
        try
        {
            reader = new StreamReader(path);
            return true;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            reader = null;
            return false;
        }
    }

    /*
         * [https://www.tutorialspoint.com/digital_circuits/digital_circuits_quine_mccluskey_tabular_method.htm]
        Step 1 − Arrange the given min terms in an ascending order and make the groups based on the number of ones present in their binary representations. So, there will be at most ‘n+1’ groups if there are ‘n’ Boolean variables in a Boolean function or ‘n’ bits in the binary equivalent of min terms.

        Step 2 − Compare the min terms present in successive groups. If there is a change in only one-bit position, then take the pair of those two min terms. Place this symbol '_' in the differed bit position and keep the remaining bits as it is.

        Step 3 − Repeat step2 with newly formed terms till we get all prime implicants.

        Step 4 − Formulate the prime implicant table. It consists of set of rows and columns. Prime implicants can be placed in row wise and min terms can be placed in column wise. Place ‘1’ in the cells corresponding to the min terms that are covered in each prime implicant.

        Step 5 − Find the essential prime implicants by observing each column. If the min term is covered only by one prime implicant, then it is essential prime implicant. Those essential prime implicants will be part of the simplified Boolean function.

        Step 6 − Reduce the prime implicant table by removing the row of each essential prime implicant and the columns corresponding to the min terms that are covered in that essential prime implicant. Repeat step 5 for Reduced prime implicant table. Stop this process when all min terms of given Boolean function are over.
        */
    public static string Run(List<Minterm> minterms, List<string> vars, ushort printMask = 65535)
    {
        int varCount = vars.Count;

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
        while (more_minterms)
        {
            Table tempTable = new Table(groups, varCount - indexer);// creates new table with the user input group list
            tempTable.eGroup = tempTable.balanceTables();//sets the ending groups as the ballenced group from the table before
            groups = tempTable.eGroup;//sets the groups variable as the last balenced group list
            TableList.Add(tempTable);//adds the table to the table list
            more_minterms = tempTable.Has_Answers;//checks if we need to run through the loop again
            indexer++;
        }

        if(nthBit(printMask, 0) == 1)
        {
            Console.WriteLine($"\nEach table in succesion");

            foreach (Table e in TableList)       //itterates through every table in the list and prints it
            {
                foreach (Group d in e.sGroup)
                {
                    foreach (Minterm a in d.Members)
                    {
                        Console.WriteLine($"binary {a.Binary}");
                    }
                }
                Console.WriteLine($"\n");
            }
        }
        
        List<Minterm> primeImplicants = new List<Minterm>();//new minterm list for prime implicants
        List<Minterm> essentialPrimeImplicants = new List<Minterm>();//new minterm list for essential prime implicants
        List<Minterm> keeperPrimeImplicants = new List<Minterm>();
        List<List<Minterm>> answers = new List<List<Minterm>>();
        foreach (Table e in TableList)//iterate over the table list to find tables
        {
            primeImplicants.AddRange(e.possible_answers);//add their possibble
        }

        primeImplicants = reduceAnswerString(primeImplicants);//take out any duplicate implicants

        primeImplicantsTable PImp = new primeImplicantsTable(primeImplicants, vars, minterms);//get prime implicants
        essentialPrimeImplicants = PImp.findEPI();                                            //get essential prime implicants
        keeperPrimeImplicants = reduceAnswerString(PImp.findKeepers());                       //get prime implicants we need to keep

        if (nthBit(printMask, 1) == 1)
        {
            Console.WriteLine($"essential");    //itterate over essential prime implicants and print
            foreach (Minterm s in essentialPrimeImplicants)
            {
                Console.WriteLine($"binary = {s.Binary}");
            }
            Console.Write($"\n");
        }

        if (nthBit(printMask, 2) == 1)
        {
            Console.WriteLine($"keepers prime");//itterate over keeperPrimeImplicants and print
            foreach (Minterm s in keeperPrimeImplicants)
            {
                Console.WriteLine($"binary = {s.Binary}");

            }
            Console.Write($"\n");
        }

        answers = PImp.getAnswerList(essentialPrimeImplicants, keeperPrimeImplicants);

        string answersString = string.Empty;
        foreach (List<Minterm> x in answers)
        {
            foreach (Minterm c in x)
            {
                Console.WriteLine("minterm binaray = " + c.Binary);
                answersString += $"{printBinaryToString(c.Binary, vars)} + ";
            }
            break;
        }
        

        if (nthBit(printMask, 3) == 1)
        {

        }
            Console.WriteLine($"Answers: {answersString.Substring(0, answersString.Length - 3)}");
        return answersString.Substring(0, answersString.Length-3);
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
    public static string printBinaryToString(string binary, List<string> variables)
    {
        string finalString = "";

        for(int i = 0; i < variables.Count; i++)             //for the amout of variables in this problem
        {
            if(binary[i] == '_')                            //if binary[i] is a _ dont add anything to final string and continue
            {
                continue;
            }
            switch (binary[i])                              //switch statement for checking if its a 1 or 0
            {
                case '1':
                    finalString += variables[i].Trim();            
                    break;
                case '0':
                    finalString += variables[i].Trim() + "'";
                    break;
                default:
                    finalString += "E";
                    break;
            }

        }

        return finalString;
    }

    private static int nthBit(ushort num, int n)
    {
        return (num >> n) & 1;
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