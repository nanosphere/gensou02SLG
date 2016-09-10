using Microsoft.VisualStudio.TestTools.UnitTesting;
using unity.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.db;



namespace unity.main.Tests
{
    [TestClass()]
    public class MainStory
    {
        [TestMethod()]
        public void test1()
        {
            game.Game gm = game.GameTest.createGame();
            MidnightSub sub = new MidnightSub();

            game.Game gm2 = new game.Game();


        }
    }
}