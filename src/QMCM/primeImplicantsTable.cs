using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class primeImplicantsTable
{

    public List<Minterm> primeImplicants;           //holds prime implicants
    public List<Minterm> essencialPrimeImplicants;  //holds essencial prime implicants
    public List<Minterm> keeperPrimeImplicants;     //holds prime implicants we want to keep after essencial primes
    public List<Minterm> answerList;                //holds lists of answers

    public List<int> value_list;                    //holds a list of minterm integer values
        
    public List<string> vars;                       //holds the list of variables

    public primeImplicantsTable(List<Minterm> iprimeImplicants, List<string> ivars, List<Minterm> minterms)
    {
        primeImplicants = new List<Minterm>();
        primeImplicants = iprimeImplicants;
        essencialPrimeImplicants = new List<Minterm>();
        keeperPrimeImplicants = new List<Minterm>();
        value_list = new List<int>();
        foreach(Minterm x in minterms)
        {
            value_list.Add(x.Value);
            
        }
        answerList = new List<Minterm>();

        vars = ivars;
    }

    //public List<List<String>> getAnswerList(List<Minterm> keepers)
    //{

    //}

    //finds keeper implicants
    public List<Minterm> findKeepers()
    {
        foreach(Minterm x in primeImplicants)
        {
            foreach(Minterm y in essencialPrimeImplicants)
            {
                if(y.Binary != x.Binary)
                {
                    keeperPrimeImplicants.Add(x);
                }
            }
        }
        return keeperPrimeImplicants;
    }

    //finds the essencial prime implicants and returns them in a minterm list
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

    //finds if term val is in minterm list list
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