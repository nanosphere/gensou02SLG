using Microsoft.VisualStudio.TestTools.UnitTesting;
using game.story.net;
using game.story.game1;


namespace game
{
    [TestClass()]
    public class GameTest
    {
        public static Game createGame()
        {
            Logger.funity = false;
            GameFactory.initFacroty();
            GameFactory.debug = true;

            new AddPlayer("p1");
            new AddPlayer("p2");
            new AddPlayer("p3");
            new AddPlayer("p4");

            GameFactory.getGame().shareData.field.aiNum = 0;
            GameFactory.getGame().localData.fgm = true;
            GameFactory.getGame().localData.myPlayer = GameFactory.getGame().shareData.players.getPlayer("p1").id;
            new InitGame();

            
            return GameFactory.getGame();
        }
        public static void setItem(Game gm , string name, game.db.ITEM item1, db.ITEM item2, db.ITEM item3, db.ITEM item4)
        {
            var p = gm.shareData.players.getPlayer(name);
            p.items[0] = item1;
            p.items[1] = item2;
            p.items[2] = item3;
            p.items[3] = item4;
        }



        public static void assertItem(int id, db.ITEM[] items)
        {
            var p = GameFactory.getGame().shareData.players.getPlayer(id);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.AreEqual(p.getItem(i), items[i]);
            }
        }
        public static void assertDead(int id, db.DEAD_REASON reason, int player)
        {
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(id).fdead, true);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(id).deadReason, reason);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(id).deadid, player);

        }
        public static void assertLive(int name)
        {
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(name).fdead, false);
        }
        public static void assertMurderer(int name, int player)
        {
            Assert.AreNotEqual(GameFactory.getGame().shareData.players.getPlayer(name).dayKill, 0);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(name).dayKill, player);
            Assert.AreEqual(GameFactory.getGame().shareData.players.getPlayer(name).murderer, true);
           
        }


        [TestMethod()]
        public void InitGameTest()
        {
            Game gm = createGame();

            Assert.AreEqual(4, GameFactory.getGame().shareData.players.players.Count);
            Assert.AreEqual(FIELD_STATE.EARLY_MORNING, GameFactory.getGame().shareData.field.state);
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(1).getItemNum());
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(2).getItemNum());
            Assert.AreEqual(4, GameFactory.getGame().shareData.players.getPlayer(3).getItemNum());


        }
    }

}