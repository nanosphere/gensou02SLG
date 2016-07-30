using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using game.db;

namespace game.story.game2
{
    public class MidNightSub
    {
        List<int> tmpKill;

        //--- 殺害
        public void kill(Player murderer)
        {
            // ナイフのみ
            if (!(murderer.getUseItem() == ITEM.KNIFE ||
                murderer.getUseItem() == ITEM.MURDERE_KNIFE))
            {
                return;
            }
            

            // 相手
            var opponent = GameFactory.getGame().shareData.players.getPlayer(murderer.net_opp);
            
            //すでに死んでいる
            if (opponent.fdead)
            {
                murderer.addMessage(opponent.name + "さんは既に無残な死体で横たわっていた…");

                // 誰も見つけていない
                if (opponent.discoverer == 0 && murderer.dayDiscovere == 0)
                {
                    murderer.addMessage("あなたは" + opponent.name + "さんの第１発見者になりました");
                    murderer.addMessage("交換するアイテムを選んでください");
                    murderer.dayDiscovere = opponent.id;
                    opponent.discoverer = murderer.id;

                }

                return;
            }

            // チェーンロックを使っているかどうか
            if (opponent.getUseItem() == ITEM.CHEAN_LOCK)
            {
                opponent.addMessage("何者かから襲撃がありました");
                murderer.addMessage(opponent.name + "さんの殺害に失敗しました");
            }
            else if (opponent.dayMurdererSuccess)
            {
                // 狂気の殺人包丁で攻撃が成功している相手だった場合
                opponent.addMessage("何者かが近くにいた気配がしました");
                murderer.addMessage(opponent.name + "さんの殺害に失敗しました");
                
            }
            else
            {
                killSub(murderer, opponent, murderer.getUseItem());
            }
        }
        private void killSub(Player murderer,Player opponent ,ITEM murdere_item)
        {
            // kill
            murderer.addMessage(opponent.name + "さんの殺害に成功しました");
            murderer.killSuccess(opponent.id);
            murderer.state = PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT;
            murderer.dayUseItem = ITEM.NONE;
            DEAD_REASON reason = DEAD_REASON.NONE;
            if (murdere_item == ITEM.KNIFE)
            {
                reason = DEAD_REASON.KNIFE;
            }
            else if (murdere_item == ITEM.MURDERE_KNIFE)
            {
                reason = DEAD_REASON.MURDERER_KNIFE;
                murderer.dayMurdererSuccess = true;
            }

            // die
            int oppo_item_mknife_index = opponent.getItemIndex(ITEM.MURDERE_KNIFE);
            if (oppo_item_mknife_index != -1)
            {
                // 手放す
                opponent.setItem( oppo_item_mknife_index , ITEM.NONE);

                //自分も死んでればランダム
                if (murderer.fdead)
                {
                    randMurdererKnifeMove();
                }
                else
                {
                    // 殺人包丁をもっている場合は手に入れる
                    murderer.addItem(ITEM.MURDERE_KNIFE);
                    murderer.addMessage(opponent.name + "さんは狂気の殺人包丁を持っていました");
                    murderer.addMessage("狂気の殺人包丁を手に入れました");
                }
            }
            //アイテムをなくす
            opponent.dead(reason, murderer.id);
            opponent.addMessage("あなたは殺されました");
            opponent.state = PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT_WAIT;
        }


        // 使える人数までランダムに枝刈り
        public void unuseKnife(int maxNum)
        {
            var knifeList = GameFactory.getGame().shareData.players.getUseItemPlayers(ITEM.KNIFE);
            if(knifeList.Count <= maxNum)
            {
                return;
            }

            int[] rand = MyRandom.shuffleArray(knifeList.Count);
            
            // knife使えなかった人
            for (int i=0 ; i < knifeList.Count - maxNum; i++)
            {
                var p = knifeList[rand[i]];
                p.addMessage("今夜は人が多そうなので襲撃をやめました");
                p.dayUseItem = ITEM.NONE;
            }
        }
        // loopしている人
        public void killKnifeLoop()
        {
            var knifeList = GameFactory.getGame().shareData.players.getUseItemPlayers(ITEM.KNIFE);
            if (knifeList.Count <= 1) return;

            foreach (var p in knifeList)
            {
                if (p.fdead) continue;
                tmpKill = new List<int>();
                tmpKill.Add(p.id);
                useKnifeLoopSub(p);
            }
        }
        private void useKnifeLoopSub(Player p)
        {
            var opp = GameFactory.getGame().shareData.players.getPlayer(p.net_opp);
            if( opp.getUseItem() == ITEM.KNIFE) {
                foreach(var id in tmpKill)
                {
                    if(id == opp.id)
                    {
                        //loop用kill
                        killLoop(tmpKill);
                        return;
                    }
                }
                tmpKill.Add(opp.id);
                useKnifeLoopSub(opp);
            }
        }
        // loop用kill
        private void killLoop(List<int> killList)
        {
            bool fmur = false;
            Dictionary<int, ITEM> tmp = new Dictionary<int, ITEM>();
            foreach (var id in killList)
            {
                var p1 = GameFactory.getGame().shareData.players.getPlayer(id);
                tmp.Add(p1.id, p1.getUseItem());
                
                //loopの中に狂気もちがいたら所有者がいなくなるのでランダムにだれかにわたす
                if(p1.getItemIndex(ITEM.MURDERE_KNIFE)!= -1)
                {
                    fmur = true;
                }
            }
            foreach (var id in killList)
            {
                var p1 = GameFactory.getGame().shareData.players.getPlayer(id);
                var p2 = GameFactory.getGame().shareData.players.getPlayer(p1.net_opp);
                killSub(p1, p2, tmp[id]);
                p1.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT;
                p2.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT;
            }

            if (fmur)
            {
                randMurdererKnifeMove();
            }
        }



        // 発狂判定
        public void plusMurderer(Player p)
        {
            //if (GameFactory.getGame().shareData.field.fmid_murdere) return;
            if (p.fdead) return;
            if (!p.murderer) return;

            p.murdererTurn += 1;
            if (p.murdererTurn > GameFactory.getGame().shareData.field.dementDay)
            {
                p.addMessage("発狂死しました");
                p.dead(DEAD_REASON.HAKKYO, 0);
                p.state = PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT_WAIT;

                //殺人包丁処理
                int index = p.getItemIndex(ITEM.MURDERE_KNIFE);
                if (index != -1)
                {
                    p.setItem(index, ITEM.NONE);
                    randMurdererKnifeMove();
                }

            }
            GameFactory.getGame().shareData.field.fmid_murdere = true;
        }
        // ランダムで第1発見者を返す
        public Player getRandomFirstDscoverer()
        {
            List<Player> pl = new List<Player>();
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                //死体ものぞく
                if (p.fdead) continue;

                //殺人者は除く
                if (p.dayKill != 0) continue;

                // すでに選ばれている人は除く
                if (p.dayDiscovere != 0) continue;

                pl.Add(p);
            }

            if (pl.Count == 0) return null;
            int rand = MyRandom.rand(0, pl.Count - 1);
            return pl[rand];
        }

        //ランダムに殺人包丁を移動する
        public void randMurdererKnifeMove()
        {
            List<Player> pl = new List<Player>();
            foreach (var p2 in GameFactory.getGame().shareData.players.players)
            {
                if (p2.fdead) continue;
                pl.Add(p2);
            }
            if (pl.Count != 0)
            {
                var p3 = pl[MyRandom.rand(0, pl.Count - 1)];
                p3.addItem(ITEM.MURDERE_KNIFE);
            }
        }

        //-------------------------------------------------------
        public class KensikitResult
        {
            public Player target;
            public DEAD_REASON reason;
        }

        public KensikitResult usedKensikit(Player p)
        {
            if (p.fdead) return null;
            if (p.getUseItem() != ITEM.KENSIKIT) return null;

            KensikitResult result = new KensikitResult();
            result.target = GameFactory.getGame().shareData.players.getPlayer(p.net_opp);
            if (result.target.fdead)
            {
                result.reason = result.target.deadReason;
            }
            else
            {
                result.reason = DEAD_REASON.LIVE;
            }
            return result;
        }
        public class TantikiResult
        {
            public Player target;
            public int count;
            public ITEM item;
        }
        public TantikiResult usedTantiki(Player p)
        {
            if (p.fdead) return null;
            if (p.getUseItem() != ITEM.KENTIKI) return null;
            
            TantikiResult result = new TantikiResult();
            result.target = GameFactory.getGame().shareData.players.getPlayer(p.net_opp);
            
            int randitem = result.target.getRandItemIndex();
            if (randitem == -1)
            {
                result.count = 0;
                result.item = ITEM.NONE;
            }
            else
            {
                result.count = result.target.getItemNum();
                ITEM item = result.target.getItem(randitem);
                if(item == ITEM.MURDERE_KNIFE)
                {
                    item = ITEM.KNIFE;
                }
                result.item = item;
            }
            return result;
        }
    }

    
}

