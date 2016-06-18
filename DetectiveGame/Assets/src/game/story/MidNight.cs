using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class MidNight : AStory
    {
        List<Player> deadlist = new List<Player>();
        public MidNight(Game gm):base(gm)
        {
            nextAction = () =>
            {
                return doit0();
            };

        }

        
        private bool doit0()
        {
            gm.message = "深夜フェイズです\n";
            gm.message = "使用するアイテムを選んでください";
            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                return doit1();
            };
            return true;
        }
        private bool doit1()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            //監禁者はアイテム未使用
            foreach (var p in gm.players.players)
            {
                if (p.fcaptivity)
                {
                    p.select.item = -1;
                }
            }


            // 包丁数
            int knifeNum = 0;

            //-----------------
            // 殺人包丁
            //-----------------
            Player murderer = getMurderer();
            if(murderer != null)
            {
                // 殺人判定
                kill(murderer,murderer.select.name,true);
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
                    kill(selectKnifePlayer[rand], p.name,false);

                    //選ばれなかった人
                    for (int i=0;i< selectKnifePlayer.Count;i++)
                    {
                        if (i == rand) continue;
                        kill(selectKnifePlayer[i], p.name, false);
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
            //殺人者のアイテム判定
            //----------------
            gm.message = "殺害に成功した人は拾うアイテムを選んでください(いない場合もあります)";
            GameFactory.getNetworkManager().askPlayers();
            nextAction = () =>
            {
                return doit2();
            };

            return true;
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            //----------------
            //殺人者のアイテム判定
            //----------------
            foreach (var p in deadlist)
            {
                Player p2 = gm.players.getPlayer(p.deadName);
                int index = 0;
                foreach (var i in p2.midnight.items)
                {
                    if (i.fyes)
                    {
                        p2.addItem(p.items[index]);
                        p.items[index] = 0;
                    }
                    index++;
                }
            }

            //----------------
            // 第1発見者
            //----------------
            foreach (var p in deadlist)
            {
                if( p.firstDiscoverer == "")
                {
                    //ランダムで選ぶ
                    Player first = getRandomFirstDscoverer(p);
                    p.firstDiscoverer = first.name;
                }
                else
                {

                }
            }
            

            //----------------
            //第一発見者処理(生き残った人間で分配)
            //----------------
            gm.message = "第1発見者は拾うアイテムを選んでください(いない場合もあります)";
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

            //----------------
            //第一発見者処理(生き残った人間で分配)
            //----------------
            foreach (var p in deadlist)
            {
                if (p.firstDiscoverer == "") continue;
                Player p2 = gm.players.getPlayer(p.firstDiscoverer);
                int index = 0;
                foreach (var i in p2.midnight.items)
                {
                    if (i.fyes)
                    {
                        p2.addItem(p.items[index]);
                        p.items[index] = 0;
                    }
                    index++;
                }
            }

            //----------------
            //検視キット
            //----------------
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getSelectItem() != 5) continue;

                var target = gm.players.getPlayer(p.select.name);
                if (target.fdead)
                {
                    p.addMessage(p.select.name + "さんの死因は"+target.deadReason+"です");
                }
                else
                {
                    p.addMessage(p.select.name+"さんは死んでいません");
                }
                
                p.usedItem();
            }

            //----------------
            //探知機
            //----------------
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getSelectItem() != 4) continue;

                var target = gm.players.getPlayer(p.select.name);
                int rand = MyRandom.rand(0, target.items.Count-1 );
                p.addMessage(p.select.name + "さんは " + target.getStrItem(rand) + " を持ち" + target.items.Count + "枚持っています");
                
                p.usedItem();
            }


            gm.message = "深夜フェイズが終わりました";
            nextAction = null;
            return true;
        }

        //================================================================
        // 以下、個別func
        //================================================================

        //--- 殺害
        private void kill(Player murderer , string name,bool fMurderer)
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
                murderer.killSuccess(opponent.name);
                murderer.usedItem();
                if (fMurderer)
                {
                    opponent.dead(murderer.name, "狂気の殺人包丁");
                }else
                {
                    opponent.dead(murderer.name, "包丁");
                }
                opponent.addMessage("あなたは殺された");
                deadlist.Add(opponent);
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
                if(p.select.name == myName)
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
                p.dead("","発狂死");
            }
        }
        // ランダムで第1発見者を返す
        private Player getRandomFirstDscoverer(Player dead)
        {
            List<Player> pl = new List<Player>();
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;

                //殺人者は除く
                if (dead.deadName == p.name) continue;

                pl.Add(p);
            }

            int rand = MyRandom.rand(0, pl.Count - 1);
            return pl[rand];
        }

    }
}
