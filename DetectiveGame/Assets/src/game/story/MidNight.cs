using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story
{
    public class MidNight : AStory
    {
        public MidNight(Game gm,int state):base(gm)
        {
            if (state == 0) { nextAction = () => { return doit0(); }; }
            else if (state == 1) { nextAction = () => { return doit1(); }; }
            else if (state == 2) { nextAction = () => { return doit2(); }; }
            else if (state == 3) { nextAction = () => { return doit3(); }; }

        }
        public override void init()
        {
            if (GameFactory.getUnityManager().midnight != null)
            {
                GameFactory.getUnityManager().midnight.updateDraw();
            }
        }

        private bool doit0()
        {

            


            gm.midnightMessage = "";
            gm.message = "深夜フェイズです\n";
            gm.message += "使用するアイテムを選んでください";
            GameFactory.getNetworkManager().askPlayers();
            nextAction = null;
            return true;
        }
        private bool doit1()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;
            
            //監禁者はアイテム未使用
            if (gm.captivityName != "")
            {
                gm.players.getPlayer(gm.captivityName).midnight1.item = -1;
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
                kill(murderer,murderer.midnight1.name,true);
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
                    if (selectKnifePlayer.Count == 0) continue;

                    //ランダム
                    int rand = MyRandom.rand(0, selectKnifePlayer.Count - 1);
                    if (p == murderer && murderer.killName != "")
                    {
                        // 狂気の殺人包丁が成功していた場合は失敗
                        selectKnifePlayer[rand].MidnightUsedItem();
                        selectKnifePlayer[rand].addMessage(p.name + "さんの襲撃に失敗しました");
                    }
                    else
                    {
                        kill(selectKnifePlayer[rand], p.name, false);
                    }

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
            gm.message += "GMへは全員がコードを送ってください";
            GameFactory.getNetworkManager().askPlayers();
            nextAction = null;

            return true;
        }
        private bool doit2()
        {
            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            var deadlist = getDeadPlayers();

            //----------------
            //殺人者のアイテム判定
            //----------------
            foreach (var p in deadlist)
            {
                if (p.deadName == "") continue;
                Player p2 = gm.players.getPlayer(p.deadName);

                for (int i = 0; i < p2.midnight2.myItems.Count; i++)
                {
                    p2.items[i] = p2.midnight2.myItems[i];
                }
                for (int i = 0; i < p2.midnight2.deadItems.Count; i++)
                {
                    p.items[i] = p2.midnight2.deadItems[i];
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
                    if (first != null)
                    {
                        p.firstDiscoverer = first.name;
                    }
                }
            }
            

            //----------------
            //第一発見者処理(生き残った人間で分配)
            //----------------
            gm.message = "第1発見者は拾うアイテムを選んでください(いない場合もあります)";
            gm.message += "GMへは全員がコードを送ってください";
            GameFactory.getNetworkManager().askPlayers();
            nextAction = null;
            return true;
        }
        private bool doit3()
        {

            if (!GameFactory.getNetworkManager().isPlayerAllAck()) return false;

            var deadlist = getDeadPlayers();

            //----------------
            //第一発見者処理(生き残った人間で分配)
            //----------------
            foreach (var p in deadlist)
            {
                if (p.firstDiscoverer == "") continue;
                Player p2 = gm.players.getPlayer(p.firstDiscoverer);

                for (int i = 0; i < p2.midnight2.myItems.Count; i++)
                {
                    p2.items[i] = p2.midnight2.myItems[i];
                }
                for (int i = 0; i < p2.midnight2.deadItems.Count; i++)
                {
                    p.items[i] = p2.midnight2.deadItems[i];
                }
            }

            //----------------
            //検視キット
            //----------------
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getMidnightSelectItem() != 5) continue;

                var target = gm.players.getPlayer(p.midnight1.name);
                if (target.fdead)
                {
                    p.addMessage(p.midnight1.name + "さんの死因は"+target.deadReason+"です");
                }
                else
                {
                    p.addMessage(p.midnight1.name+"さんは死んでいません");
                }
                
                p.MidnightUsedItem();
            }

            //----------------
            //探知機
            //----------------
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getMidnightSelectItem() != 4) continue;

                var target = gm.players.getPlayer(p.midnight1.name);
                string randitem = target.getStrRandItem();
                if (randitem == "")
                {
                    p.addMessage(p.midnight1.name + "さんはアイテムを持っていません");
                }
                else
                {
                    p.addMessage(p.midnight1.name + "さんは " + randitem + " を持ち" + target.getItems().Count + "枚持っています");
                }

                p.MidnightUsedItem();
            }

            //---------------

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
                murderer.addMessage(name + "さんはすでに無残な死体で横たわっていた");

                // 誰も見つけていない
                if ( opponent.firstDiscoverer == "")
                {
                    murderer.addMessage("あなたはだ1発見者です");
                    opponent.firstDiscoverer = murderer.name;
                }
                
                return;
            }

            // チェーンロックを使っているかどうか
            if (opponent.getMidnightSelectItem() == 3)
            {
                opponent.addMessage("何者かが扉を開けようとしたようだ（襲撃がありました）");
                murderer.addMessage(name+ "さんの殺害に失敗した");
                murderer.MidnightUsedItem();
            }
            else if (murderer.getMidnightSelectItem() == 2 && 
                opponent.getMidnightSelectItem() == 2 &&
                opponent.midnight1.name == murderer.name)
            {
                //相手も自分を狙っているとき
                murderer.addMessage(name + "さんを殺害した");
                murderer.addMessage("あなたは殺された");
                murderer.MidnightKillSuccess(opponent.name);
                murderer.MidnightUsedItem();
                murderer.MidnightDead(opponent.name, "包丁");
                opponent.addMessage(murderer.name + "さんを殺害した");
                opponent.addMessage("あなたは殺された");
                opponent.MidnightKillSuccess(murderer.name);
                opponent.MidnightUsedItem();
                opponent.MidnightDead(murderer.name, "包丁");
            }
            else
            {
                //死亡
                murderer.addMessage(name+ "さんを殺害した");
                murderer.MidnightKillSuccess(opponent.name);
                murderer.MidnightUsedItem();
                opponent.MidnightUsedItem();
                if (fMurderer)
                {
                    opponent.MidnightDead(murderer.name, "狂気の殺人包丁");
                }else
                {
                    opponent.MidnightDead(murderer.name, "包丁");
                    if (opponent.hasMurdererKnife())
                    {
                        murderer.additem(1);
                        murderer.addMessage(name + "さんは狂気の殺人包丁を持っていた");
                        murderer.addMessage("狂気の殺人包丁を手に入れた");
                    }
                }
                opponent.addMessage("あなたは殺された");
            }
        }

        // 狂気の殺人包丁所持者を返す
        private Player getMurderer()
        {
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.getMidnightSelectItem() == 1)
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
                if (p.getMidnightSelectItem() == item)
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
                knifeList[rand].MidnightUsedItem();
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
                if(p.midnight1.name == myName)
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
                p.MidnightDead("","発狂死");
                if (p.hasMurdererKnife())
                {
                    List<Player> pl = new List<Player>();
                    foreach(var p2 in gm.players.players)
                    {
                        if (p2.fdead) continue;
                        pl.Add(p2);
                    }
                    if (pl.Count != 0)
                    {
                        var p3 = pl[MyRandom.rand(0, pl.Count - 1)];
                        if (!p3.additem(1))
                        {
                            p3.setItem(MyRandom.rand(0, p3.items.Length-1), 1);
                        }
                    }
                }
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

            if (pl.Count == 0) return null;
            int rand = MyRandom.rand(0, pl.Count - 1);
            return pl[rand];
        }

        // 死んだプレイヤーを返す
        private List<Player> getDeadPlayers()
        {
            List<Player> pl = new List<Player>();
            foreach (var p in gm.players.players)
            {
                if (p.fdeadToday) {
                    pl.Add(p);
                }
            }
            return pl;
        }

    }
}
