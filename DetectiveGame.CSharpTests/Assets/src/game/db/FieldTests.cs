using Microsoft.VisualStudio.TestTools.UnitTesting;
using game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Tests
{
    [TestClass()]
    public class FieldTests
    {
        [TestMethod()]
        public void syncTest()
        {
            Field f = new Field();
            f.state = FIELD_STATE.MIDNIGHT;
            f.timer = 10;
            f.turn = 2;
            f.useKnife = 2;
            f.captivity = 3;
            f.dementDay = 3;

            Field f2 = new Field();
            f2.sync(f);
            
            Assert.AreEqual(FIELD_STATE.MIDNIGHT, f2.state);
            Assert.AreEqual(10, f2.timer);
            Assert.AreEqual(2, f2.turn);
            Assert.AreEqual(2, f2.useKnife);
            Assert.AreEqual(3, f2.captivity);
            Assert.AreEqual(3, f2.dementDay);
        }
    }
}