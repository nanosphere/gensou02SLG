using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace game
{
    [TestClass()]
    public class GameTest
    {
        public static Game initTest()
        {
            Logger.funity = false;

            GameFactory.initFacroty();
            Game gm = GameFactory.getGame();
            gm.info.player_name = "p3";
            gm.info.fhost = true;
            gm.players.addPlayer("p1");
            gm.players.addPlayer("p2");
            gm.players.addPlayer("p3");

            gm.init();
            return gm;
        }

        [TestMethod()]
        public void test()
        {
            Game gm = initTest();

            //Assert
            //The object has a new name
            Assert.IsTrue(common.MyTest.AssertTestInList<Player>(gm.players.players, (Player p) => { return p.name == "p1"; }));
            Assert.IsTrue(common.MyTest.AssertTestInList<Player>(gm.players.players, (Player p) => { return p.name == "p2"; }));
            Assert.IsTrue(common.MyTest.AssertTestInList<Player>(gm.players.players, (Player p) => { return p.name == "p3"; }));

            Assert.IsTrue(gm.item_pool.Count >= 12);

        }
    }

}