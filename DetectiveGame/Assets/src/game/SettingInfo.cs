using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public class SettingInfo
    {
        //-----------------------
        // 情報
        //-----------------------
        public string player_name = "player test";
        public bool fhost = true;
        
        public int timer_minutes = 60*5;
        public int useKnife = 2;
        public int dementDay = 2;
        public int max_item = 4;

        public int[] totalItem;

        public SettingInfo()
        {
            totalItem = new int[5];
            totalItem[0] = 1;
            totalItem[1] = 3;
            totalItem[2] = 12;
            totalItem[3] = 4;
            totalItem[4] = 4;
            
            // 24
            //if (item == 1) return "狂気の殺人包丁";
            //if (item == 2) return "包丁";
            //if (item == 3) return "チェーンロック";
            //if (item == 4) return "探知機";
            //if (item == 5) return "検死キット";

        }


        public void sync(SettingInfo o)
        {
            timer_minutes = o.timer_minutes;
            useKnife = o.useKnife;
            dementDay = o.dementDay;
            max_item = o.max_item;

            for (int i = 0; i < totalItem.Length; i++)
            {
                totalItem[i] = o.totalItem[i];
            }
        }
    }
}

