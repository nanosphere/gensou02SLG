using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.game1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using game.story.net;

namespace game.story.game1.Tests
{
    [TestClass()]
    public class InitGameTests
    {
        [TestMethod()]
        public void InitGameTest()
        {
            Logger.funity = false;
            GameFactory.initFacroty();
            GameFactory.debug = true;

            new AddPlayer("p1");
            new AddPlayer("p2");
            new AddPlayer("p3");

            GameFactory.getGame().localData.fgm = true;
            new InitGame();

            Assert.AreEqual(FIELD_STATE.EARLY_MORNING, GameFactory.getGame().shareData.field.state);
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(1).getItemNum());
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(2).getItemNum());
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(3).getItemNum());


        }
    }
}