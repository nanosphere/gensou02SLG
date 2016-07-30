using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.story.game2
{
    public class Morning : AStory
    {

        public override void init()
        {
            Logger.info("story.Morning():init.");


            GameFactory.getGame().localData.start_time = DateTime.Now;
            GameFactory.getGame().shareData.field.now_time = GameFactory.getGame().shareData.field.timer;

        }

        public override void run(game.net.NetworkData data)
        {
            Logger.info("story.Morning():run.");

        }
        public override bool end()
        {

            Logger.info("story.Morning():end.");
            return true;
        }


    }
}
