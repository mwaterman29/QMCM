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
    List<int> _value_list;
    public List<int> Value_List
    {
        get { return _value_list; }
        set { _value_list = Value_List; }
    }

    //Updated count of ones and zeroes
    public int Ones, Zeroes;

    public Minterm(int val, int variableCount)
    {
        Value = val;
        _value_list = new List<int>();
        _value_list.Add(val);
        Binary = Convert.ToString(val, 2).PadLeft(variableCount, '0');
    }

    //constructor to combine 2 minterms
    public Minterm(Minterm first, Minterm second)
    {
        _value_list = new List<int>();
        _value_list.AddRange(first.Value_List);//combine the Value_Lists 
        _value_list.AddRange(second.Value_List);
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
