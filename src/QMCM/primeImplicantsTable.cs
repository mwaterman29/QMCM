using System;
using System.Collections.Generic;
using System.Text;

class primeImplicantsTable
{

    public List<Minterm> primeImplicants;
    public List<Minterm> essencialPrimeImplicants;

    public List<string> vars;

    public primeImplicantsTable(List<Minterm> iprimeImplicants, List<string> ivars)
    {
        primeImplicants = new List<Minterm>();
        primeImplicants = iprimeImplicants;
        essencialPrimeImplicants = new List<Minterm>();
        vars = ivars;
    }

    public List<Minterm> findEPI()
    {
        
        foreach(Minterm x in primeImplicants)
        {
            foreach(int c in x.Value_List)
            {
                if(notInGroup(c, primeImplicants))
                {
                    essencialPrimeImplicants.Add(x);
                    break;
                }
            }
        }

        return essencialPrimeImplicants;
    }

    private bool notInGroup(int val, List<Minterm> list)
    {
        int instancesVarFound = 0;
        foreach (Minterm x in list)
        {
            foreach(int c in x.Value_List)
            {
                if(c == val)
                {
                    instancesVarFound++;
                }
            }
        }
        if(instancesVarFound > 1)
        {
            return false;
        }
        return true;
    }
}