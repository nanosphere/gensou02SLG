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
            nextAction = () =>
            {
                doit1();
            };

        }
        
        private void doit1()
        {
        
            // 包丁数
            int knifeNum = 0;

            //-----------------
            // 殺人包丁
            //-----------------
            Player murderer = getMurderer();
            if(murderer != null)
            {
                // 殺人判定
                kill(murderer,murderer.selectName);
                knifeNum += 1;
            }

            //-----------------
            // 包丁
            //-----------------
            var knifeList = getKnifeList(knifeNum);
            if (knifeList.Count > 0) {
                // 包丁の使われる人を検索
                foreach (var p in gm.players.players)
                {
                    var selectKnifePlayer = getSelectKnifePlayer(knifeList, p.name);
                    //ランダム
                    int rand = MyRandom.rand(0, selectKnifePlayer.Count - 1);
                    kill(selectKnifePlayer[rand], p.name);

                    //選ばれなかった人
                    for (int i=0;i< selectKnifePlayer.Count;i++)
                    {
                        if (i == rand) continue;
                        kill(selectKnifePlayer[i], p.name);
                    }
                }
            }

            //----------------
            //殺人者で生き残った人にマーダー判定(および設定)
            //----------------
            foreach (var p in gm.players.players)
            {
                plusMurderer(p);
            }

            //----------------
            //殺人者で生き残った人にアイテム取得処理(殺人包丁→各自包丁)
            //----------------
            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                doit2();
            };


        }
        private void doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return;

            //----------------
            //殺人者で生き残った人にアイテム取得処理(殺人包丁→各自包丁)
            //----------------


            //----------------
            //検視キット
            //----------------

            //----------------
            //探知機
            //----------------

            //----------------
            //第一発見者処理(生き残った人間で分配)
            //----------------


            nextAction = null;
        }


        //--- 殺害
        private void kill(Player murderer , string name)
        {
            var opponent = gm.players.getPlayer(name);

            //すでに死んでいる
            if(opponent.fdead)
            {
                murderer.addMessage(name + "はすでに無残な死体で横たわっていた");

                // 誰も見つけていない
                if ( opponent.firstDiscoverer == "")
                {
                    murderer.addMessage("あなたはだ1発見者です");
                    opponent.firstDiscoverer = murderer.name;
                }
                
                return;
            }

            // チェーンロックを使っているかどうか
            if (opponent.getSelectItem() == 3)
            {
                opponent.addMessage("何者かが扉を開けようとしたようだ（襲撃がありました）");
                murderer.addMessage(name+"の殺害に失敗した");
                murderer.usedItem();
            }
            else
            {
                //死亡
                murderer.addMessage(name+"を殺害した");
                murderer.killSuccess();
                murderer.usedItem();
                opponent.dead();
                opponent.addMessage("あなたは殺された");
            }
        }

        // 狂気の殺人包丁所持者を返す
        private Player getMurderer()
        {
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getSelectItem() == 1)
                {
                    return p;
                }
            }
            return null;
        }
        // 指定アイテムのプレイヤーを返す
        private List<Player> getPlayers(int item)
        {
            List<Player> pl = new List<Player>();
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getSelectItem() == item)
                {
                    pl.Add(p);
                }
            }
            return pl;
        }

        // 包丁リストを返す
        private List<Player> getKnifeList(int knifeNum)
        {
            var knifeList = getPlayers(2);

            //使用の数が上限以下になるように減らす
            for (int i = 0; i < gm.info.useKnife; i++)//無限ループ防止
            {
                if (knifeNum + knifeList.Count <= gm.info.useKnife)
                {
                    break;
                }
                int rand = MyRandom.rand(0, knifeList.Count - 1);
                knifeList[rand].addMessage("今夜は人の気配が多そうなのでやめよう");
                knifeList[rand].addMessage("（狂気の殺人包丁または包丁を使った人が" + gm.info.useKnife + "人以上いました）");
                knifeList[rand].usedItem();
                knifeList.RemoveAt(rand);
            }
            return knifeList;
        }
        //包丁で自分を狙っている人を返す
        private List<Player> getSelectKnifePlayer(List<Player> knifeList, string myName)
        {
            List<Player> pl = new List<Player>();
            foreach(var p in knifeList)
            {
                if(p.selectName == myName)
                {
                    pl.Add(p);
                }
            }
            return pl;
        }
        // 発狂判定
        private void plusMurderer(Player p)
        {
            if (p.fdead) return;
            if (!p.murderer) return;

            p.murdererTurn += 1;
            if (p.murdererTurn > gm.info.dementDay)
            {
                p.addMessage("発狂死しました");
                p.dead();
            }
        }

    }
}
