using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class Morning : AStory
    {
        public Morning(Game gm):base(gm)
        {
            nextAction = () =>
            {
                doit1();
                return true;
            };
        }

        public override void init()
        {
        }
        private void doit1()
        {

            gm.message = "朝フェイズです\n";
            gm.message += "今朝の状況とアイテム交換について話してください\n";
            nextAction = null;
        }
        
    }
}
