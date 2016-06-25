using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace game.story
{

    [TestClass()]
    public class NoonTest
    {
        Game gm;

        [TestMethod()]
        public void test1()
        {
            gm = game.GameTest.initTest();

            //Arrange
            gm.story.setState(3,true);

            //Act
            //Try to rename the GameObject
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
            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 3, 4, 5);
            setItem("p3", 5, 5, 5, 5);

            setSNoonCode1("p1", 1, "p2");
            setSNoonCode1("p2", -1, "p2");
            setSNoonCode1("p3", 0, "p2");

            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 3, 4, 5);
            assertItem("p3", 5, 5, 5, 5);
            //---

            gm.story.setState(4, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 3, 4, 5);
            assertItem("p3", 5, 5, 5, 5);

        }

        [TestMethod()]
        public void test3()
        {
            gm = game.GameTest.initTest();
            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 3, 4, 5);
            setItem("p3", 5, 5, 5, 5);

            setSNoonCode1("p1", 1, "p2");
            setSNoonCode1("p2", -1, "p2");
            setSNoonCode1("p3", 0, "p2");

            //---
            net.NoonCode2 nc;

            nc = new net.NoonCode2();
            setNoonCode2(nc, -1, "p1");
            setNoonCode2(nc, -1, "p2");
            setNoonCode2(nc, -1, "p3");
            gm.players.getPlayer("p1").noon2 = nc;
            gm.players.getPlayer("p1").fnetWait = false;

            nc = new net.NoonCode2();
            setNoonCode2(nc, 3, "p1");
            setNoonCode2(nc, -1, "p2");
            setNoonCode2(nc, 2, "p3");
            gm.players.getPlayer("p2").noon2 = nc;
            gm.players.getPlayer("p2").fnetWait = false;

            nc = new net.NoonCode2();
            setNoonCode2(nc, -1, "p1");
            setNoonCode2(nc, -1, "p2");
            setNoonCode2(nc, -1, "p3");
            gm.players.getPlayer("p3").noon2 = nc;
            gm.players.getPlayer("p3").fnetWait = false;

            foreach (var p2 in gm.players.players)
            {
                Assert.IsFalse(p2.fnetWait);
            }
            //---

            gm.story.setState(5, true);
            gm.story.update();

            //Assert
            //The object has a new name
            assertItem("p1", 1, 5, 3, 4);
            assertItem("p2", 2, 3, 5, 2);
            assertItem("p3", 4, 5, 5, 5);
            
        }

        //--------------------------------------------------------
        private void setItem(string name,int item1, int item2, int item3, int item4)
        {
            var p = gm.players.getPlayer(name);
            p.items[0] = item1;
            p.items[1] = item2;
            p.items[2] = item3;
            p.items[3] = item4;
            
        }
        private void assertItem(string name, int item1, int item2, int item3, int item4)
        {
            Assert.AreEqual(gm.players.getPlayer(name).items[0], item1);
            Assert.AreEqual(gm.players.getPlayer(name).items[1], item2);
            Assert.AreEqual(gm.players.getPlayer(name).items[2], item3);
            Assert.AreEqual(gm.players.getPlayer(name).items[3], item4);

        }

        private void setSNoonCode1(string name,int item,string name2)
        {
            net.NoonCode1 o = new net.NoonCode1();
            o.item = item;
            o.name = name2;
            gm.players.getPlayer(name).noon1 = o;
        }
        private void setNoonCode2(net.NoonCode2 nc,int item,string name)
        {
            net.NoonCode2Obj nco;
            nco = new net.NoonCode2Obj();
            nco.name = name;
            nco.item = item;
            nc.players.Add(nco);
        }
        

    }

}