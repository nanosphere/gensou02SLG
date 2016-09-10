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
    public class ItemsTests
    {
        [TestMethod()]
        public void syncTest()
        {
            Items pool = new Items();
            pool.items.Add(ITEM.CHEAN_LOCK);
            pool.maxItem[1] = 10;

            Items p2 = new Items();
            p2.sync(pool);

            Assert.AreEqual(ITEM.CHEAN_LOCK, p2.items[0]);
            Assert.AreEqual(10, p2.maxItem[1]);
        }

        [TestMethod()]
        public void setItemNumTest()
        {
            Items pool = new Items();
            pool.setItemNum(1, 1, 1, 1, 1);

            Assert.AreEqual(1, pool.maxItem[1]);
            Assert.AreEqual(1, pool.maxItem[2]);
            Assert.AreEqual(1, pool.maxItem[3]);
            Assert.AreEqual(1, pool.maxItem[4]);
            Assert.AreEqual(1, pool.maxItem[5]);
        }


        [TestMethod()]
        public void setItemPoolTest()
        {
            Items pool = new Items();
            pool.setDefaultItemNum(3);
            pool.setItemPool();
            Assert.AreEqual(12, pool.items.Count);

            int n = 0;
            foreach (var item in pool.items)
            {
                if (item == ITEM.MURDERE_KNIFE)
                {
                    n++;
                }
            }
            Assert.AreEqual(1, n);
        }


        [TestMethod()]
        public void popItemTest()
        {
            Items pool = new Items();
            pool.setDefaultItemNum(3);
            pool.setItemPool();
            int num = pool.items.Count;

            ITEM i1 = pool.items[0];
            ITEM i2 = pool.popItem();

            Assert.AreEqual(num-1, pool.items.Count);
            Assert.AreEqual(i1, i2);


        }

        [TestMethod()]
        public void getItemNumTest()
        {
            Items pool = new Items();
            pool.setDefaultItemNum(3);
            pool.setItemPool();

            Assert.AreEqual(1, pool.getItemNum(ITEM.MURDERE_KNIFE));
        }
    }
}