using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TESTER : MonoBehaviour
{
        
    void Start()
    {
        Frogen frog = new Frogen();
        frog.TestForFrogen();
    }

    public class Frogen
    {
        private void PrMethod()
        {
            Debug.Log("Frogen Bruh");
        }

        public void TestForFrogen()
        {
            Pogen rog = new Pogen();
            Frogen fr = rog;

            //  When PrMethod on Pogen is private, calls Frogen Bruh here because in this case it only knows
            //  of the Frogen specific version of this method. Since that is still contained within Pogen it
            //  can still be called if direcetly targeted like this.

            //rog.PrMethod(); 

            rog.PrMethod();
            fr.PrMethod();
        }

    }

    public class Pogen : Frogen 
    { 
        public void PrMethod()
        {
            Debug.Log("Pogen Bruh");
        }

    }




    public void Test2()
    {
        Action<Mango> aMango = (Mango m) => Debug.Log(m);
        Action<Mango> aFruit = (Mango m) => Debug.Log(m);

        aMango = aFruit;
        aMango += (Mango m) => m.Fr();

        aMango(new Mango());

        //Fruit fruit = new Mango();
        //Mango mango = (Mango)fruit;
        //mango.Fr();
    }

    public class Fruit 
    {
        public void Fr()
        {
            Debug.Log("fruitty");
        }

        public override string ToString()
        {
            return "this is a fruit";
        }
    }

    public class Mango : Fruit 
    {
        new public void Fr()
        {
            Debug.Log("mangoy");
        }

        public override string ToString()
        {
            return "this is a mango";
        }
    }



    public class InfIEnum : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return new actualEnum();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new actualEnum();
        }
    }

    public class actualEnum : IEnumerator<int>
    {
        int i = 0;

        public int Current => i;

        object IEnumerator.Current => Current + 44;

        

        public bool MoveNext()
        {
            i++;

            return i < 5;
        }

        public void Reset()
        {
            i = 0;
        }

        public void Dispose() { }
    }
}
