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

    public List<List<Minterm>> getAnswerList(List<Minterm> essencialPrimes, List<Minterm> keepers)
    {
        List<List<Minterm>> answersL = new List<List<Minterm>>();//list of answer lists
        List<Minterm> answers = new List<Minterm>();            //list of answers
        List<int> keeperVals = new List<int>();                 //list of keeper values
        List<int> intersecionVals = new List<int>();            //values of the intersection keeper value list and minterm valuelist  
        List<int> tempKeeperVals = new List<int>();             //tempory list of keeper values
        List<Minterm> usedTerms = new List<Minterm>();          //midterms already used
        List<int> usedInt = new List<int>();                    //integers already used
        List<List<Minterm>> final_answers = new List<List<Minterm>>();
        bool bFlag;                                             //used to see if second part needs to break
        int flag = 0;                                           //used to see if first part needs to add value to keeper values
        keepers = keepers.Distinct().ToList();
        essencialPrimes = essencialPrimes.Distinct().ToList();
        //create list of original minterm values that we need to get combos for (ie 1, 3, 12)
        foreach(Minterm l in keepers)                           //itterate through keepers minterm list
        {
            foreach (int s in l.Value_List)                     //itterate through the minterms value list
            {       
                flag = 0;                                       //reset flag variable to 0
                foreach (Minterm a in essencialPrimes)          //itterate through minterms in essencial primes
                {   
                    if (a.Value_List.Contains(s))               //if the essencial prime covers this minterm
                    {
                        flag++;                                 //set flag + 1. if this flag is greater than 0
                    }                                           //we know that the essencial primes cover that minterm
                    
                }
                
                if(flag == 0)                                   //if flag == 0 we need to add that minterm integer to
                {                                               //the ones we need to find combos for later
                    keeperVals.Add(s);
                    
                }
            }
        }
        keeperVals = keeperVals.Distinct().ToList();            //take out any duplicates from the list

        foreach(Minterm p in keepers)                           //for all the minterms in the keepers list
        {                                                       //take out all not needed values
            p.Value_List = p.Value_List.Intersect(keeperVals).Distinct().ToList();
        }

        //get combos needed
        foreach(Minterm k in keepers)                           //itterate over all keeper values                           
        {               
            usedInt = new List<int>();                          //reset used int list                      
            tempKeeperVals = new List<int>(keeperVals);         //reset temp keeper vals list
            answers = new List<Minterm>();                      //reset answers Minterm list
            intersecionVals = new List<int>();                  //reset intersection vals list
            bFlag = false;                                      //reset b flag to false
            intersecionVals = k.Value_List.Intersect(tempKeeperVals).Distinct().ToList();//find minvalues this minterm already covers
            answers.Distinct().ToList().Add(k);                                     //add that minterm to answer list
            foreach (int p in intersecionVals)                  //itterate over intersection vals and remove
            {                                                   //minvalues that this minterm already covers
                if(tempKeeperVals.Contains(p))
                {
                    tempKeeperVals.Remove(p);
                }
            }
            
            //start to get all combos
            foreach (Minterm j in keepers)                      //itterate over all keeper list minterms                      
            {
                bFlag = false;                                  //reset bFlag to false
                foreach(int q in tempKeeperVals)                //itterate over all ints in temp keeper vals
                {
                    if (j.Value_List.Contains(q) && !usedTerms.Contains(j) && !usedInt.Contains(q))//if q is in the minterms needed values
                    {                                                                              //and we havent used that minterm yet
                                                                                                   //and we havent seen that number yet
                        answers.Add(j);                         //add the minterm to the answer list
                        answers = answers.Distinct().ToList();
                        usedTerms.Add(j);                       //add the minterm to the used minterm list
                        foreach(int n in j.Value_List)          //add all minterms minvalues to the used int list
                        {
                            usedInt.Add(n);
                        }
                        bFlag = true;                           //set bflag to true
                        
                    }    
                    if(bFlag)                                   //if this operation was succesful breal
                    {
                        break;
                    }
                }
            }
            foreach(Minterm h in essencialPrimes)              //add essencial primes to every answer
            {
                answers.Add(h);
                answers = answers.Distinct().ToList();
            }
            answers = answers.Distinct().ToList();
            answersL.Add(clear_dupes(answers));                             //add answers list to list of answers lists
        }
        if (answersL.Count == 0)                               //if there were no keeper primes to find combos for, add esencial primes to answer list
        {
            foreach (Minterm w in essencialPrimes)
            {
                answers.Add(w);
            }
            answersL.Add(answers);
        }
        
        return answersL;                                        //return answer list
    }

    public List<Minterm> findKeepers()//finds the prime implicants we would like to keep from the prime implicants list
    {
        int flag = 0;
        foreach (Minterm x in primeImplicants)
        {
            flag = 0;
            foreach (Minterm y in essencialPrimeImplicants)
            {
                if (y.Binary == x.Binary)
                {
                    //Console.WriteLine($"prime {x.Binary} EPI {y.Binary}");
                    flag++;
                }
            }
            if (flag == 0)
            {
                keeperPrimeImplicants.Add(x);
            }
        }
        keeperPrimeImplicants = keeperPrimeImplicants.Distinct().ToList();
        return keeperPrimeImplicants;
    }
    ////finds keeper implicants
    //public List<Minterm> findKeepers()//finds the prime implicants we would like to keep from the prime implicants list
    //{
    //    int flag = 0;
    //    int flag2 = 0;
    //    List<int> values_needed = new List<int>();
    //    foreach (Minterm r in primeImplicants)
    //    {
    //        foreach(int a in r.Value_List)
    //        {
    //            flag2 = 0;
    //            Console.WriteLine($"a = {a}");
    //            foreach(Minterm s in essencialPrimeImplicants)
    //            {
    //                if(s.Value_List.Contains(a))
    //                {
    //                    flag2++;
    //                }
    //            }
    //            if (flag2 == 0)
    //            {
    //                Console.WriteLine($" in a = {a}");
    //                values_needed.Add(a);
    //            }
    //        }
    //    }
    //    List<int> tempVals = new List<int>();
    //    values_needed = values_needed.Distinct().ToList();
    //    foreach(Minterm a in primeImplicants)
    //    {
    //        tempVals.AddRange(a.Value_List);
    //    }

       
    //    values_needed = values_needed.Distinct().ToList();
    //    foreach(Minterm x in primeImplicants)
    //    {
    //        flag = 0;
    //        foreach(Minterm y in essencialPrimeImplicants)
    //        {
    //            if(y.Binary == x.Binary)
    //            {
    //                Console.WriteLine($"prime {x.Binary} EPI {y.Binary}");
    //                flag++;
    //            }
    //        }
    //        if(flag == 0)
    //        {
    //            foreach(int r in values_needed)
    //            {
    //                if(!x.Value_List.Contains(r))
    //                {
    //                    Console.WriteLine($"x = {x.Binary}");
    //                    keeperPrimeImplicants.Add(x);
    //                }
    //            }
                
                

    //        }
    //    }
    //    return keeperPrimeImplicants.Distinct().ToList();
    //}
   



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
        essencialPrimeImplicants = essencialPrimeImplicants.Distinct().ToList();
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

    public static List<List<Minterm>> GenerateCombinations(List<Minterm> keepers)
    {
        //Iterative creation of level-order tree
        List<List<Minterm>> allCombinations = new List<List<Minterm>>();
        List<List<Minterm>> combinations = new List<List<Minterm>>(); //start with no combinations
        combinations.Add(new List<Minterm>());

        //iterate to n levels where n is the length of the keepers list
        int n = keepers.Count;
        int count = 0;
        while(count < n)
        {
            List<List<Minterm>> nextCombinations = new List<List<Minterm>>();

            //append each item to each combo
            foreach (List<Minterm> combo in combinations)
            { 
                foreach(Minterm k in keepers)
                {
                    //Deep copy this combination
                    List<Minterm> nextCombo = new List<Minterm>(combo);
                    //Append new minterm to it
                    nextCombo.Add(k);
                    //Add this combination to the next level of the tree
                    nextCombinations.Add(nextCombo);
                    //Add to list of all combinations
                    allCombinations.Add(nextCombo);
                }
            }
            //Replace level with next level, increment count
            combinations = nextCombinations;
            count++;

        }

        return allCombinations;

    }

    private List<Minterm> clear_dupes(List<Minterm> starters)
    {
        List<Minterm> answers = new List<Minterm>();
        List<string> answers_binary = new List<string>();
        
        foreach(Minterm m in starters)
        {
            foreach(Minterm n in starters)
            {
                if(m.Binary == n.Binary && !(list_contains_string(answers_binary, m.Binary)))
                {
                    Console.WriteLine(m.Binary + " " + n.Binary);
                    answers.Add(m);
                    answers_binary.Add(m.Binary);
                    answers_binary.Add(n.Binary);
                }
            }

        }
        
        return answers;
    }

    private bool list_contains_string(List<string> list, string comp)
    {
        foreach(string l in list)
        {
            if (l.Contains(comp))
            {
                Console.WriteLine("true");
                return true;
            }
        }
        Console.WriteLine("false");
        return false;
    }

}