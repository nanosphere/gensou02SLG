using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MyRandom
{
    private static Random cRandom = new Random(Environment.TickCount);
    
    public static int rand(int min,int max)
    {
        if (max <= min) return 0;
        return cRandom.Next(min,max+1);
    }
    public static int[] shuffleArray(int len)
    {
        int[] n = new int[len];
        for(int i = 0; i < n.Length; i++)
        {
            n[i] = i;
        }

        for (int i = n.Length; i > 1; --i)
        {
            int a = i - 1;
            int b = cRandom.Next() % i;

            var tmp = n[a];
            n[a] = n[b];
            n[b] = tmp;
        }
        return n;
    }
}
