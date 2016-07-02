using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public class Player
    {
        // global
        public string name = "";
        public int[] items;
        
        // flag
        public bool murderer = false;
        public int murdererTurn = 0;
        public int kill = 0;
        public string killName = "";
        public bool fdead = false;
        public string deadName = "";
        public string deadReason = "";
        public string firstDiscoverer = "";
        public bool fdeadToday = false;

        // net
        public bool fnetWait = false;
        public net.NoonCode1 noon1 = new net.NoonCode1();
        public net.NoonCode2 noon2 = new net.NoonCode2();
        public net.NightCode1 night1 = new net.NightCode1();
        public net.NightCode2 night2 = new net.NightCode2();
        public net.MidnightCode1 midnight1 = new net.MidnightCode1();
        public net.MidnightCode2 midnight2 = new net.MidnightCode2();
        public net.MidnightCode3 midnight3 = new net.MidnightCode3();

        public string message = "";

        public void sync(Player o)
        {
            message = o.message;
            name = o.name;
            fnetWait = o.fnetWait;
            fdeadToday = o.fdeadToday;
            items = new int[o.items.Length];
            for(int i=0;i<o.items.Length;i++)
            {
                items[i] = o.items[i];
            }
            murderer = o.murderer;
            murdererTurn = o.murdererTurn;
            kill = o.kill;
            killName = o.killName;
            fdead = o.fdead;
            deadName = o.deadName;
            deadReason = o.deadReason;
            firstDiscoverer = o.firstDiscoverer;
            noon1.sync(o.noon1);
            noon2.sync(o.noon2);
            night1.sync(o.night1);
            night2.sync(o.night2);
            midnight1.sync(o.midnight1);
            midnight2.sync(o.midnight2);
            midnight3.sync(o.midnight3);


        }
        public string toLine()
        {
            string s = "";
            s += "name=" + name;
            s += " wait=" + fnetWait;
            s += " [";
            foreach (var item in items)
            {
                s += ""+item + ",";
            }
            s += "]";
            s += " murderer=" + murderer+"("+ murdererTurn+")";
            s += " kill=" + kill;
            s += " fdead=" + fdead;
            s += " firstDis=" + firstDiscoverer;

            return s;
        }

        //-----------------------------------------
        // init
        //-----------------------------------------
        public void init()
        {
            // 処理 はしります
            items = new int[GameFactory.getGame().info.max_item];
        }
        //-----------------------------------------
        // item
        //-----------------------------------------
        public void setItem(int index,int item)
        {
            items[index] = item;
        }
        public bool additem(int item)
        {
            for(int i=0;i<items.Length;i++)
            {
                if( items[i] <= 0)
                {
                    items[i] = item;
                    return true;
                }
            }
            Logger.info("Player.setItem():item set is fail. item="+item);
            return false;
        }
        public void removeMurdererKnife()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == 1)
                {
                    items[i] = -1;
                    return;
                }
            }
            Logger.info("Player.removeMurdererKnife():murderer knife is not found.");
            return ;
        }
        
        public string toItems()
        {
            string s = "";
            int i = 0;
            foreach (var o in items)
            {
                s += ""+i +"."+strItem(o) + "\n";
                i++;
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
        public static int intItem(string item)
        {
            if (item == "狂気の殺人包丁") return 1;
            if (item == "包丁") return 2;
            if (item == "チェーンロック") return 3;
            if (item == "探知機") return 4;
            if (item == "検死キット") return 5;

            return -1;
        }

        public string getStrItem(int index)
        {
            if (0 <= index && index < items.Length)
            {
                return strItem(items[index]);
            }
            return "";
        }
        public void addMessage(string mess)
        {
            message += mess + "\n";
        }
        public List<int> getItems()
        {
            List<int> items2 = new List<int>();
            foreach (var item in items)
            {
                if (item > 0)
                {
                    items2.Add(item);
                }
            }
            return items2;
        }
        //----------------------------------------------------------------
        public string getStrRandItem()
        {
            List<int> items2 = getItems();
            if (items2.Count == 0) return "";

            int rand = MyRandom.rand(0, items2.Count - 1);
            return strItem( items2[rand]);
        }
        
        //----------------------------------------------------------------
        public int getNoonSelectItem()
        {
            if (0 <= noon1.item && noon1.item < items.Length)
            {
                return items[noon1.item];
            }
            return 0;
        }

        public int getMidnightSelectItem()
        {
            if (0 <= midnight1.item && midnight1.item < items.Length)
            {
                return items[midnight1.item];
            }
            return 0;
        }
        
        public void MidnightKillSuccess(string name)
        {
            if (name == "") return;
            killName = name;
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
        public bool hasMurdererKnife()
        {
            foreach(var item in items)
            {
                if (item == 1) return true;
            }
            return false;
        }
        public void MidnightDead(string name,string reason)
        {
            fdead = true;
            deadName = name;
            deadReason = reason;
            firstDiscoverer = "";
            fdeadToday = true;
            midnight1.item = -1;
        }
        public void MidnightUsedItem()
        {
            if ( midnight1.item == -1) return;

            // 狂気の殺人包丁
            if (getMidnightSelectItem() != 1)
            {
                items[midnight1.item] = 0;
            }
            midnight1.item = -1;
            
        }

    }
}
