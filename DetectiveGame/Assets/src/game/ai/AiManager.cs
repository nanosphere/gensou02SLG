using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.db;

namespace game.ai
{
    public enum AI_MODE{
        NONE,
        AI1
    }
    public class AiManager
    {
        private Computer getAi(AI_MODE mode)
        {
            if(mode == AI_MODE.NONE)
            {
                return null;
            }else if( mode == AI_MODE.AI1)
            {
                return new Computer();
            }
            return null;
        }
        public void noon_ack(Player myp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.noon_ack(myp);
        }
        public void noon_item(Player myp,Player opp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.noon_item(myp, opp);
        }
        public void noon_end(Player myp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.noon_end(myp);
        }

        public void night_ack(Player myp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.night_ack(myp);
        }
        public void night_vote(Player myp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.night_vote(myp);
        }

        public void midnight_select(Player myp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.midnight_select(myp);
        }
        public void midnight_item_select(Player myp,Player opp)
        {
            var p = getAi(myp.ai);
            if (p == null) return;
            p.midnight_item_select(myp, opp);
        }
    }
}
