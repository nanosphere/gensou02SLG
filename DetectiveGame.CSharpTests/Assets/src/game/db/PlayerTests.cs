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
    public class PlayerTests
    {

        [TestMethod()]
        public void addItemTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.addItem(ITEM.CHEAN_LOCK);
            p.addItem(ITEM.CHEAN_LOCK);

            Assert.AreEqual(ITEM.CHEAN_LOCK, p.getItem(0));
            Assert.AreEqual(ITEM.KNIFE, p.getItem(1));
            Assert.AreEqual(ITEM.CHEAN_LOCK, p.getItem(2));
        }

        [TestMethod()]
        public void getItemNumTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;

            Assert.AreEqual(2, p.getItemNum());
        }

        [TestMethod()]
        public void getItemTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;

            Assert.AreEqual(ITEM.KNIFE, p.getItem(1));
            Assert.AreEqual(ITEM.NONE, p.getItem(-1));
        }

        [TestMethod()]
        public void addMessageTest()
        {
            Player p = new Player();
            p.addMessage("a");
            int len = p.message.Length;
            Assert.AreNotEqual("", p.message);
            p.addMessage("a");
            Assert.AreNotEqual("", p.message);
            Assert.IsTrue(len < p.message.Length);
        }

        [TestMethod()]
        public void usedItemTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;

            p.net_item = -1;
            p.usedItem();
            p.net_item = 1;
            p.usedItem();
            Assert.AreEqual(ITEM.NONE, p.getItem(1));
        }

        [TestMethod()]
        public void deadTest()
        {
            Player p = new Player();

            p.dead(DEAD_REASON.KNIFE, 2);
            Assert.IsTrue(p.dayDead);
            Assert.IsTrue(p.fdead);
            Assert.AreEqual(DEAD_REASON.KNIFE, p.deadReason);
            Assert.AreEqual(2, p.deadid);
            Assert.AreEqual(0, p.discoverer);
        }

        [TestMethod()]
        public void killSuccessTest()
        {
            Player p = new Player();

            p.killSuccess(2);
            Assert.IsTrue(p.murderer);
            Assert.AreEqual(2, p.dayKill);
            Assert.AreEqual(0, p.murdererTurn);
        }

        [TestMethod()]
        public void getItemIndexTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;

            Assert.AreEqual(1, p.getItemIndex(ITEM.KNIFE));
            Assert.AreEqual(-1, p.getItemIndex(ITEM.MURDERE_KNIFE));
        }

        [TestMethod()]
        public void setItemTest()
        {
            Player p = new Player();
            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;
            p.setItem(2, ITEM.KNIFE);
            p.setItem(-1, ITEM.KNIFE);

            Assert.AreEqual(ITEM.KNIFE, p.getItem(2));
        }

        [TestMethod()]
        public void getReasonTest()
        {
            Player p = new Player();

            Assert.AreEqual("", p.getReason());
            p.deadReason = DEAD_REASON.KNIFE;
            Assert.AreEqual("包丁", p.getReason());
        }

        [TestMethod()]
        public void getgetRandItemIndexTest()
        {
            Player p = new Player();

            p.items[1] = ITEM.KNIFE;
            p.items[7] = ITEM.KNIFE;
            for (int i = 0; i < 10; i++)
            {
                int n = p.getRandItemIndex();
                if (n == 1 || n == 7)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void getUseItemTest()
        {
            Player p = new Player();
            Assert.AreEqual(ITEM.NONE, p.getUseItem());
            p.items[1] = ITEM.KNIFE;
            p.net_item = 1;
            p.usedItem();
            Assert.AreEqual(ITEM.KNIFE, p.getUseItem());

        }

        [TestMethod()]
        public void usedItemTest1()
        {
            Player p = new Player();
            p.items[1] = ITEM.MURDERE_KNIFE;
            p.items[7] = ITEM.KNIFE;

            p.net_item = -1;
            p.usedItem();
            p.net_item = 1;
            p.usedItem();
            Assert.AreEqual(ITEM.MURDERE_KNIFE, p.getItem(1));
            Assert.AreEqual(ITEM.MURDERE_KNIFE, p.dayUseItem);
        }
    }
}