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

    public List<List<string>> getAnswerList(List<Minterm> keepers)
    {
        List<List<string>> answerStringList = new List<List<string>>();
        List<string> answerString = new List<string>();
        List<int> already_seen_int = new List<int>();
        List<int> delList = new List<int>();
        List<Minterm> already_seen_minterm = new List<Minterm>();
        int flag;

        foreach(Minterm throwaway in keepers)
        {
            Console.WriteLine($"got to start");
            //Console.WriteLine($"got here");
            answerString = new List<string>();
            already_seen_int = new List<int>();
            Console.WriteLine($"start list val");
            foreach (int x in already_seen_int)
            {
                Console.WriteLine($" { x }");
            }
            foreach (Minterm e in keepers)
            {
                foreach (int a in e.Value_List)
                {
                    foreach (int b in value_list)
                    {
                        if (a == b && !already_seen_int.Contains(b))
                        {
                            //Console.WriteLine($"got here 2 a {a} b {b}");
                            
                            if (!already_seen_minterm.Contains(e))
                            {
                                //foreach (int x in already_seen_int)
                                //{
                                //    Console.WriteLine($" () { x }");
                                //}
                                Console.WriteLine($"got here 3 {e.Binary}  {b}");
                                
                                if (!answerString.Contains(e.Binary))
                                {
                                    flag = 0;
                                    foreach(int v in e.Value_List)
                                    {
                                        if(already_seen_int.Contains(v))
                                        {
                                            Console.WriteLine($"{v} flag++");
                                            flag++;
                                        }
                                    }
                                    Console.WriteLine($"flag {flag}");
                                    if(flag == 0)
                                    {
                                        Console.WriteLine($"got this");
                                        answerString.Add(e.Binary);
                                        already_seen_minterm.Add(e);
                                    }
                                   
                                }
                            }
                            already_seen_int.Add(b);

                            //break;
                        }
                    }

                }

            }

            foreach(Minterm m in keepers)
            {
                foreach(Minterm p in already_seen_minterm)
                {
                    foreach(int e in p.Value_List)
                    {
                        if(m.Value_List.Contains(e))
                        {
                            delList.Add(e);
                        }
                    }
                }
            }
            foreach(Minterm n in keepers)
            {
                foreach (int y in delList)
                {
                    n.Value_List.Remove(y);
                }
            }

            foreach(int x in already_seen_int)
            {
                Console.WriteLine($" { x }");
            }
            answerStringList.Add(answerString);
            Console.WriteLine($"MINS");
            foreach (Minterm b in already_seen_minterm)
            {
                Console.WriteLine($"{b.Binary}");
            }
        }
        return answerStringList;
    }

    //finds keeper implicants
    public List<Minterm> findKeepers()//finds the prime implicants we would like to keep from the prime implicants list
    {
        int flag = 0;
        foreach(Minterm x in primeImplicants)
        {
            flag = 0;
            foreach(Minterm y in essencialPrimeImplicants)
            {
                if(y.Binary == x.Binary)
                {
                    //Console.WriteLine($"prime {x.Binary} EPI {y.Binary}");
                    flag++;
                }
            }
            if(flag == 0)
            {
                keeperPrimeImplicants.Add(x);
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