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

        //test for new minterm constructer  
        Minterm first = new Minterm(0, 4);
        Minterm second = new Minterm(1, 4);
        Minterm third = new Minterm(first, second);

        Console.WriteLine($"Firsts binary = {first.Binary} and seconds binary = {second.Binary}, so thirds binary = {third.Binary}");

        //list of tables
        List<Table> TableList = new List<Table>();
        //value of next table returning more minterms
        bool more_minterms = true;
        int indexer = 0;

       while(more_minterms)
        {
            Table tempTable = new Table(groups, varCount - indexer);
            tempTable.eGroup = tempTable.balanceTables();
            groups = tempTable.eGroup;
            TableList.Add(tempTable);
            more_minterms = tempTable.Has_Answers;
            indexer++;
        }

        
        foreach(Table e in TableList)
        {
            foreach(Group d in e.sGroup)
            {
                foreach(Minterm a in d.Members)
                {
                    Console.WriteLine($"minterm {a} = binary {a.Binary}");
                }
                Console.WriteLine($"\n");
            }
        }


    }
}
