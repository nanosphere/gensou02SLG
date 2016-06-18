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
                return true;
            };
        }
        

        private void doit1()
        {
            //監禁解除
            foreach (var p in gm.players.players)
            {
                p.fcaptivity = false;
                p.killName = "";
            }
            


            gm.message = "早朝フェイズです\n";
            gm.message += "状況とアイテムが確認できたら進めるを押してください\n";
            nextAction = null;
        }
        
    }
}
