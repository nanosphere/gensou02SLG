using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace common
{
    public class MyTest
    {
        public delegate bool checkFunc<T>(T t);
        public static bool AssertTestInList<T>(List<T> list , checkFunc<T> f)
        {
            foreach(var i in list)
            {
                if( f(i))
                {
                    return true;
                }
            }
            return false;
        }
    }

}