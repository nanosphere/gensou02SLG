using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class EarlyMorning : AStory
    {
        public EarlyMorning(Game gm):base(gm)
        {
            nextAction = () =>
            {
                doit1();
            };
        }
        

        private void doit1()
        {



            nextAction = null;
        }
        
    }
}
