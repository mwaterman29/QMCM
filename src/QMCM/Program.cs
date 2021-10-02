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
        List<string> answerString = new List<string>();
        foreach(Table e in TableList)
        {
            answerString.AddRange(e.possible_answers);
        }
        foreach(string s in answerString)
        {
            Console.WriteLine($"string {s}");
        }
        answerString = reduceAnswerString(answerString);
        Console.WriteLine($"\n");
        foreach (string s in answerString)
        {
            Console.WriteLine($"string {s}");
        }

    }

    public static List<string> reduceAnswerString(List<string> answerString)
    {
        return answerString.Distinct().ToList();
    }
}
