using System;
using System.Collections.Generic;
using System.Text;

public class Table
{
    //starting group, group with starting values
    public List<Group> sGroup;
    //ending group, group with final values for next table
    public List<Group> eGroup;
    //contains all posible answers from this table at the end
    public List<Minterm> possible_answers;
    
    int _group_count;
    public int Group_Count
    {
        set { _group_count = value; }
        get { return _group_count; }
    }

    //holds the truth value of the current table having more answers to pass on to a new table
    bool _has_answers;
    public bool Has_Answers
    {
        set { _has_answers = value; }
        get { return _has_answers; }
    }
    
    public Table(List<Group> group, int varCount)
    {
        sGroup = group;
        possible_answers = new List<Minterm>();
        Has_Answers = false;
        Group_Count = varCount; //sGroup[0].Members[0].Binary.Length; //establishes the max amount of groups needed
    }

    //ballences first table into the final combined table
    public List<Group> balanceTables()
    {
        List<Group> group = sGroup;
        int bitDifference = 0; // number of bit differences between pair of minterms
        List<Group> tempTable = new List<Group>();
        for (int i = 0; i <= Group_Count; i++)
            tempTable.Add(new Group(i));
        //Console.WriteLine($"tempTable c : {tempTable.Count} with gc {Group_Count}");

        for (int i = 0; i < group.Count - 1; i++) // for each group 0 - n-1
        {
            foreach(Minterm m in group[i].Members)//min term to compare
            {
                foreach(Minterm n in group[i+1].Members)//minterm in i+1
                {
                    //reset difference for each pair of terms
                    bitDifference = 0;

                    for (int j = 0; j < m.Binary.Length; j++)//iterate through each string at once comparing the strings
                    {
                        if (m.Binary[j] != n.Binary[j])//if binary digits are the same
                        {
                            bitDifference += 1;                 //if binary digits are not the same add flag
                            Has_Answers = true;
                        }            
                    }
                    if(bitDifference == 1) //if only one bit differs add minterm to next tables group
                    {
                        //Console.WriteLine($"got here");
                        tempTable[i].Members.Add(new Minterm(m, n));//make new minterm and add to other tables group
                        m.Is_Used = true;       //marks m as used
                        n.Is_Used = true;       //marks n as used
                    }
                }

            }
        }

        for(int k = 0; k < group.Count; k++)
        {
            foreach (Minterm b in group[k].Members)
            {
                if(!(b.Is_Used))
                {
                    possible_answers.Add(b);
                }

            }
        }
        //foreach(string s in possible_answers)
        //{
        //    Console.WriteLine($"answers {s}");
        //}
        return tempTable;
    }
}
