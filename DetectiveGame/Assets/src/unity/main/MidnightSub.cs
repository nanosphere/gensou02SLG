using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.story.game2;
using game.db;


namespace unity.main
{
    public class MidnightSub
    {
        public List<ITEM> mySelectedItem;
        public List<ITEM> enemySelectedItem;

        public void setMyItemList( MyDropdown drop, int item_index , Player opp, bool first, int id1,int id2 ,int id3,int id4)
        {
            var myp = GameFactory.getGame().getMyPlayer();
            var myitem = myp.getItem(item_index);
            drop.clear();

            //狂気の殺人包丁ならそれしか選択できない
            if (myitem == ITEM.MURDERE_KNIFE)
            {
                var id = myp.id * 10 + item_index;
                drop.add(Player.getStr(myitem) + "(" + id + ")", id);
                return;

            }

            
            // 敵と味方から選択可能
            // 自分で既に選択済みなのは除外
            List<int> kouho = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                kouho.Add(myp.id * 10 + i);
            }
            for (int i = 0; i < 4; i++)
            {
                kouho.Add(opp.id * 10 + i);
            }

            for (int i = 0; i < 4; i++)
            {
                //狂気の殺人包丁は除外
                if (myp.getItem(i) == ITEM.MURDERE_KNIFE)
                {
                    kouho.Remove(myp.id * 10 + i);
                    continue;
                }

                //他で選択されているのは除外
                if (first)
                {
                    // 最初は自分以外のindexを除外
                    if (item_index != i)
                    {
                        kouho.Remove(myp.id * 10 + i);
                        continue;
                    }
                }
            }
            if (!first)
            {
                if (item_index == 0)
                {
                    kouho.Remove(id2);
                    kouho.Remove(id3);
                    kouho.Remove(id4);
                }
                else if (item_index == 1)
                {
                    kouho.Remove(id1);
                    kouho.Remove(id3);
                    kouho.Remove(id4);
                }
                else if (item_index == 2)
                {
                    kouho.Remove(id1);
                    kouho.Remove(id2);
                    kouho.Remove(id4);
                }
                else if (item_index == 3)
                {
                    kouho.Remove(id1);
                    kouho.Remove(id2);
                    kouho.Remove(id3);
                }
            }
            foreach (var no in kouho)
            {
                int id = no / 10;
                int item = no % 10;
                var p = GameFactory.getGame().shareData.players.getPlayer(id);
                string s = p.getItemStr(item);
                if (s == "")
                {
                    s = "なし";
                }
                drop.add( s+ "(" + no + ")", no);
            }
            //選択
            if (first)
            {
                //最初は選択を自分のindex通り
                drop.select(myp.id * 10 + item_index);
            }else
            {
                if (item_index == 0)
                {
                    drop.select(id1);
                }
                else if (item_index == 1)
                {
                    drop.select(id2);
                }
                else if (item_index == 2)
                {
                    drop.select(id3);
                }
                else if (item_index == 3)
                {
                    drop.select(id4);
                }
            }
        }


        public void nokoriItem(Player opp, int id1, int id2, int id3, int id4)
        {
            mySelectedItem = new List<ITEM>();
            enemySelectedItem = new List<ITEM>();

            //-------------
            var myp = GameFactory.getGame().getMyPlayer();
            List<int> enemykouho = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                enemykouho.Add(myp.id * 10 + i);
            }
            for (int i = 0; i < 4; i++)
            {
                enemykouho.Add(opp.id * 10 + i);
            }


            // playerのアイテム
            mySelectedItem.Add(nokoriItemSub(id1));
            mySelectedItem.Add(nokoriItemSub(id2));
            mySelectedItem.Add(nokoriItemSub(id3));
            mySelectedItem.Add(nokoriItemSub(id4));

            // playerのアイテムを削除
            enemykouho.Remove(id1);
            enemykouho.Remove(id2);
            enemykouho.Remove(id3);
            enemykouho.Remove(id4);
            
            foreach (int no in enemykouho)
            {
                if (no == -1) continue;
                int id = no / 10;
                int item = no % 10;

                var p = GameFactory.getGame().shareData.players.getPlayer(id);
                if (p == null) continue;
                enemySelectedItem.Add(p.getItem(item));
            }
            
        }
        private ITEM nokoriItemSub(int no)
        {
            if (no == -1) return ITEM.NONE;

            int id = no / 10;
            int item = no % 10;
            var p = GameFactory.getGame().shareData.players.getPlayer(id);
            if (p == null) return ITEM.NONE;

            return p.getItem(item);
        }
    }
}

