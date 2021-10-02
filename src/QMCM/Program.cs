using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        //Read line of comma-separated variables
        string line = Console.ReadLine();
        List<string> vars = line.Split(',').ToList();
        int varCount = vars.Count;

        //Read line of comma-separated minterms
        line = Console.ReadLine();
        List<Minterm> minterms = line.Split(',')
            .Select(x => new Minterm(int.Parse(x), varCount))
            .ToList();

        //List out minterms and their properties
        minterms.ForEach(m => Console.WriteLine($"Minterm {m.Value} is binary {m.Binary} with {m.Ones} ones and {m.Zeroes} zeroes"));

        //Make n groups
        List<Group> groups = new List<Group>();
        for (int i = 0; i <= varCount; i++)
            groups.Add(new Group(i));

        //Sort minterms into groups
        minterms.ForEach(m => groups[m.Ones].Members.Add(m));

        //List out groups
        groups.ForEach(g => Console.WriteLine($"Group {g.Key} has {g.Members.Count} members"));

        //Pare down groups, remove groups with no members
        groups = groups.Where(g => g.Members.Count != 0).ToList();

        //List out groups
        groups.ForEach(g => Console.WriteLine($"Group {g.Key} has {g.Members.Count} members"));

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

        
        foreach(Table e in TableList)
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
        foreach(Table e in TableList)//iterate over the table list to find tables
        {
            primeImplicants.AddRange(e.possible_answers);//add their possibble
        }

        foreach(Minterm s in primeImplicants)//itterate over the prime implicants list
        {
            Console.WriteLine($"binary = {s.Binary}");
            foreach(int q in s.Value_List)
            {
                Console.Write($" {q} ");
            }
            Console.Write($"\n");
        }

        primeImplicants = reduceAnswerString(primeImplicants);//take out any duplicate implicants
        Console.WriteLine($"\n");
        foreach (Minterm s in primeImplicants)
        {
            Console.WriteLine($"binary = {s.Binary}");
            foreach (int q in s.Value_List)
            {
                Console.Write($" {q} ");
            }
            Console.Write($"\n");
        }
        essencialPrimeImplicants = findEPI(primeImplicants);
        foreach (Minterm s in essencialPrimeImplicants)
        {
            Console.WriteLine($"binary = {s.Binary}");
            foreach (int q in s.Value_List)
            {
                Console.Write($" {q} ");
            }
            Console.Write($"\n");
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

    public static List<Minterm> findEPI(List<Minterm> primeImplicants)
    {
        List<Minterm> EPI = new List<Minterm>();

        return EPI;
    }
}
