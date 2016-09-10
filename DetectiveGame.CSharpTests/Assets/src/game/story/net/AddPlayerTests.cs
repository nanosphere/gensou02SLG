using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.story.net.Tests
{
    [TestClass()]
    public class AddPlayerTests
    {
        [TestMethod()]
        public void AddPlayerTest()
        {
            Logger.funity = false;
            GameFactory.initFacroty();

            new AddPlayer("p1");
            new AddPlayer("p2");
            new AddPlayer("p3");
            new AddPlayer("p2");

            Assert.AreEqual(3, GameFactory.getGame().shareData.players.players.Count);

        }
    }
}