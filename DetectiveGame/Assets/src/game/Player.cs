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
        
        public bool murderer = false;
        public int murdererTurn = 0;
        public int kill = 0;
        public string killName = "";
        public bool fdead = false;
        public string deadName = "";
        public string deadReason = "";
        public string firstDiscoverer = "";
        public bool fcaptivity = false;

        public net.SelectCode select=null;
        public net.NoonCode noon = null;
        public net.NightCode night = null;
        public net.MidnightCode midnight = null;

        public string message = "";

        public void sync(Player o)
        {
            name = o.name;
            fnetWait = o.fnetWait;
            items.Clear();
            foreach(var s in o.items)
            {
                items.Add(s);
            }
            murderer = o.murderer;
            murdererTurn = o.murdererTurn;
            kill = o.kill;
            fdead = o.fdead;
            deadName = o.deadName;
            deadReason = o.deadReason;
            firstDiscoverer = o.firstDiscoverer;
            fcaptivity = o.fcaptivity;
            if (o.select != null && select == null) select = new net.SelectCode();
            if (select != null) select.sync(o.select);
            if (o.noon != null && noon == null) noon = new net.NoonCode();
            if (noon != null) noon.sync(o.noon);

            if (o.night != null && night == null) night = new net.NightCode();
            if (night != null) night.sync(o.night);
            if (o.midnight != null && midnight == null) midnight = new net.MidnightCode();
            if (midnight != null) midnight.sync(o.midnight);
            
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
            if (select != null)
            {
                s += " select=" + select.item + "," + select.name;
            }
            if (noon != null)
            {
                s += " noon=";
                foreach(var p in noon.players)
                {
                    s += "[" + p.name + "," + p.item + "]";
                }
            }

            return s;
        }

        //-----------------------------------------
        // init
        //-----------------------------------------
        public void init()
        {
            // 処理 はしります
        }
        //-----------------------------------------
        // item
        //-----------------------------------------
        public void addItem(int item)
        {
            items.Add(item);
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
        public List<string> getStrItemList()
        {
            List<string> itemsstr = new List<string>();
            itemsstr.Add("何もなし");

            int j = 0;
            foreach (var item in items)
            {
                itemsstr.Add("" + j + "." + Player.strItem(item) );
                j++;
            }
            return itemsstr;
        }
        public string getStrItem(int index)
        {
            if (0 <= index && index < items.Count)
            {
                return Player.strItem(items[index]);
            }
            return "";
        }
        public int getSelectItem()
        {
            if (0 <= getSelectItemIndex() && getSelectItemIndex() < items.Count)
            {
                return items[getSelectItemIndex()];
            }
            return 0;
        }

        public void addMessage(string mess)
        {
            message += mess+"\n";
        }
        public void killSuccess(string name)
        {
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
        public void dead(string name,string reason)
        {
            fdead = true;
            deadName = name;
            deadReason = reason;
            firstDiscoverer = "";
            select = null;
        }
        public void usedItem()
        {
            if (getSelectItemIndex() == -1) return;

            // 狂気の殺人包丁
            if (getSelectItemIndex() != 1)
            {
                items.RemoveAt(getSelectItemIndex());
            }
            select = null;
            
        }
        public int getSelectItemIndex()
        {
            if (select == null) return -1;
            return select.item;
        }
    }
}
