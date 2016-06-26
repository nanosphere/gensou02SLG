using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class Night : AStory
    {
        public Night(Game gm,int state):base(gm)
        {
            if (state == 0) { nextAction = () => { return doit1(); }; }
            else if (state == 1) { nextAction = () => { return doit2(); }; }
            else if (state == 2) { nextAction = () => { return doit3(); }; }
                        
        }
        public override void init()
        {
            if (GameFactory.getUnityManager().night != null)
            {
                GameFactory.getUnityManager().night.updateDraw();
            }
        }

        private bool doit1()
        {
            

            gm.message = "夜フェイズです\n";
            gm.message += "誰を監禁するか話し合った後に監禁するかしないかを選択してコードをGMに送ってください\n";

            GameFactory.getNetworkManager().askPlayers();
            nextAction = null;
            return true;
            
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            int yes = 0;
            int no = 0;
            foreach (var p in gm.players.players)
            {
                if(p.night1.fyes)
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
                nextAction = null;
                gm.fcapativity = true;

            }
            else
            {
                gm.message += "監禁しないに決まりました\n";
                nextAction = null;
                gm.fcapativity = false;
            }

            nextAction = null;
            return true;
        }
        private bool doit3()
        {

            if ( !gm.fcapativity )
            {
                gm.message += "skipします。次のターンを押してください\n";
                nextAction = null;
                return true;
            }
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            // 投票結果
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var p in gm.players.players)
            {
                result.Add(p.name , 0);
            }
            foreach (var p in gm.players.players)
            {
                if (p.night2.name == "") continue;
                result[p.night2.name]++;
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
                gm.message += p.name +"："+ result[p.name] + "票\n";
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
            gm.captivityName = pl[rand].name;

            gm.message += "夜フェーズは終了です\n";
            nextAction = null;
            return true;
        }

    }
}
