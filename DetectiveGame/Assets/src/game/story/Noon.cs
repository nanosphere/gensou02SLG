using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class Noon : AStory
    {
        public Noon(Game gm,int state):base(gm)
        {
            if (state == 0) { nextAction = () => { return doit1(); }; }
            else if (state == 1) { nextAction = () => { return doit2(); }; }
            else if (state == 2) { nextAction = () => { return doit3(); }; }
            
            
        }
        public override void init()
        {
            if (GameFactory.getUnityManager().noon != null)
            {
                GameFactory.getUnityManager().noon.updateDraw();
            }
        }

        private bool doit1()
        {
            

            gm.message = "昼フェイズです\n";
            gm.message += "交換要求ターンです\n";
            gm.message += "アイテムと相手を選択してコードをGMに送ってください\n";

            GameFactory.getNetworkManager().askPlayers();

            nextAction = null;
            return true;
            
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            // アイテム交換
            foreach (var p in gm.players.players)
            {
                if (p.getNoonSelectItem() <= 0) continue;
                if (p.name == p.noon1.name) continue;

                var oppo = gm.players.getPlayer(p.noon1.name);
                oppo.addMessage(p.name + "さんからアイテム交換の要望がありました");
            }

            // 
            gm.message = "交換承認ターンです\n";
            gm.message += "※交換するしないに関わらずコードをGMに送ってください\n";
            gm.message += "※同じアイテムは選択しないでください\n";
            
            //
            GameFactory.getNetworkManager().askPlayers();
            nextAction = null;
            return true;
        }
        private bool doit3()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            // アイテム交換
            foreach (var p in gm.players.players)
            {
                if (p.getNoonSelectItem() <= 0) continue;
                if (p.name == p.noon1.name) continue;
                var oppo = gm.players.getPlayer(p.noon1.name);

                // 互いにおｋか
                int item = -1;
                foreach(var p2 in oppo.noon2.players)
                {
                    if(p.name == p2.name)
                    {
                        item = p2.item;
                        break;
                    }
                }
                //交換
                if (item != -1)
                {
                    p.addMessage(oppo.name + "さんへの交換は成立しました");
                    p.addMessage("　交換内容："+ Player.strItem(p.getNoonSelectItem()) + "(" + p.name + ")⇔"+Player.strItem(oppo.items[item]) + "(" + oppo.name + ")");

                    oppo.addMessage(p.name + "さんからの交換を実施しました");
                    oppo.addMessage("　交換内容：" + Player.strItem(p.getNoonSelectItem()) + "(" + p.name + ")⇔" + Player.strItem(oppo.items[item]) + "(" + oppo.name + ")");

                    var tmp = p.items[p.noon1.item];
                    p.items[p.noon1.item] = oppo.items[item];
                    oppo.items[item] = tmp;
                }
                else
                {
                    p.addMessage(oppo.name + "さんとの交換は不成立でした");
                }
            }

            

            gm.message = "交換が終わりました\n";
            gm.message += "進むを押してください\n";
            nextAction = null;
            return true;
        }

    }
}
