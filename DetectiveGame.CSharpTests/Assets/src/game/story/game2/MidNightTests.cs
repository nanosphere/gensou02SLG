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
    public class MidNightTests
    {
        [TestMethod()]
        public void MidNight1Test()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }

                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);

                sendCmd(1, 2, 0);
                sendCmd(2, 0, -1);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                sendCmd2(1, 2,
                    new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                if(GameFactory.getGame().shareData.players.getPlayer(3).state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT)
                {
                    Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_END);
                }
                else
                {
                    Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);
                }

                sendCmd2(GameFactory.getGame().shareData.players.getPlayer(2).discoverer, 2,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(1);
                GameTest.assertMurderer(1, 2);
                GameTest.assertDead(2, DEAD_REASON.MURDERER_KNIFE, 1);
                GameTest.assertLive(3);

                GameTest.assertItem(1, new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(GameFactory.getGame().shareData.players.getPlayer(2).discoverer,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }
        

        [TestMethod()]
        public void MidNight1Test3()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                sendCmd(1, 2, 0);
                sendCmd(2, 3, 0);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                sendCmd2(2, 3,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                sendCmd2(1, 2,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertDead(3, DEAD_REASON.KNIFE, 2);

                //第１発見者
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).discoverer, 4);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).discoverer, 0);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);

                sendCmd2(4, 2,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(1);
                GameTest.assertMurderer(1, 2);
                GameTest.assertMurderer(2, 3);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);
                GameTest.assertDead(3, DEAD_REASON.KNIFE, 2);
                GameTest.assertLive(4);

                GameTest.assertItem(1, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(4, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }
        
        [TestMethod()]
        public void MidNight1Test4()
        {
            for (int i = 0; i < 500; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p3", ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE);
                GameTest.setItem(gm, "p4", ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE);

                sendCmd(1, 2, 2);
                sendCmd(2, 1, 2);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);

                Assert.AreNotEqual(
                    GameFactory.getGame().shareData.players.getPlayer(3).dayDiscovere,
                    GameFactory.getGame().shareData.players.getPlayer(4).dayDiscovere);

                if (!(GameFactory.getGame().shareData.players.getPlayer(3).getItemIndex(ITEM.MURDERE_KNIFE) != -1 ||
                    GameFactory.getGame().shareData.players.getPlayer(4).getItemIndex(ITEM.MURDERE_KNIFE) != -1
                    ))
                {
                    Assert.Fail();
                }

                sendCmd2(3, GameFactory.getGame().shareData.players.getPlayer(3).dayDiscovere,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );
                sendCmd2(4, GameFactory.getGame().shareData.players.getPlayer(4).dayDiscovere,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(3);
                GameTest.assertLive(4);
                GameTest.assertDead(1, DEAD_REASON.KNIFE, 2);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 1);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(3, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(4, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }

        [TestMethod()]
        public void MidNight1Test5()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                sendCmd(1, 2, 0);
                sendCmd(2, 3, 1);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                GameTest.assertLive(1);
                GameTest.assertLive(2);
                GameTest.assertLive(4);

                sendCmd2(2, 3,
                    new ITEM[] { ITEM.KNIFE, ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                var p3 = GameFactory.getGame().shareData.players.getPlayer(3);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(p3.discoverer).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(p3.discoverer).dayDiscovere,3);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);

                sendCmd2(p3.discoverer, 3,
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );
                
                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(1);
                GameTest.assertLive(2);
                GameTest.assertLive(4);
                GameTest.assertMurderer(2, 3);
                GameTest.assertDead(3, DEAD_REASON.MURDERER_KNIFE, 2);

            }
        }

        [TestMethod()]
        public void MidNight1Test6()
        {
            for (int i = 0; i < 500; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                sendCmd(1, 3, 0);
                sendCmd(2, 3, 1);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);
                
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                //アイテム
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).deadid, 1);
                sendCmd2(1, 3,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );
                
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_END);

                //第１発見者
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).discoverer, 2);
                sendCmd2(2, 3,
                    new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );
                
                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(1);
                GameTest.assertLive(2);
                GameTest.assertLive(4);
                GameTest.assertMurderer(1, 3);
                GameTest.assertDead(3, DEAD_REASON.MURDERER_KNIFE, 1);

                GameTest.assertItem(1, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

            }
        }

        [TestMethod()]
        public void MidNight1Test7()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                sendCmd(1, 0, -1);
                sendCmd(2, 1, 1);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                Assert.AreNotEqual(GameFactory.getGame().shareData.players.getPlayer(2).getItemIndex(ITEM.MURDERE_KNIFE), -1);
                GameTest.assertMurderer(2, 1);
                GameTest.assertLive(3);
                GameTest.assertLive(4);
                GameTest.assertDead(1, DEAD_REASON.KNIFE, 2);

            }
        }
        [TestMethod()]
        public void MidNight1Test8()
        {
            for (int i = 0; i < 100; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }
                
                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                var p1 = GameFactory.getGame().shareData.players.getPlayer(1);
                p1.murderer = true;
                p1.murdererTurn = 2;

                sendCmd(1, 0, -1);
                sendCmd(2, 0, -1);
                sendCmd(3, 0, -1);
                sendCmd(4, 0, -1);
                
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                var p2 = GameFactory.getGame().shareData.players.getPlayer(GameFactory.getGame().shareData.players.getPlayer(1).discoverer);
                Assert.AreEqual(p2.state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);

                GameTest.assertDead(1, DEAD_REASON.HAKKYO, 0);
                GameTest.assertLive(2);
                GameTest.assertLive(3);
                GameTest.assertLive(4);

            }
        }

        [TestMethod()]
        public void MidNight1Test9()
        {
            for (int i = 0; i < 500; i++)
            {
                Game gm = GameTest.createGame();
                { var o = new MidNight(); o.init(); }


                GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p2", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
                GameTest.setItem(gm, "p3", ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);

                sendCmd(1, 0, -1);
                sendCmd(2, 1, 0);
                sendCmd(3, 2, 0);
                sendCmd(4, 0, -1);

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                GameTest.assertLive(2);
                GameTest.assertLive(3);
                GameTest.assertLive(4);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(3, new ITEM[] { ITEM.NONE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

                sendCmd2(2, 1,
                    new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK);

                GameTest.assertItem(1, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(2, new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });
                GameTest.assertItem(3, new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE });

                sendCmd2(3, 2,
                    new ITEM[] { ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(1).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(2).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(3).state, PLAYER_STATE.MIDNIGHT_END);
                Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(4).state, PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT);

                sendCmd2(4, 1,
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE },
                    new ITEM[] { ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE, ITEM.NONE }
                );

                Assert.IsTrue(gm.shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END));

                GameTest.assertLive(3);
                GameTest.assertLive(4);
                GameTest.assertDead(1, DEAD_REASON.KNIFE, 2);
                GameTest.assertDead(2, DEAD_REASON.KNIFE, 3);

            }
        }

        //-----------------------------------------------------------------------------------------------
        private void sendCmd(int src,int dest,int item )
        {
            var dat = game.net.CreateStoryCode.MidnightSelect(src,dest,item);
            dat.src = src;
            { var o = new MidNight(); o.run(dat); }
        }
        private void sendCmd2(int src, int dest, ITEM []srcitem, ITEM[]destitem)
        {
            var dat = game.net.CreateStoryCode.MidnightSelectItem(src, dest, srcitem,destitem);
            dat.src = src;
            { var o = new MidNight(); o.run(dat); }
        }

    }
}