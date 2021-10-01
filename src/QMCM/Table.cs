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
    public List<string> possible_answers;

    int _group_count;
    public int Group_Count
    {
        set { _group_count = Group_Count; }
        get { return _group_count; }
    }

    //holds amount of non answers, this will be used to see if we need to make another table
    int _number_non_answers;
    public int Number_Non_Answers
    {
        set { _number_non_answers = Number_Non_Answers; }
        get { return _number_non_answers; }
    }
    
    public Table(List<Group> group)
    {
        sGroup = group;
        possible_answers = new List<string>();
        Number_Non_Answers = 0;
        Group_Count = sGroup[0].Members[0].Binary.Length; //establishes the max amount of groups needed
                                                          //probably change later for efficency
        eGroup = balanceTabls(sGroup);                    //get final group for next table
    }

    //ballences first table into the final combind table
    public List<Group> balanceTabls(List<Group> group)
    {
        int flag = 0;
        List<Group> tempTable = new List<Group>();
        for (int i = 0; i <= Group_Count; i++)
            tempTable.Add(new Group(i));

        for (int i = 0; i < group.Count - 1; i++)
        {
            foreach(Minterm m in group[i].Members)//min term to compare
            {
                foreach(Minterm n in group[i+1].Members)//minterm in i+1
                {
                    for (int j = 0; j < m.Binary.Length; j++)//iterate through each string at once comparing the strings
                    {
                        if (m.Binary[i] != n.Binary[i])//if binary digits are the same
                        {
                            flag += 1;                 //if binary digits are not the same add flag
                        }            
                    }
                    if(flag < 2)//if only one bit differs add minterm to next tables group
                    {
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
                    possible_answers.Add(b.Binary);
                }

            }
        }
        return tempTable;
    }
}
