using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public abstract class AStory
    {
        protected Game gm;
        public AStory(Game gm)
        {
            this.gm = gm;
        }

        public abstract void init();
        public abstract void doit();
        public abstract void onUpdate();


    }
}
