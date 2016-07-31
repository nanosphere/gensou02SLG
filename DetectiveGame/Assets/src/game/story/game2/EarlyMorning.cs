using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.story.game2
{
    public class EarlyMorning : AStory
    {
        public override void init()
        {
            Logger.info("story.EarlyMorning():init.");

            GameFactory.getGame().shareData.field.turn += 1;

            //監禁解除
            GameFactory.getGame().shareData.field.captivity = 0;

            
        }

        public override void run(game.net.NetworkData data)
        {
            Logger.info("story.EarlyMorning():run.");

        }
        public override bool end()
        {
            //マーダー判定＠狂気
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                if (p.murderer) continue;
                if (p.hasItem(db.ITEM.MURDERE_KNIFE))
                {
                    p.murderer = true;
                    p.murdererTurn = 1;
                }
            }

            Logger.info("story.EarlyMorning():end.");
            return true;
        }


    }
}
