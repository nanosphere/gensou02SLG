using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public enum FIELD_STATE
    {
        NONE,
        EARLY_MORNING,
        MORNING,
        NOON,
        NIGHT,
        MIDNIGHT,
    }
    public class Field
    {
        // def
        public int timer = 60 * 5;
        public int useKnife = 2;
        public int dementDay = 2;   //発狂死の日数
        public int aiNum = 2;

        // global
        public FIELD_STATE state = FIELD_STATE.NONE;
        public int turn = 0;
        public int[] dustItems = new int[(int)db.ITEM.END];
        public db.ITEM dustItem = db.ITEM.NONE;


        // game
        public int captivity = 0;   //監禁者
        public int yes = 0;
        public int no = 0;
        public bool fmid_murdere = false;
        public int now_time = 0;
        
        public void sync(Field o)
        {
            timer = o.timer;
            useKnife = o.useKnife;
            dementDay = o.dementDay;
            state = o.state;
            turn = o.turn;
            captivity = o.captivity;
            yes = o.yes;
            no = o.no;
            fmid_murdere = o.fmid_murdere;
            now_time = o.now_time;
            for(int i=0;i< dustItems.Length; i++)
            {
                dustItems[i] = o.dustItems[i];
            }
            dustItem = o.dustItem;
        }
        public string toLine()
        {
            string s = "";
            s += "state=" + state.ToString();
            s += " turn=" + turn;
            s += " captivity=" + captivity;
            s += " yes=" + yes;
            s += " no=" + no;
            s += " fmid_murdere=" + fmid_murdere;
            s += " now_time=" + now_time;

            return s;
        }
        public void addDust(db.ITEM item)
        {
            dustItems[(int)item]++;
        }

    }
}

