using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace game.story
{

    [TestClass()]
    public class NightTest
    {
        Game gm;

        [TestMethod()]
        public void test1()
        {
            gm = game.GameTest.initTest();

            //Arrange
            gm.story.setState(6);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach(var p in gm.players.players)
            {
                Assert.IsTrue(p.fnetWait);
            }
            
        }

        [TestMethod()]
        public void test2()
        {
            gm = game.GameTest.initTest();

            setNightYes("p1", true);
            setNightYes("p2", true);
            setNightYes("p3", false);

            //---

            gm.story.setState(7);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }
            Assert.IsTrue(gm.fcapativity);

        }
        [TestMethod()]
        public void test3()
        {
            gm = game.GameTest.initTest();

            setNightYes("p1", false);
            setNightYes("p2", true);
            setNightYes("p3", false);

            //---

            gm.story.setState(7);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsFalse(p2.fnetWait);
            }
            Assert.IsFalse(gm.fcapativity);


        }

        [TestMethod()]
        public void test4()
        {
            gm = game.GameTest.initTest();

            gm.fcapativity = true;
            setNightPlayer("p1", "p2");
            setNightPlayer("p2", "");
            setNightPlayer("p3", "p2");

            //---

            gm.story.setState(8);
            gm.story.update();

            //Assert
            //The object has a new name
            Assert.AreEqual(gm.captivityName, "p2");

        }

        //--------------------------------------------------------
        private void setNightYes(string name,bool fyes)
        {
            net.NightCode1 nc = new net.NightCode1();
            nc.fyes = fyes;
            var p = gm.players.getPlayer(name);
            p.night1 = nc;
            
        }
        private void setNightPlayer(string name, string name2)
        {
            net.NightCode2 nc = new net.NightCode2();
            nc.name = name2;
            var p = gm.players.getPlayer(name);
            p.night2 = nc;

        }
    }

}