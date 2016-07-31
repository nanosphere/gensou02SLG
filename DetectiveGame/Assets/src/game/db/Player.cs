using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.db
{
    public enum ITEM
    {
        NONE,
        MURDERE_KNIFE,
        KNIFE,
        CHEAN_LOCK,
        KENTIKI,
        KENSIKIT,
        END
    }


    public enum PLAYER_STATE
    {
        NONE,
        
        NOON_WAIT_ACK,
        NOON_REQUEST_RETURN,
        NOON_ITEM,
        NOON_ITEM_OK,
        NOON_END,
        
        NIGHT_SELECT_OK,
        NIGHT_SELECT_END,
        NIGHT_VOTE,
        NIGHT_VOTE_OK,
        NIGHT_VOTE_END,

        MIDNIGHT_SELECT_OK,
        MIDNIGHT_KILL_ITEM_SELECT,
        MIDNIGHT_KILL_ITEM_SELECT_WAIT,
        
        MIDNIGHT_END,
        //MIDNIGHT_DISCOVERER_ITEM_SELECT,
        //MIDNIGHT_DISCOVERER_ITEM_SELECT_WAIT,
        MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT,
        MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT,

        END
    }
    public enum DEAD_REASON
    {
        NONE,
        MURDERER_KNIFE,
        KNIFE,
        HAKKYO,
        LIVE,
    }
    public class Player
    {
        // global
        public int id = 0;
        public string name = "";
        public PLAYER_STATE state = PLAYER_STATE.NONE;
        public string message = "";
        public ITEM[] items = new ITEM[8];
        public ai.AI_MODE ai = game.ai.AI_MODE.NONE;

        // flag
        public bool fdead = false;
        public DEAD_REASON deadReason = DEAD_REASON.NONE;
        public int deadid = 0;  //殺された場合の相手
        public int discoverer = 0;  //第１発見者のid
        
        public bool murderer = false;
        public int murdererTurn = 0;
        
        //その日限定
        public int dayDiscovere = 0;    //発見者か
        public bool dayDead = false; //今日殺されたか
        public int dayKill = 0;    //殺した場合の相手
        public bool dayMurdererSuccess = false;//狂気の殺人包丁で成功した場合
        public ITEM dayUseItem = ITEM.NONE;
        
        public int dayNoonCount = 0;    //交換した回数
        public int dayNightVote = 0;  //投票された数
        public int dayMidKnifeP = 0;  //優先順位

        // net
        public int net_opp = 0;
        public int net_item = -1;
        public bool net_yes= false;
        

        public void sync(Player o)
        {
            id = o.id;
            name = o.name;
            state = o.state;
            message = o.message;
            ai = o.ai;
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = o.items[i];
            }
            //
            fdead = o.fdead;
            deadReason = o.deadReason;
            deadid = o.deadid;
            discoverer = o.discoverer;
            murderer = o.murderer;
            murdererTurn = o.murdererTurn;
            dayDiscovere = o.dayDiscovere;
            dayDead = o.dayDead;
            dayKill = o.dayKill;
            dayMurdererSuccess = o.dayMurdererSuccess;
            dayUseItem = o.dayUseItem;

            dayNoonCount = o.dayNoonCount;
            dayNightVote = o.dayNightVote;
            dayMidKnifeP = o.dayMidKnifeP;
            //
            net_opp = o.net_opp;
            net_item = o.net_item;

        }
        public string toLine()
        {
            string s = "";
            s += ""+name+"("+id+") s="+state.ToString();
            s += " fdead=" + fdead;
            s += " deadReason=" + deadReason.ToString();
            s += " deadid=" + deadid;
            s += " discoverer=" + discoverer;

            return s;
        }
        public void addItem(ITEM item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == ITEM.NONE)
                {
                    items[i] = item;
                    return;
                }
            }
            Logger.info("Player.setItem():item set is fail. item=" + item);
            return;
        }
        public int getItemNum()
        {
            int n = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != ITEM.NONE)
                {
                    n++;
                }
            }
            return n;
        }
        public ITEM getItem(int index)
        {
            if(0<=index && index < items.Length)
            {
                return items[index];
            }
            return ITEM.NONE;
        }
        public string getItemStr(int index)
        {
            if (0 <= index && index < items.Length)
            {
                return getStr(items[index]);
            }
            return "";
        }
        public bool hasItem(ITEM item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item) return true;
            }
            return false;
        }
        public static string getStr(ITEM item)
        {
            switch (item)
            {
                case ITEM.MURDERE_KNIFE: return "狂気の殺人包丁";
                case ITEM.KNIFE: return "包丁";
                case ITEM.CHEAN_LOCK: return "チェーンロック";
                case ITEM.KENTIKI: return "探知機";
                case ITEM.KENSIKIT: return "検死キット";
            }
            return "";
        }

        public void addMessage(string str)
        {
            message += str + "\n";
        }

        public void usedItem()
        {
            if (net_item == -1) return;

            GameFactory.getGame().shareData.field.addDust(getItem(net_item));
            dayUseItem = getItem(net_item);
            if (dayUseItem != ITEM.MURDERE_KNIFE)
            {
                setItem(net_item, ITEM.NONE);
            }
            net_item = -1;
        }

        // dead
        public void dead(DEAD_REASON reason, int p2)
        {
            dayDead = true;
            fdead = true;
            deadReason = reason;
            deadid = p2;
            discoverer = 0;
        }
        // kill success
        public void killSuccess(int p2)
        {
            dayKill = p2;
            murdererTurn = 0;
            murderer = true;
        }

        // get index
        public int getItemIndex(ITEM item)
        {
            for (int i=0;i<items.Length;i++)
            {
                if (items[i] == item) return i;
            }
            return -1;
        }

        public void setItem(int index, ITEM item)
        {
            if (0 <= index && index < items.Length)
            {
                items[index] = item;
            }
        }
        
        public string getReason()
        {
            switch (deadReason)
            {
                case DEAD_REASON.KNIFE: return "包丁";
                case DEAD_REASON.MURDERER_KNIFE: return "狂気の殺人包丁";
                case DEAD_REASON.HAKKYO: return "発狂死";
            }
            return "";
        }
        public int getRandItemIndex()
        {
            int[] rand = MyRandom.shuffleArray(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[rand[i]] != ITEM.NONE)
                {
                    return rand[i];
                }
            }
            return -1;
        }
        // 使っているアイテムを返す
        public ITEM getUseItem()
        {
            return dayUseItem;
        }
        public string getUseItemStr()
        {
            return getStr(getUseItem());
        }

        public bool isSelectState(FIELD_STATE field )
        {
            if( field == FIELD_STATE.NOON)
            {
                if (state != PLAYER_STATE.NOON_END) return true;
            }
            else if (field == FIELD_STATE.NIGHT)
            {
                if (state == PLAYER_STATE.NIGHT_VOTE) return true;
                if (state == PLAYER_STATE.NONE) return true;
            }
            else if (field == FIELD_STATE.MIDNIGHT)
            {
                if (state == PLAYER_STATE.NONE) return true;
            }
            return false;
        }
    }
}
