using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.db.Tests
{
    [TestClass()]
    public class PlayersTests
    {
        [TestMethod()]
        public void syncTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);

            Players o2 = new Players();
            o2.sync(o);

            Assert.AreEqual(1, o2.players.Count);
            Assert.AreEqual("p1", o2.players[0].name);
            Assert.AreEqual(1, o2.players[0].id);
        }

        [TestMethod()]
        public void addPlayerTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);

            Assert.AreEqual(3, o.players.Count);
        }

        [TestMethod()]
        public void isPlayerTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            Assert.IsTrue(o.isPlayer("p3"));
            Assert.IsFalse(o.isPlayer("p5"));
            Assert.IsTrue(o.isPlayer(3));
            Assert.IsTrue(o.isPlayer(4));
            Assert.IsFalse(o.isPlayer(5));
        }

        [TestMethod()]
        public void getPlayerTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            Assert.AreEqual("p3", o.getPlayer(3).name);
            Assert.AreEqual(3, o.getPlayer("p3").id);
        }

        [TestMethod()]
        public void getUseMurdererKnifeTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            o.players[3].setItem(3, ITEM.MURDERE_KNIFE);
            o.players[3].net_item = 3;
            o.players[3].usedItem();

            Assert.AreEqual(4, o.getUseMurdererKnife().id);

        }

        [TestMethod()]
        public void getUseItemPlayersTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            o.players[2].setItem(3, ITEM.KNIFE);
            o.players[3].setItem(3, ITEM.KNIFE);
            o.players[2].net_item = 3;
            o.players[3].net_item = 3;
            o.players[2].usedItem();
            o.players[3].usedItem();

            Assert.AreEqual(2, o.getUseItemPlayers(ITEM.KNIFE).Count);

        }

        [TestMethod()]
        public void setAllStateTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            o.setAllState(PLAYER_STATE.NIGHT_SELECT_OK);
            foreach (var p in o.players)
            {
                Assert.AreEqual(p.state, PLAYER_STATE.NIGHT_SELECT_OK);
            }
        }

        [TestMethod()]
        public void isAllPlayerStateTest()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            o.setAllState(PLAYER_STATE.NIGHT_SELECT_OK);
            Assert.IsTrue(o.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_OK));
            o.getPlayer(1).state = PLAYER_STATE.NONE;
            Assert.IsFalse(o.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_OK));

        }

        [TestMethod()]
        public void isAllPlayerStateTest1()
        {
            Players o = new Players();
            o.addPlayer("p1", ai.AI_MODE.NONE);
            o.addPlayer("p2", ai.AI_MODE.NONE);
            o.addPlayer("p3", ai.AI_MODE.NONE);
            o.addPlayer("p4", ai.AI_MODE.NONE);

            o.setAllState(PLAYER_STATE.NIGHT_SELECT_OK);
            o.getPlayer(2).fdead = true;
            o.getPlayer(2).state = PLAYER_STATE.END;
            Assert.IsTrue(o.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_OK, PLAYER_STATE.NONE));
            o.getPlayer(1).state = PLAYER_STATE.NONE;
            Assert.IsTrue(o.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_OK, PLAYER_STATE.NONE));
            Assert.IsFalse(o.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_OK, PLAYER_STATE.MIDNIGHT_END));

        }
    }
}