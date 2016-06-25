using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MyRandom
{
    public static Random cRandom = new Random(Environment.TickCount);

    public static int rand()
    {
        return cRandom.Next();
    }
    public static int rand(int min,int max)
    {
        if (max <= min) return 0;
        return cRandom.Next(min,max);
    }
}
