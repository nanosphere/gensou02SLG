using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using game.story.game2;

namespace game.story.game1
{
    public class NextTurn
    {
        public NextTurn()
        {
            Logger.info("story.NextTurn():run.");

            AStory story = null;
            AStory story2 = null;
            if (GameFactory.getGame().shareData.field.state == FIELD_STATE.EARLY_MORNING)
            {
                story = new EarlyMorning();
                story2 = new Morning();
            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.MORNING)
            {
                story = new Morning();
                story2 = new Noon();
            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.NOON)
            {
                story = new Noon();
                story2 = new Night();
            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.NIGHT)
            {
                story = new Night();
                story2 = new MidNight();
            }
            else if (GameFactory.getGame().shareData.field.state == FIELD_STATE.MIDNIGHT)
            {
                story = new MidNight();
                story2 = new EarlyMorning();
            }


            // 次のターンを実施
            if (story.end())
            {
                if (GameFactory.getGame().shareData.field.state == FIELD_STATE.MIDNIGHT)
                {
                    GameFactory.getGame().shareData.field.state = FIELD_STATE.EARLY_MORNING;
                }
                else
                {
                    GameFactory.getGame().shareData.field.state += 1;
                }

                story2.init();
            }


            // send
            if (!GameFactory.debug)
            {
                // update view
                GameFactory.getUnityManager().updateDraw(true);
            }


        }
    }
}
