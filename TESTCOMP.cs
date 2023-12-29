using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TESTCOMP : IComparable<TESTCOMP>
{
    public int powah = 1;

    //random??
    public TESTCOMP(int i)
    {
        powah = i;
    }

    public int CompareTo(TESTCOMP other)
    {
        if(other != null)
        {
            return powah - other.powah;
        }
        else
        {
            return 1;
        }
    }
}

public class COMPSTOMP
{
    public int i;

    public COMPSTOMP(int i)
    {
        this.i = i;
    }

    public override string ToString()
    {
        return "" + i;
    }
}

public class POMPSTOMP : COMPSTOMP
{
    public int b = 1;

    public POMPSTOMP(int i) : base(i) { }
}
