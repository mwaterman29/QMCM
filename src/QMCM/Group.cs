using System;
using System.Collections.Generic;
using System.Linq;

public class Group
{
    //Members
    int _key;
    public int Key
    {
        get { return _key; }
        set { _key = value; }
    }

    public List<Minterm> Members;

    public Group()
    {
        Members = new List<Minterm>();
        Key = -1;
    }

    public Group(int key)
    {
        Members = new List<Minterm>();
        Key = key;
    }

}
