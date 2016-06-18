using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class Noon : AStory
    {
        public Noon(Game gm):base(gm)
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

            gm.message = "昼フェイズです\n";
            gm.message += "交換要求ターンです\n";
            gm.message += "アイテムと相手を選択してコードをGMに送ってください\n";

            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                return doit2();
            };
            
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            //選択しの作成
            GameFactory.getUnityManager().noon.create();

            // アイテム交換
            foreach (var p in gm.players.players)
            {
                if (p.getSelectItemIndex() == -1) continue;
                if (p.name == p.select.name) continue;

                var oppo = gm.players.getPlayer(p.select.name);
                oppo.addMessage(p.name + "からアイテム交換の要望がありました");
            }

            // 
            gm.message = "交換承認ターンです\n";
            gm.message += "交換する場合はアイテムを、しない場合は何もなしを選択してコードをGMに送ってください\n";
            gm.message += "※交換を受けない場合はアイテムなしを選択してください\n";
            gm.message += "※同じアイテムは選択しないでください\n";
            gm.message += "※要望があった人いがいはアイテムなしにしてください\n";
            
            //
            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                return doit3();
            };
            return true;
        }
        private bool doit3()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            
            // アイテム交換
            foreach (var p in gm.players.players)
            {
                Logger.info(p.toLine());
                if (p.getSelectItemIndex() == -1) continue;
                if (p.name == p.select.name) continue;
                var oppo = gm.players.getPlayer(p.select.name);

                // 互いにおｋか
                int item = -1;
                foreach(var p2 in oppo.noon.players)
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
                    p.addMessage(
                        Player.strItem(p.items[p.getSelectItemIndex()]) + "(" + p.name + ")と" +
                        Player.strItem(oppo.items[item]) + "(" + oppo.name + ")" +
                        "のアイテムを交換しました");
                    oppo.addMessage(
                        Player.strItem(p.items[p.getSelectItemIndex()]) + "(" + p.name + ")と" +
                        Player.strItem(oppo.items[item]) + "(" + oppo.name + ")" +
                        "のアイテムを交換しました");
                    var tmp = p.items[p.getSelectItemIndex()];
                    p.items[p.getSelectItemIndex()] = oppo.items[item];
                    oppo.items[item] = tmp;
                }
                else
                {
                    p.addMessage(oppo.name + "が交換をしませんでした");
                }
            }


            gm.message = "交換が終わりました\n";
            gm.message += "進むを押してください\n";
            nextAction = null;
            return true;
        }

    }
}
