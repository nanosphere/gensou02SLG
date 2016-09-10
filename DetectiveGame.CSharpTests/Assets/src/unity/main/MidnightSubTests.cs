using Microsoft.VisualStudio.TestTools.UnitTesting;
using unity.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.db;



namespace unity.main.Tests
{
    [TestClass()]
    public class MidnightSubTests
    {
        [TestMethod()]
        public void nokoriItemTest()
        {
            game.Game gm = game.GameTest.createGame();
            MidnightSub sub = new MidnightSub();

            game.GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            game.GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);

            sub.nokoriItem(GameFactory.getGame().shareData.players.getPlayer(2),10, 13, 21, 22);
            Assert.AreEqual(sub.mySelectedItem.Count, 4);
            Assert.AreEqual(sub.enemySelectedItem.Count, 4);
            Assert.AreEqual(sub.mySelectedItem[0], ITEM.MURDERE_KNIFE);
            Assert.AreEqual(sub.mySelectedItem[1], ITEM.KNIFE);
            Assert.AreEqual(sub.mySelectedItem[2], ITEM.CHEAN_LOCK);
            Assert.AreEqual(sub.mySelectedItem[3], ITEM.CHEAN_LOCK);
            Assert.AreEqual(sub.enemySelectedItem[0], ITEM.KNIFE);
            Assert.AreEqual(sub.enemySelectedItem[1], ITEM.KNIFE);
            Assert.AreEqual(sub.enemySelectedItem[2], ITEM.CHEAN_LOCK);
            Assert.AreEqual(sub.enemySelectedItem[3], ITEM.CHEAN_LOCK);

        }
        [TestMethod()]
        public void nokoriItemTest2()
        {
            game.Game gm = game.GameTest.createGame();
            MidnightSub sub = new MidnightSub();

            game.GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.NONE, ITEM.KNIFE, ITEM.KNIFE);
            game.GameTest.setItem(gm, "p2", ITEM.KENSIKIT, ITEM.CHEAN_LOCK, ITEM.NONE, ITEM.KENTIKI);

            sub.nokoriItem(GameFactory.getGame().shareData.players.getPlayer(2), 10, 13, 21, 22);
            Assert.AreEqual(sub.mySelectedItem.Count, 4);
            Assert.AreEqual(sub.enemySelectedItem.Count, 4);
            Assert.AreEqual(sub.mySelectedItem[0], ITEM.MURDERE_KNIFE);
            Assert.AreEqual(sub.mySelectedItem[1], ITEM.KNIFE);
            Assert.AreEqual(sub.mySelectedItem[2], ITEM.CHEAN_LOCK);
            Assert.AreEqual(sub.mySelectedItem[3], ITEM.NONE);
            Assert.AreEqual(sub.enemySelectedItem[0], ITEM.NONE);
            Assert.AreEqual(sub.enemySelectedItem[1], ITEM.KNIFE);
            Assert.AreEqual(sub.enemySelectedItem[2], ITEM.KENSIKIT);
            Assert.AreEqual(sub.enemySelectedItem[3], ITEM.KENTIKI);

        }


        [TestMethod()]
        public void setMyItemListTest()
        {
            game.Game gm = game.GameTest.createGame();
            MidnightSub sub = new MidnightSub();

            game.GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            game.GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);

            MyDropdown drop = new MyDropdownDebug();
            sub.setMyItemList(drop,0,GameFactory.getGame().shareData.players.getPlayer(2),true,12,13,21,22);
            Assert.AreEqual(drop.items.Count, 1);
            Assert.AreEqual(drop.option[0], 10);
            Assert.AreEqual(drop.getSelect(), 10);

            sub.setMyItemList(drop, 1, GameFactory.getGame().shareData.players.getPlayer(2), true, 12, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 11);
            Assert.AreEqual(drop.option[1], 20);
            Assert.AreEqual(drop.option[2], 21);
            Assert.AreEqual(drop.option[3], 22);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 11);

            sub.setMyItemList(drop, 2, GameFactory.getGame().shareData.players.getPlayer(2), true, 12, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 12);
            Assert.AreEqual(drop.option[1], 20);
            Assert.AreEqual(drop.option[2], 21);
            Assert.AreEqual(drop.option[3], 22);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 12);

            sub.setMyItemList(drop, 3, GameFactory.getGame().shareData.players.getPlayer(2), true, 12, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 13);
            Assert.AreEqual(drop.option[1], 20);
            Assert.AreEqual(drop.option[2], 21);
            Assert.AreEqual(drop.option[3], 22);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 13);

        }
        [TestMethod()]
        public void setMyItemListTest2()
        {
            game.Game gm = game.GameTest.createGame();
            MidnightSub sub = new MidnightSub();

            game.GameTest.setItem(gm, "p1", ITEM.MURDERE_KNIFE, ITEM.KNIFE, ITEM.KNIFE, ITEM.KNIFE);
            game.GameTest.setItem(gm, "p2", ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK, ITEM.CHEAN_LOCK);

            MyDropdown drop = new MyDropdownDebug();
            sub.setMyItemList(drop, 0, GameFactory.getGame().shareData.players.getPlayer(2), false, 10, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 1);
            Assert.AreEqual(drop.option[0], 10);
            Assert.AreEqual(drop.getSelect(), 10);

            sub.setMyItemList(drop, 1, GameFactory.getGame().shareData.players.getPlayer(2), false, 10, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 11);
            Assert.AreEqual(drop.option[1], 12);
            Assert.AreEqual(drop.option[2], 13);
            Assert.AreEqual(drop.option[3], 20);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 13);

            sub.setMyItemList(drop, 2, GameFactory.getGame().shareData.players.getPlayer(2), false, 10, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 11);
            Assert.AreEqual(drop.option[1], 12);
            Assert.AreEqual(drop.option[2], 20);
            Assert.AreEqual(drop.option[3], 21);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 21);

            sub.setMyItemList(drop, 3, GameFactory.getGame().shareData.players.getPlayer(2), false, 10, 13, 21, 22);
            Assert.AreEqual(drop.items.Count, 5);
            Assert.AreEqual(drop.option[0], 11);
            Assert.AreEqual(drop.option[1], 12);
            Assert.AreEqual(drop.option[2], 20);
            Assert.AreEqual(drop.option[3], 22);
            Assert.AreEqual(drop.option[4], 23);
            Assert.AreEqual(drop.getSelect(), 22);

        }
    }
}