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
    public class MidNightSubTests
    {
        [TestMethod()]
        public void killTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 2, ITEM.KNIFE);
                sub.kill(gm.shareData.players.getPlayer(1));

                GameTest.assertLive(1);
                GameTest.assertMurderer(1, 2);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertLive(3);
                GameTest.assertLive(4);

                if (gm.shareData.players.getPlayer(1).getItem(0) == ITEM.MURDERE_KNIFE)
                {
                    gm.shareData.players.getPlayer(1).setItem(0, ITEM.NONE);
                }
                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
            }
        }


        [TestMethod()]
        public void killTest3()
        {
            for (int i = 0; i < 1000; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 2, ITEM.KNIFE);
                setKnife(gm, 2, 3, ITEM.KNIFE);
                sub.kill(gm.shareData.players.getPlayer(2));
                sub.kill(gm.shareData.players.getPlayer(1));

                GameTest.assertLive(1);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertDead(3, DEAD_REASON.KNIFE, 2);
                GameTest.assertLive(4);

                if (gm.shareData.players.getPlayer(1).getItem(0) == ITEM.MURDERE_KNIFE)
                {
                    gm.shareData.players.getPlayer(1).setItem(0, ITEM.NONE);
                }
                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }
        [TestMethod()]
        public void killTest5()
        {
            for (int i = 0; i < 1000; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 2, ITEM.KNIFE);
                setKnife(gm, 2, 3, ITEM.MURDERE_KNIFE);
                sub.kill(gm.shareData.players.getPlayer(2));
                sub.kill(gm.shareData.players.getPlayer(1));

                GameTest.assertLive(1);
                GameTest.assertDead(3, DEAD_REASON.MURDERER_KNIFE, 2);
                GameTest.assertLive(2);
                GameTest.assertLive(4);

                gm.shareData.players.getPlayer(2).setItem(1, ITEM.NONE);
                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }
        [TestMethod()]
        public void killTest6()
        {
            for (int i = 0; i < 1000; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 3, ITEM.KNIFE);
                setKnife(gm, 2, 3, ITEM.MURDERE_KNIFE);
                GameTest.setItem(gm, "p3", ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE);
                sub.kill(gm.shareData.players.getPlayer(2));
                sub.kill(gm.shareData.players.getPlayer(1));

                GameTest.assertLive(1);
                GameTest.assertLive(2);
                GameTest.assertDead(3, DEAD_REASON.MURDERER_KNIFE, 2);
                GameTest.assertLive(4);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }

        private void setKnife(Game gm, int id, int dest, ITEM item)
        {
            var p = gm.shareData.players.getPlayer(id);

            GameTest.setItem(gm, p.name, item, ITEM.NONE, ITEM.NONE, ITEM.NONE);
            p.net_item = 0;
            p.net_opp = dest;
            p.usedItem();
        }

        //-------------------------------------------------

        [TestMethod()]
        public void plusMurdererTest()
        {
            Game gm = GameTest.createGame();
            MidNightSub sub = new MidNightSub();
            var p1 = gm.shareData.players.getPlayer(1);

            gm.shareData.field.dementDay = 2;

            p1.murderer = true;
            p1.murdererTurn = 1;

            sub.plusMurderer(p1);

            Assert.AreEqual(false, p1.fdead);
            Assert.AreEqual(2, p1.murdererTurn);
        }

        [TestMethod()]
        public void plusMurdererTest2()
        {
            Game gm = GameTest.createGame();
            GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            MidNightSub sub = new MidNightSub();
            var p1 = gm.shareData.players.getPlayer(1);
            gm.shareData.field.dementDay = 4;
            p1.murderer = true;
            p1.murdererTurn = 5;

            sub.plusMurderer(p1);

            Assert.AreEqual(true, p1.fdead);
            Assert.AreEqual(DEAD_REASON.HAKKYO, p1.deadReason);

            Assert.AreEqual(gm.shareData.players.getPlayer(1).getItemIndex(ITEM.MURDERE_KNIFE), -1);
            if (gm.shareData.players.getPlayer(2).getItemIndex(ITEM.MURDERE_KNIFE) != -1 ||
               gm.shareData.players.getPlayer(3).getItemIndex(ITEM.MURDERE_KNIFE) != -1 ||
               gm.shareData.players.getPlayer(4).getItemIndex(ITEM.MURDERE_KNIFE) != -1
               )
            {
            }
            else
            {
                Assert.Fail();
            }

        }


        [TestMethod()]
        public void getRandomFirstDscovererTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();
                gm.shareData.players.getPlayer(1).dead(DEAD_REASON.HAKKYO, 0);

                var p1 = sub.getRandomFirstDscoverer();
                Assert.AreNotEqual(1, p1.id);
            }
        }

        [TestMethod()]
        public void unuseKnifeTest()
        {
            Game gm = GameTest.createGame();
            MidNightSub sub = new MidNightSub();

            setKnife(gm, 1, 2, ITEM.KNIFE);
            setKnife(gm, 2, 3, ITEM.KNIFE);
            setKnife(gm, 3, 4, ITEM.KNIFE);
            gm.shareData.players.getPlayer(1).usedItem();
            gm.shareData.players.getPlayer(2).usedItem();
            gm.shareData.players.getPlayer(3).usedItem();

            Assert.AreEqual(3, gm.shareData.players.getUseItemPlayers(ITEM.KNIFE).Count);
            sub.unuseKnife(2);
            Assert.AreEqual(2, gm.shareData.players.getUseItemPlayers(ITEM.KNIFE).Count);

        }

        [TestMethod()]
        public void killKnifeLoopTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 2, ITEM.KNIFE);
                setKnife(gm, 2, 3, ITEM.KNIFE);
                setKnife(gm, 3, 1, ITEM.KNIFE);
                sub.killKnifeLoop();

                GameTest.assertDead(1, DEAD_REASON.KNIFE, 3);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertDead(3, DEAD_REASON.KNIFE, 2);
                GameTest.assertLive(4);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(3, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
            }

        }
        [TestMethod()]
        public void killKnifeLoopTest2()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();

                setKnife(gm, 1, 2, ITEM.KNIFE);
                setKnife(gm, 2, 1, ITEM.KNIFE);
                sub.killKnifeLoop();

                GameTest.assertDead(1, DEAD_REASON.KNIFE, 2);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertLive(3);
                GameTest.assertLive(4);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
            }

        }

        [TestMethod()]
        public void randMurdererKnifeMoveTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                MidNightSub sub = new MidNightSub();
                GameTest.setItem(gm, "p1", ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE);
                gm.shareData.players.getPlayer(1).dead(DEAD_REASON.HAKKYO, 0);

                sub.randMurdererKnifeMove();

                Player p = null;
                foreach (var p2 in gm.shareData.players.players)
                {
                    if (p2.getItemIndex(ITEM.MURDERE_KNIFE) != -1)
                    {
                        p = p2;
                        break;
                    }
                }
                Assert.AreNotEqual(null, p);
                Assert.AreNotEqual(1, p.id);
            }
        }


        //---------------------------------------------------

        [TestMethod()]
        public void usedKensikitTest()
        {
            Game gm = GameTest.createGame();
            MidNightSub sub = new MidNightSub();
            GameTest.setItem(gm, "p1", ITEM.KENSIKIT, ITEM.NONE, ITEM.NONE, ITEM.NONE);

            var p1 = gm.shareData.players.getPlayer(1);
            var p2 = gm.shareData.players.getPlayer(2);
            p1.net_item = 0;
            p1.net_opp = 2;
            p1.usedItem();

            var p1r = sub.usedKensikit(p1);
            Assert.AreEqual(p1r.reason, DEAD_REASON.LIVE);
            Assert.AreEqual(p1r.target.id, 2);
            Assert.AreEqual(sub.usedKensikit(p2),null);
            Assert.AreEqual(p1.getItem(0), ITEM.NONE);
        }
        [TestMethod()]
        public void usedKensikitTest2()
        {
            Game gm = GameTest.createGame();
            MidNightSub sub = new MidNightSub();
            GameTest.setItem(gm, "p1", ITEM.KENSIKIT, ITEM.NONE, ITEM.NONE, ITEM.NONE);

            var p1 = gm.shareData.players.getPlayer(1);
            var p2 = gm.shareData.players.getPlayer(2);
            p1.net_item = 0;
            p1.net_opp = 2;
            p1.usedItem();
            p2.fdead = true;
            p2.deadReason = DEAD_REASON.MURDERER_KNIFE;

            var p1r = sub.usedKensikit(p1);
            Assert.AreEqual(p1r.reason, DEAD_REASON.MURDERER_KNIFE);
            Assert.AreEqual(p1r.target.id, 2);
            Assert.AreEqual(sub.usedKensikit(p2), null);
            Assert.AreEqual(p1.getItem(0), ITEM.NONE);
        }

        //---------------------------------------------------

        [TestMethod()]
        public void usedTantikiTest()
        {
            Game gm = GameTest.createGame();
            MidNightSub sub = new MidNightSub();
            GameTest.setItem(gm, "p1", ITEM.KENTIKI, ITEM.NONE, ITEM.NONE, ITEM.NONE);
            GameTest.setItem(gm, "p2", ITEM.MURDERE_KNIFE, ITEM.MURDERE_KNIFE, ITEM.MURDERE_KNIFE, ITEM.MURDERE_KNIFE);

            var p1 = gm.shareData.players.getPlayer(1);
            var p2 = gm.shareData.players.getPlayer(2);
            p1.net_item = 0;
            p1.net_opp = 2;
            p1.usedItem();

            var p1r = sub.usedTantiki(p1);
            Assert.AreEqual(p1r.item, ITEM.KNIFE);
            Assert.AreEqual(p1r.count, 4);
            Assert.AreEqual(p1r.target.id, 2);
            Assert.AreEqual(sub.usedTantiki(p2), null);
            Assert.AreEqual(p1.getItem(0), ITEM.NONE);
        }
    }
}