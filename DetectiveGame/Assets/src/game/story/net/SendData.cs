using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.net;

namespace game.story.net
{
    public class SendData
    {
        public SendData(int srcPlayer, NetworkData data)
        {
            Logger.info("story.SendData():run.");
            if (GameFactory.getGame().shareData.field.state == FIELD_STATE.NOON)
            {
                { var o = new game2.Noon(); o.run(data); }

            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.NIGHT)
            {
                { var o = new game2.Night(); o.run(data); }

            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.MIDNIGHT)
            {
                { var o = new game2.MidNight(); o.run(data); }
            }
            GameFactory.getUnityManager().updateDraw(false);

        }
        
    }
}
