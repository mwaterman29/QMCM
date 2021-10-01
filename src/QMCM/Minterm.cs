using System;
using System.Collections.Generic;
using System.Text;

public class Minterm
{
    //Integer value of the minterm (i.e. 5)
    int _value;
    public int Value
    { 
        get { return _value; }
        set { _value = value; }
    }

    bool _is_used;
    public bool Is_Used
    {
        get { return _is_used; }
        set { _is_used = Is_Used; }
    }

    //Binary representation of the minterm (i.e. 0101)
    string _binary;
    public string Binary
    {
        get { return _binary; }
        set
        {
            _binary = value;
            foreach(char c in _binary)
            {
                if (c == '0')
                    Zeroes++;
                else if(c == '1')
                    Ones++;
            }
        }
    }
    
    //holds the minterm value history of the minterm
    public List<int> Value_List;

    //Updated count of ones and zeroes
    public int Ones, Zeroes;

    public Minterm(int val, int variableCount)
    {
        Is_Used = false;
        Value = val;
        Value_List = new List<int>();
        Value_List.Add(val);
        Binary = Convert.ToString(val, 2).PadLeft(variableCount, '0');
    }

    //constructor to combine 2 minterms
    public Minterm(Minterm first, Minterm second)
    {
        Is_Used = false;
        Value_List = new List<int>();
        Value_List.AddRange(first.Value_List);//combine the Value_Lists 
        Value_List.AddRange(second.Value_List);
        for(int i = 0; i < first.Binary.Length; i++)//start of combining the binarys
        {
            if (first.Binary[i] == second.Binary[i])
            {
                _binary += first.Binary[i];
            }
            else
            {
                _binary += '_';
            }
        }
    }


}
