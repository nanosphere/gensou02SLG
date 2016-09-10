using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.game2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.db;

namespace game.story.game2.Tests
{
    [TestClass()]
    public class NightTests
    {
        [TestMethod()]
        public void NightTest()
        {
            Game gm = GameTest.createGame();

            {
                AStory night = new Night();
                night.init();
            }

            sendCmd(1, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(2, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(3, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(4, false, PLAYER_STATE.NIGHT_VOTE);

            Assert.AreEqual(gm.shareData.field.yes, 3);
            Assert.AreEqual(gm.shareData.field.no, 1);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).state, db.PLAYER_STATE.NIGHT_VOTE);
            Assert.AreEqual(gm.shareData.players.getPlayer(2).state, db.PLAYER_STATE.NIGHT_VOTE);
            Assert.AreEqual(gm.shareData.players.getPlayer(3).state, db.PLAYER_STATE.NIGHT_VOTE);
            Assert.AreEqual(gm.shareData.players.getPlayer(4).state, db.PLAYER_STATE.NIGHT_VOTE);

            sendCmd2(1, 2, PLAYER_STATE.NIGHT_VOTE_OK);
            sendCmd2(2, 0, PLAYER_STATE.NIGHT_VOTE_OK);
            sendCmd2(3, 2, PLAYER_STATE.NIGHT_VOTE_OK);
            sendCmd2(4, 1, PLAYER_STATE.NIGHT_VOTE_END);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).dayNightVote, 1);
            Assert.AreEqual(gm.shareData.players.getPlayer(2).dayNightVote, 2);
            Assert.AreEqual(gm.shareData.players.getPlayer(3).dayNightVote, 0);
            Assert.AreEqual(gm.shareData.players.getPlayer(4).dayNightVote, 0);

            Assert.AreEqual(gm.shareData.field.captivity, 2);

            // end
            Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_END));

        }

        [TestMethod()]
        public void NightTest2()
        {
            Game gm = GameTest.createGame();
            { var o = new Night(); o.init(); }

            sendCmd(1, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(2, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(3, false, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(4, false, PLAYER_STATE.NIGHT_SELECT_END);

            Assert.AreEqual(gm.shareData.field.yes, 2);
            Assert.AreEqual(gm.shareData.field.no, 2);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).state, db.PLAYER_STATE.NIGHT_SELECT_END);
            Assert.AreEqual(gm.shareData.players.getPlayer(2).state, db.PLAYER_STATE.NIGHT_SELECT_END);
            Assert.AreEqual(gm.shareData.players.getPlayer(3).state, db.PLAYER_STATE.NIGHT_SELECT_END);
            Assert.AreEqual(gm.shareData.players.getPlayer(4).state, db.PLAYER_STATE.NIGHT_SELECT_END);
            
        }

        [TestMethod()]
        public void NightTest3()
        {
            Game gm = GameTest.createGame();
            gm.shareData.players.getPlayer(2).fdead = true;
            { var o = new Night(); o.init(); }

            

            sendCmd(1, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(3, true, PLAYER_STATE.NIGHT_SELECT_OK);
            sendCmd(4, true, PLAYER_STATE.NIGHT_VOTE);

            Assert.AreEqual(gm.shareData.field.yes, 3);
            Assert.AreEqual(gm.shareData.field.no, 0);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).state, db.PLAYER_STATE.NIGHT_VOTE);
            Assert.AreEqual(gm.shareData.players.getPlayer(2).state, db.PLAYER_STATE.NIGHT_VOTE_END);
            Assert.AreEqual(gm.shareData.players.getPlayer(3).state, db.PLAYER_STATE.NIGHT_VOTE);
            Assert.AreEqual(gm.shareData.players.getPlayer(4).state, db.PLAYER_STATE.NIGHT_VOTE);

            sendCmd2(1, 2, PLAYER_STATE.NIGHT_VOTE_OK);
            sendCmd2(3, 4, PLAYER_STATE.NIGHT_VOTE_OK);
            sendCmd2(4, 4, PLAYER_STATE.NIGHT_VOTE_END);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).dayNightVote, 0);
            Assert.AreEqual(gm.shareData.players.getPlayer(2).dayNightVote, 0);
            Assert.AreEqual(gm.shareData.players.getPlayer(3).dayNightVote, 0);
            Assert.AreEqual(gm.shareData.players.getPlayer(4).dayNightVote, 2);

            Assert.AreEqual(gm.shareData.field.captivity, 4);

            // end
            Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_END));


        }



        //-------------------------------------------------------------
        private void sendCmd(int src, bool fyes,PLAYER_STATE s1)
        {
            if (fyes)
            {
                var dat = game.net.CreateStoryCode.NightYes(src);
                dat.src = src;
                { var o = new Night(); o.run(dat); }
            }
            else
            {
                var dat = game.net.CreateStoryCode.NightNo(src);
                dat.src = src;
                { var o = new Night(); o.run(dat); }
            }
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(src).state, s1);
        }
        private void sendCmd2(int src, int dest,PLAYER_STATE s1)
        {
            var dat = game.net.CreateStoryCode.NightVote(src,dest);
            dat.src = src;
            { var o = new Night(); o.run(dat); }
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(src).state, s1);
        }
    }
}