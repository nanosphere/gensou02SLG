using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class MidNight : AStory
    {
        public MidNight(Game gm):base(gm)
        {

        }
        
        public override void init()
        {
            

        }
        
        public override void doit()
        {
            List<Player> deadList = new List<Player>();
            

            //殺人(殺人包丁
            Player p1 = null;
            foreach (var p in gm.players)
            {
                if( p.getSelectItem() == 1)
                {
                    p1 = p;
                    break;
                }
            }
            // 殺害
            if(p1 != null)
            {
                var opponent = gm.getPlayer(p1.name);

                // チェーンロックを使っているかどうか
                if( opponent.getSelectItem() == 3)
                {
                    opponent.message = "何者かが近づいた気配があったが何も起こらなかった（襲撃がありました）";
                }else
                {
                    //死亡
                    opponent.fdead = true;
                    deadList.Add(opponent);
                }
                
            }

            //各自包丁
            //殺人者で生き残った人にマーダー判定(および設定)
            //殺人者で生き残った人にアイテム取得処理(殺人包丁→各自包丁)
            //検視キット
            //探知機
            //第一発見者処理(生き残った人間で分配)

        }

        public override void onUpdate()
        {
            /*
            //深夜処理
            bool f = true;
            foreach (var p in players)
            {
                if (p.action == -1)
                {
                    f = false;
                    break;
                }
            }
            if (f)
            {
                //全員の選択が終わった
                Logger.info("all selects.");
                
            }
            */
        }
    }
}
