using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.game2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using game.story.game1;
using game.db;

namespace game.story.game2.Tests
{
    [TestClass()]
    public class NoonTests
    {
        [TestMethod()]
        public void NoonTest()
        {
            Game gm = GameTest.createGame();

            { var o = new Noon(); o.init(); }


            GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);
            GameTest.setItem(gm, "p3", ITEM.KENSIKIT, ITEM.KENSIKIT, ITEM.KENSIKIT, ITEM.KENSIKIT);

            sendCmd(1, 2,  PLAYER_STATE.NOON_WAIT_ACK, PLAYER_STATE.NOON_REQUEST_RETURN);
            sendCmd2(2, 1, true, PLAYER_STATE.NOON_ITEM, PLAYER_STATE.NOON_ITEM);
            sendCmd3(1, 1, PLAYER_STATE.NOON_ITEM_OK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").state, db.PLAYER_STATE.NOON_ITEM);
            sendCmd3(2, 1, PLAYER_STATE.NONE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").state, db.PLAYER_STATE.NONE);

            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[0], ITEM.MURDERE_KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[1], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[2], ITEM.KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[3], ITEM.KNIFE);

            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[0], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[1], ITEM.KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[2], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[3], ITEM.CHEAN_LOCK);

            Assert.AreEqual(gm.shareData.players.getPlayer("p3").items[0], ITEM.KENSIKIT);
            Assert.AreEqual(gm.shareData.players.getPlayer("p3").items[1], ITEM.KENSIKIT);
            Assert.AreEqual(gm.shareData.players.getPlayer("p3").items[2], ITEM.KENSIKIT);
            Assert.AreEqual(gm.shareData.players.getPlayer("p3").items[3], ITEM.KENSIKIT);

            // end
            sendCmd4(1);
            sendCmd4(2);
            sendCmd4(3);
            sendCmd4(4);
            Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.NOON_END));

        }


        [TestMethod()]
        public void NoonTest2()
        {
            Game gm = GameTest.createGame();
            { var o = new Noon(); o.init(); }

            GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);

            sendCmd4(3);
            sendCmd4(4);

            //---------------------------------------------
            sendCmd(1, 2,  PLAYER_STATE.NOON_WAIT_ACK, PLAYER_STATE.NOON_REQUEST_RETURN);
            sendCmd2(2, 1, true,  PLAYER_STATE.NOON_ITEM, PLAYER_STATE.NOON_ITEM);
            sendCmd3(1, 1, PLAYER_STATE.NOON_ITEM_OK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").state, db.PLAYER_STATE.NOON_ITEM);
            sendCmd3(2, 1, PLAYER_STATE.NONE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").state, db.PLAYER_STATE.NONE);
            //---------------------------------------------
            sendCmd4(1);
            sendCmd4(2);

            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[0], ITEM.MURDERE_KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[1], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[2], ITEM.KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p1").items[3], ITEM.KNIFE);

            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[0], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[1], ITEM.KNIFE);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[2], ITEM.CHEAN_LOCK);
            Assert.AreEqual(gm.shareData.players.getPlayer("p2").items[3], ITEM.CHEAN_LOCK);


            Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.NOON_END));

        }
        
        //-----------------------------------------------------------------------------------------------
        private void sendCmd(int src,int dest, PLAYER_STATE s1, PLAYER_STATE s2)
        {
            var dat = game.net.CreateStoryCode.NoonRequest(src,dest);
            { var o = new Noon(); o.run(dat); }

            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(src).state, s1);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(dest).state, s2);
        }
        private void sendCmd2(int src,int dest,bool fyes, PLAYER_STATE s1, PLAYER_STATE s2)
        {
            if (fyes) {
                var dat = game.net.CreateStoryCode.NoonYes(src);
                dat.src = src;
                dat.dest = dest;
                { var o = new Noon(); o.run(dat); }
            }
            else
            {
                var dat = game.net.CreateStoryCode.NoonNo(src);
                dat.src = src;
                dat.dest = dest;
                { var o = new Noon(); o.run(dat); }
            }
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(src).state, s1);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(dest).state, s2);
        }
        private void sendCmd3(int src, int item, PLAYER_STATE s1)
        {
            var dat = game.net.CreateStoryCode.NoonItem(src,item);
            dat.src = src;
            { var o = new Noon(); o.run(dat); }
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(src).state, s1);
        }
        private void sendCmd4(int src)
        {
            var dat = game.net.CreateStoryCode.NoonEnd(src);
            dat.src = src;
            { var o = new Noon(); o.run(dat); }
        }

    }
}