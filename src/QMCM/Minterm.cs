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
    //Updated count of ones and zeroes
    public int Ones, Zeroes;

    public Minterm(int val, int variableCount)
    {
        Value = val;
        Binary = Convert.ToString(val, 2).PadLeft(variableCount, '0');
    }

}
