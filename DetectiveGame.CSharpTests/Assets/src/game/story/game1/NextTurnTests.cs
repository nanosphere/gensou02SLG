using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.game1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.story.game1.Tests
{
    [TestClass()]
    public class NextTurnTests
    {
        [TestMethod()]
        public void NextTurnTest()
        {
            Game gm = GameTest.createGame();

            new NextTurn();
            Assert.AreEqual(FIELD_STATE.MORNING, GameFactory.getGame().shareData.field.state);

            new NextTurn();
            Assert.AreEqual(FIELD_STATE.NOON, GameFactory.getGame().shareData.field.state);

        }
    }
}