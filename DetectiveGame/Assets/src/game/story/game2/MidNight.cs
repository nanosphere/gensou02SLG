using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using game.db;



namespace game.story.game2
{
    public class MidNight : AStory
    {
        public override void init()
        {
            Logger.info("story.MidNight():init.");


            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                p.message = "";
                if (p.fdead)
                {
                    p.state = db.PLAYER_STATE.MIDNIGHT_SELECT_OK;
                }
                else
                {
                    p.state = db.PLAYER_STATE.NONE;
                }
                p.dayDiscovere = 0;    //発見者か
                p.dayDead = false; //今日殺されたか
                p.dayKill = 0;    //殺した場合の相手
                p.dayMurdererSuccess = false;
                p.dayUseItem = ITEM.NONE;



                p.dayMidKnifeP = 0;
            }
            GameFactory.getGame().shareData.field.fmid_murdere = false;


            // ai
            if (GameFactory.getGame().localData.fgm)
            {
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    GameFactory.getAiManager().midnight_select(p);
                }
            }

        }
        
        public override bool end()
        {
            Logger.info("story.MidNight():end.");

            if (GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END))
            {
                return true;
            }
            return false;
        }

        MidNightSub sub;
        public override void run(game.net.NetworkData data)
        {
            Logger.info("story.MidNight():run.");

            sub = new MidNightSub();

            var src = GameFactory.getGame().shareData.players.getPlayer(data.src);

            if (data.cmd == game.net.NET_COMMAND.MIDNIGHT_SELECT)
            {
                src.state = PLAYER_STATE.MIDNIGHT_SELECT_OK;
                src.net_item = data.item;
                src.net_opp = data.dest;
                src.message = "選択しました";

                //全員選択が終わった
                if (!GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_SELECT_OK))
                {
                    return;
                }

                //判定
                checkAllSelectOk();

            }
            else if (data.cmd == game.net.NET_COMMAND.MIDNIGHT_SELECT_ITEM)
            {
                var dest = GameFactory.getGame().shareData.players.getPlayer(data.dest);
                if ((src.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT &&
                    dest.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT) ||
                    (src.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT &&
                    dest.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT)
                    )
                {
                    //アイテムを選択
                    for (int i = 0; i < src.items.Length; i++)
                    {
                        src.setItem(i, data.srcItem[i]);
                        dest.setItem(i, data.destItem[i]);
                    }

                    //状態を更新
                    if(src.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT)
                    {
                        src.state = PLAYER_STATE.MIDNIGHT_SELECT_OK;
                    }else if (src.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT)
                    {
                        src.state = PLAYER_STATE.MIDNIGHT_END;
                    }

                    if (dest.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT)
                    {
                        dest.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT;
                    }else if (dest.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT)
                    {
                        dest.state = PLAYER_STATE.MIDNIGHT_END;
                    }
                }

            }
            
            //ナイフ処理
            // 全員SELECT_OKか、第１発見者待機になるまでknife
            if (GameFactory.getGame().shareData.players.isAllPlayerState(
                PLAYER_STATE.MIDNIGHT_SELECT_OK,
                PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT))
            {
                checkKnife();
            }

            // knifeが全員終わった後の処理
            if (GameFactory.getGame().shareData.players.isAllPlayerState(
                PLAYER_STATE.MIDNIGHT_SELECT_OK,
                PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT))
            {
                GameFactory.getGame().shareData.players.setAllState(PLAYER_STATE.MIDNIGHT_END);

                // 発狂処理
                checkMurdere();

                //第１発見者を選ぶ
                checkDiscover();

            }

            //aiの処理
            aiItem();


            // item処理が終わるまで待つ
            if (GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END))
            {
                // 完了判定
                checkEnd();
            }

           

        }
        private void aiItem()
        {
            // ai
            if (GameFactory.getGame().localData.fgm)
            {
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if(p.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT)
                    {
                        GameFactory.getAiManager().midnight_item_select(p,
                            GameFactory.getGame().shareData.players.getPlayer(p.dayKill));
                        
                    }
                    if (p.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT)
                    {
                        GameFactory.getAiManager().midnight_item_select(p,
                            GameFactory.getGame().shareData.players.getPlayer(p.dayDiscovere));
                        
                        
                    }
                }
            }
        }

        private void checkDiscover()
        {
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                if (p.state != PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT) continue;
                if (p.discoverer == 0)
                {
                    //ランダムに第１発見者
                    Player p2 = sub.getRandomFirstDscoverer();
                    if (p2 != null)
                    {
                        p2.addMessage("あなたは" + p.name + "さんの第１発見者になりました");
                        p2.addMessage("交換するアイテムを選んでください");
                        p2.dayDiscovere = p.id;
                        p.discoverer = p2.id;
                        p2.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT;
                    }
                    else
                    {
                        // 候補がいなければskip
                        p.state = PLAYER_STATE.MIDNIGHT_END;
                    }
                }else
                {
                    var p2 = GameFactory.getGame().shareData.players.getPlayer(p.discoverer);
                    p2.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT;
                }
            }
        }

        private void checkAllSelectOk()
        {
            ShareData gm = GameFactory.getGame().shareData;

            // メッセージクリア
            foreach (var p in gm.players.players)
            {
                p.message = "";
            }
            //監禁者はアイテム未使用
            if (gm.field.captivity != 0)
            {
                gm.players.getPlayer(gm.field.captivity).net_item = -1;
            }

            // アイテムを使用
            foreach(var p in GameFactory.getGame().shareData.players.players)
            {
                p.usedItem();
            }

            // 包丁数
            int knifeNum = 0;

            //-----------------
            // 殺人包丁
            //-----------------
            Player murderer = gm.players.getUseMurdererKnife();
            if (murderer != null)
            {
                // 殺人判定
                sub.kill(murderer);
                knifeNum += 1;
            }

            //-----------------
            // 包丁をランダムに人数以下まで枝刈り
            //-----------------
            sub.unuseKnife(GameFactory.getGame().shareData.field.useKnife - knifeNum);

            //-----------------
            // まず、包丁loopしている処理をする
            //-----------------
            sub.killKnifeLoop();

        }

        private void checkKnife()
        {
            //主に包丁処理

            //-----------------
            // 包丁をランダムに使う
            //-----------------
            var knifeList = GameFactory.getGame().shareData.players.getUseItemPlayers(ITEM.KNIFE);
            if (knifeList.Count > 0)
            {
                // shuffle用の配列
                int[] rand = MyRandom.shuffleArray(knifeList.Count);

                //使用アイテムの保存
                Dictionary<int, ITEM> tmp = new Dictionary<int, ITEM>();
                foreach (var p in knifeList)
                {
                    tmp.Add(p.id, p.getUseItem());
                }

                // 包丁を使う
                for (int i = 0; i < knifeList.Count; i++)
                {
                    var p = knifeList[rand[i]];

                    // 先がナイフの場合はあとまわし
                    var opp = GameFactory.getGame().shareData.players.getPlayer(p.net_opp);
                    if (tmp.ContainsKey(opp.id))
                    {
                        if (tmp[opp.id] == ITEM.KNIFE)
                        {
                            continue;
                        }
                    }
                    // 実行
                    sub.kill(p);
                }
            }
        }
        private void checkMurdere()
        {
            //----------------
            //殺人者で生き残った人にマーダー判定(および設定)
            //----------------
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                sub.plusMurderer(p);
            }
            
        }
        private void checkEnd()
        {
            ShareData gm = GameFactory.getGame().shareData;

            

            //----------------
            //検視キット
            //----------------
            foreach (var p in gm.players.players)
            {
                var result = sub.usedKensikit(p);
                if (result == null) continue;
                if (result.reason == DEAD_REASON.LIVE)
                {
                    p.addMessage(result.target.name + "さんは生きています");
                }else
                {
                    p.addMessage(result.target.name + "さんの死因は"+ result.target.getReason()+"です");
                }
            }

            //----------------
            //探知機
            //----------------
            foreach (var p in gm.players.players)
            {
                var result = sub.usedTantiki(p);
                if (result == null) continue;
                if (result.count <= 0)
                {
                    p.addMessage(result.target.name + "さんはアイテムを持っていませんでした");
                }
                else
                {
                    p.addMessage(result.target.name + "さんは " + Player.getStr(result.item) + " を持ち、アイテムを" + result.count + "枚持っています");
                }
            }

        }

    }
}
