using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public abstract class AStory
    {
        protected delegate void nextActionFunc();
        protected nextActionFunc nextAction=null;
        public bool frun = false;

        protected Game gm;
        public AStory(Game gm){
            this.gm = gm;
        }

        public void update()
        {
            if (!frun) return;

            // 処理
            if(nextAction != null)
            {
                nextAction();
            }
        }
        

    }
}
