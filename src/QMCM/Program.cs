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

        Table test = new Table(groups);
        for (int g = 0; g < groups.Count; g++)
        {
            foreach (Minterm m in test.sGroup[g].Members)
            {
                Console.WriteLine($"Minterm {g} = {m.Binary}");
            }
        }
        for (int a = 0; a < groups.Count - 1; a++)
        {
            foreach (Minterm n in test.eGroup[a].Members)
            {
                Console.WriteLine($"Minterm {a} = {n.Binary}");
            }
        }
    }
}
