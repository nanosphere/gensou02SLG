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

        public void sync(SettingInfo o)
        {
            timer_minutes = o.timer_minutes;
            useKnife = o.useKnife;
            dementDay = o.dementDay;
        }
    }
}

