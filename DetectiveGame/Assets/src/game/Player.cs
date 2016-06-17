using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public class Player
    {
        public string name = "";
        public bool fnetWait = false;

        public List<int> items = new List<int>();

        public int openPoint = 0;
        public int point = 0;

        public bool murderer = false;
        public int murdererTurn = 0;
        public int kill = 0;
        public bool fdead = false;
        public string firstDiscoverer = "";

        public int selectItem = -1;
        public string selectName = "";

        public string message = "";

        public void init()
        {
            // 処理 はしります
        }
        public void sync(Player o)
        {
            name = o.name;
            openPoint = o.openPoint;
            point = o.point;
            items.Clear();
            foreach(var s in o.items)
            {
                items.Add(s);
            }
        }
        public void addItem(int item)
        {
            items.Add(item);

        }

        public string toItems()
        {
            string s = "";
            foreach (var o in items)
            {
                s += strItem(o) + "\n";
            }
            return s;
        }
        public static string strItem(int item)
        {
            if (item == 1) return "狂気の殺人包丁";
            if (item == 2) return "包丁";
            if (item == 3) return "チェーンロック";
            if (item == 4) return "探知機";
            if (item == 5) return "検死キット";

            return "";
        }

        public int getSelectItem()
        {
            if (0 <= selectItem && selectItem < items.Count)
            {
                return items[selectItem];
            }
            return 0;
        }

        public void addMessage(string mess)
        {
            message += mess+"\n";
        }
        public void killSuccess()
        {
            kill += 1;
            if (!murderer) {
                // まーだーではない
                murderer = true;
                murdererTurn = 0;
            }
            else
            {
                //マーダーなら
                murdererTurn = 0;
            }

        }
        public void dead()
        {
            fdead = true;
            firstDiscoverer = "";
            selectItem = -1;
        }
        public void usedItem()
        {
            items.RemoveAt(selectItem);
            selectItem = -1;
            
        }
        
    }
}
