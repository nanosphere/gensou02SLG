using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.story;

namespace game.db
{
    public class Items
    {
        public List<ITEM> items = new List<ITEM>();
        public int[] maxItem = new int[(int)ITEM.END];

        public void sync(Items o)
        {
            items.Clear();
            foreach(var s in o.items)
            {
                items.Add(s);
            }
            for(int i = 0; i < maxItem.Length; i++)
            {
                maxItem[i] = o.maxItem[i];
            }
        }
        public void setItemNum(int murder_knife, int knife, int chainlock, int i4, int i5)
        {
            maxItem[(int)ITEM.MURDERE_KNIFE] = murder_knife;
            maxItem[(int)ITEM.KNIFE] = knife;
            maxItem[(int)ITEM.CHEAN_LOCK] = chainlock;
            maxItem[(int)ITEM.KENTIKI] = i4;
            maxItem[(int)ITEM.KENSIKIT] = i5;
        }
        public void setDefaultItemNum(int playerNum)
        {
            if (playerNum == 2) setItemNum(1, 1, 4, 1, 1);//8
            else if (playerNum == 3) setItemNum(1, 2, 6, 2, 1);  //12
            else if (playerNum == 4) setItemNum(1, 3, 8, 2, 2);  //16
            else if (playerNum == 5) setItemNum(1, 4, 10, 3, 2);  //20
            else if (playerNum == 6) setItemNum(1, 4, 12, 4, 3);  //24
            else if (playerNum == 7) setItemNum(1, 5, 14, 4, 4);  //28
            else if (playerNum == 8) setItemNum(1, 6, 16, 5, 4);  //32
            else if (playerNum == 9) setItemNum(1, 7, 18, 5, 5);  //36
        }
        public int getItemNum(ITEM item_state)
        {
            int n = 0;
            foreach (var s in items)
            {
                if(s == item_state)
                {
                    n++;
                }
            }
            return n;

        }
        public void setItemPool()
        {
            items.Clear();
            for (int j = 0; j < maxItem.Length; j++)
            {
                for (int i = 0; i < maxItem[j]; i++)
                {
                    items.Add((ITEM)j);
                }
            }
            
            shuffle();

        }
        //-------------------------------
        // アイテムを取り出す
        //-------------------------------
        public ITEM popItem()
        {
            if (items.Count == 0) return ITEM.NONE;
            var item = items[0];
            items.RemoveAt(0);
            return item;
        }

        //-------------------------------
        // シャッフル
        //-------------------------------
        public void shuffle()
        {
            var cRandom = new System.Random(Environment.TickCount);

            for (int i = items.Count; i > 1; --i)
            {
                int a = i - 1;
                int b = cRandom.Next() % i;

                var tmp = items[a];
                items[a] = items[b];
                items[b] = tmp;
            }
        }
        

    }
}
