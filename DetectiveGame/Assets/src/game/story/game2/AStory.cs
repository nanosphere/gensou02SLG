using System;

namespace game.story.game2
{
    public abstract class AStory
    {
        public abstract void init();
        public abstract void run(game.net.NetworkData data);
        public abstract bool end();


    }
}
