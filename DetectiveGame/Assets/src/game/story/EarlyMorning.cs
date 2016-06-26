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

        public override void init()
        {

        }

        private void doit1()
        {
            // 状況確認
            gm.midnightMessage = "";
            bool f = true;
            foreach (var p in gm.players.players)
            {
                if (p.fdeadToday)
                {
                    f = false;
                    gm.midnightMessage += p.name + "さんが無残な死体で発見された\n";
                    gm.midnightMessage += "　所持アイテム[";
                    foreach (var item in p.items)
                    {
                        if (item <= 0) continue;
                        gm.midnightMessage += Player.strItem(item) + ",";
                    }
                    gm.midnightMessage += "]\n";
                }
            }
            if (f)
            {
                gm.midnightMessage += "昨晩は何事もなかったようだ";
            }


            //監禁解除
            foreach (var p in gm.players.players)
            {
                p.killName = "";
                p.fdeadToday = false;
            }
            gm.captivityName = "";
            


            gm.message = "早朝フェイズです\n";
            gm.message += "状況とアイテムが確認できたら進めるを押してください\n";
            nextAction = null;
        }
        
    }
}
