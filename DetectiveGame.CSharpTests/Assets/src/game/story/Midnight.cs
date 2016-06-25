using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace game.story
{

    [TestClass()]
    public class MidnightTest
    {
        Game gm;

        [TestMethod()]
        public void test1()
        {
            gm = game.GameTest.initTest();

            //Arrange
            gm.story.setState(9,true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach(var p in gm.players.players)
            {
                Assert.IsTrue(p.fnetWait);
            }
        }

        [TestMethod()]
        public void murder1()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 3, 4, 5);
            setItem("p3", 5, 5, 5, 5);

            setMindnight1("p1", 0, "p2");
            setMindnight1("p2", -1, "p2");
            setMindnight1("p3", -1, "p2");
            
            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertMurderer("p1","p2");
            assertDead("p2", "狂気の殺人包丁", "p1");
            assertLive("p3");

            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 3, 4, 5);
            assertItem("p3", 5, 5, 5, 5);
            

        }

        [TestMethod()]
        public void murder2()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 4, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", 1, "p2");
            setMindnight1("p2", 1, "p3");
            setMindnight1("p3", 1, "p1");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            int f = 0;
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
                if (p2.fdead)
                {
                    Assert.AreEqual(p2.deadReason, "包丁");
                    Assert.AreEqual(p2.getItems().Count, 3);
                    f++;
                }else
                {
                    Assert.AreEqual(p2.murderer, true);
                    Assert.AreEqual(p2.getItems().Count, 3);
                }
            }
            Assert.AreEqual(f, 2);

        }

        [TestMethod()]
        public void murder3()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", -1, "");
            setMindnight1("p2", 1, "p3");
            setMindnight1("p3", 1, "p2");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertDead("p2", "包丁", "p3");
            assertDead("p3", "包丁", "p2");
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 0, 2, 2);
            assertItem("p3", 2, 0, 2, 2);

        }
        [TestMethod()]
        public void murder4()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", 0, "p2");
            setMindnight1("p2", 3, "p1");
            setMindnight1("p3", -1, "p2");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertMurderer("p1", "p2");
            assertDead("p2", "狂気の殺人包丁", "p1");
            assertLive("p3");
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 2, 2, 0);
            assertItem("p3", 2, 2, 2, 2);

        }

        [TestMethod()]
        public void murder5()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", 0, "p2");
            setMindnight1("p2", 3, "p3");
            setMindnight1("p3", -1, "p2");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertMurderer("p1", "p2");
            assertDead("p2", "狂気の殺人包丁", "p1");
            assertLive("p3");
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 2, 2, 0);
            assertItem("p3", 2, 2, 2, 2);

        }
        [TestMethod()]
        public void murder6()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", 0, "p3");
            setMindnight1("p2", 3, "p1");
            setMindnight1("p3", -1, "p2");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertMurderer("p1", "p3");
            assertLive("p2");
            assertDead("p3", "狂気の殺人包丁", "p1");
            Assert.AreEqual(gm.players.getPlayer("p2").murderer, false);
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 2, 2, 0);
            assertItem("p3", 2, 2, 2, 2);

        }
        [TestMethod()]
        public void murder7()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 2, 2, 2);
            setItem("p3", 2, 2, 2, 2);

            setMindnight1("p1", -1, "p3");
            setMindnight1("p2", 3, "p1");
            setMindnight1("p3", -1, "p2");

            //---
            gm.story.setState(10, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertDead("p1", "包丁", "p2");
            assertLive("p2");
            assertMurderer("p2", "p1");
            assertLive("p3");
            assertItem("p1", 1, 2, 3, 4);
            assertItem("p2", 2, 2, 2, 1);
            assertItem("p3", 2, 2, 2, 2);

        }
        [TestMethod()]
        public void itemget1()
        {
            gm = game.GameTest.initTest();

            setItem("p1", 1, 2, 3, 4);
            setItem("p2", 2, 3, 4, 5);
            setItem("p3", 2, 2, 2, 2);

            setMindnight2("p1", 1, 4,4,4, 2,2,2,2);
            setMindnight2("p2", -1,-1,-1,-1, -1, -1, -1, -1);
            setMindnight2("p3", -1, -1, -1, -1, -1, -1, -1, -1);

            gm.players.getPlayer("p2").MidnightDead("p1","");

            //---
            gm.story.setState(11, true);
            gm.story.update();

            //Assert
            //The object has a new name
            foreach (var p2 in gm.players.players)
            {
                Assert.IsTrue(p2.fnetWait);
            }

            assertLive("p1");
            assertLive("p3");
            assertDead("p2", "", "p1");
            assertItem("p1", 1, 4, 4, 4);
            assertItem("p2", 2, 2, 2, 2);
            assertItem("p3", 2, 2, 2, 2);

        }

        //--------------------------------------------------------
        private void setMindnight1(string name, int item, string name2)
        {
            net.MidnightCode1 o = new net.MidnightCode1();
            o.item = item;
            o.name = name2;
            gm.players.getPlayer(name).midnight1 = o;
        }
        private void setMindnight2(string name, int item1, int item2, int item3, int item4, int ditem1, int ditem2, int ditem3, int ditem4)
        {
            net.MidnightCode2 o = new net.MidnightCode2();
            o.myItems.Add(item1);
            o.myItems.Add(item2);
            o.myItems.Add(item3);
            o.myItems.Add(item4);
            o.deadItems.Add(ditem1);
            o.deadItems.Add(ditem2);
            o.deadItems.Add(ditem3);
            o.deadItems.Add(ditem4);
            gm.players.getPlayer(name).midnight2 = o;
        }

        private void setItem(string name, int item1, int item2, int item3, int item4)
        {
            var p = gm.players.getPlayer(name);
            p.items[0] = item1;
            p.items[1] = item2;
            p.items[2] = item3;
            p.items[3] = item4;

        }
        private void assertItem(string name, int item1, int item2, int item3, int item4)
        {
            var p = gm.players.getPlayer(name);
            Assert.AreEqual(p.items[0], item1);
            Assert.AreEqual(p.items[1], item2);
            Assert.AreEqual(p.items[2], item3);
            Assert.AreEqual(p.items[3], item4);

        }


        private void assertDead(string name,string reason , string player)
        {
            Assert.AreEqual(gm.players.getPlayer(name).fdead, true);
            Assert.AreEqual(gm.players.getPlayer(name).deadReason, reason);
            Assert.AreEqual(gm.players.getPlayer(name).deadName, player);

        }
        private void assertLive(string name)
        {
            Assert.AreEqual(gm.players.getPlayer(name).fdead, false);
        }
        private void assertMurderer(string name , string player)
        {
            Assert.AreEqual(gm.players.getPlayer(name).kill, 1);
            Assert.AreEqual(gm.players.getPlayer(name).killName, player);
            Assert.AreEqual(gm.players.getPlayer(name).murderer, true);
            Assert.AreEqual(gm.players.getPlayer(name).murdererTurn, 1);

        }
    }

}