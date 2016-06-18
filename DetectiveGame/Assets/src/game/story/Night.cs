using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class Night : AStory
    {
        public Night(Game gm):base(gm)
        {
            nextAction = () =>
            {
                doit1();
                return true;
            };
            
        }
        

        private void doit1()
        {
            //選択しの作成
            GameFactory.getUnityManager().noon.create();

            gm.message = "夜フェイズです\n";
            gm.message += "監禁ターンです\n";
            gm.message += "監禁するかしないかを選択してコードをGMに送ってください\n";

            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                return doit2();
            };
            
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            int yes = 0;
            int no = 0;
            foreach (var p in gm.players.players)
            {
                if(p.night.fyes)
                {
                    yes++;
                }else
                {
                    no++;
                }
            }

            //
            gm.message = "投票する　：" + yes + "票\n";
            gm.message += "投票しない：" + no + "票\n";
            if ( yes > no)
            {
                gm.message += "監禁するに決まりました\n";
                gm.message += "監禁するまたは無投票を選択してコードをGMに送ってください\n";
                //
                GameFactory.getNetworkManager().askPlayers();
                nextAction = () =>
                {
                    return doit3();
                };
            }
            else
            {
                gm.message += "監禁しないに決まりました\n";
                gm.message += "深夜フェーズに移動します\n";
                nextAction = null;
            }

            return true;
        }
        private bool doit3()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            // 投票結果
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var p in gm.players.players)
            {
                result.Add(p.name , 0);
            }
            foreach (var p in gm.players.players)
            {
                result[p.night.name]++;
            }
            // 結果
            gm.message = "投票結果（同数の場合はランダムです）\n";
            int max = 0;
            foreach (var p in gm.players.players)
            {
                if( max < result[p.name])
                {
                    max = result[p.name];
                }
                gm.message = p.name +"："+ result[p.name] + "票\n";
            }
            List<Player> pl = new List<Player>();
            foreach (var p in gm.players.players)
            {
                if( max == result[p.name])
                {
                    pl.Add(p);
                }
            }

            // 同数の場合はランダム
            int rand = MyRandom.rand(0,pl.Count-1);
            gm.message += pl[rand].name + "さんは監禁されました\n";
            pl[rand].fcaptivity = true;

            nextAction = null;
            return true;
        }

    }
}
