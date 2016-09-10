using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class MyRandomTests
    {

        [TestMethod()]
        public void randTest1()
        {
            int[] rand = new int[2];
            for (int i = 0; i < 1000; i++)
            {
                rand[MyRandom.rand(0, 1)]++;
            }

            Assert.IsTrue(500 - 50 < rand[0]);
            Assert.IsTrue(500 + 50 > rand[0]);
            Assert.IsTrue(500 - 50 < rand[1]);
            Assert.IsTrue(500 + 50 > rand[1]);

        }

        [TestMethod()]
        public void shuffleArrayTest()
        {
            int[] result = new int[5];
            int[] result1 = new int[5];
            for (int i = 0; i < 10000; i++)
            {
                int[] rands = MyRandom.shuffleArray(5);
                result[rands[0]]++;
                result1[rands[4]]++;
            }

            Assert.IsTrue(2000 - 100 < result[0]);
            Assert.IsTrue(2000 + 100 > result[0]);
            Assert.IsTrue(2000 - 100 < result[4]);
            Assert.IsTrue(2000 + 100 > result[4]);

            Assert.IsTrue(2000 - 100 < result1[0]);
            Assert.IsTrue(2000 + 100 > result1[0]);
            Assert.IsTrue(2000 - 100 < result1[4]);
            Assert.IsTrue(2000 + 100 > result1[4]);

        }
    }
}