using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using game.db;

namespace game.story.game2
{
    public class Night : AStory
    {

        public override void init()
        {
            Logger.info("story.Night():init.");

            GameFactory.getGame().shareData.field.captivity = 0;
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                p.message = "";
                if (p.fdead)
                {
                    p.state = db.PLAYER_STATE.NIGHT_SELECT_END;
                }
                else
                {
                    p.state = db.PLAYER_STATE.NONE;
                }
                p.dayNightVote = 0;
            }


            GameFactory.getGame().localData.start_time = DateTime.Now;
            GameFactory.getGame().shareData.field.now_time = GameFactory.getGame().shareData.field.timer;

            // ai
            if (GameFactory.getGame().localData.fgm)
            {
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    GameFactory.getAiManager().night_ack(p);
                }
            }


        }
        public override bool end()
        {
            Logger.info("story.Night():end.");

            if (GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_END))
            {
                return true;
            }
            if (GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_END))
            {
                return true;
            }

            return false;
        }

        
        public override void run(game.net.NetworkData data)
        {
            Logger.info("story.Night():run.");

            var src = GameFactory.getGame().shareData.players.getPlayer(data.src);

            if (data.cmd == game.net.NET_COMMAND.NIGHT_ACK)
            {
                src.net_yes = data.fyes;
                src.message = "投票しました";
                src.state = PLAYER_STATE.NIGHT_SELECT_OK;
                if (!GameFactory.getGame().shareData.players.isAllPlayerState( PLAYER_STATE.NIGHT_SELECT_OK))
                {
                    return;
                }

                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    p.message = "";
                }

                //全員選択が終わった
                allselect();
            }
            else if (data.cmd == game.net.NET_COMMAND.NIGHT_VOTE)
            {
                src.state = db.PLAYER_STATE.NIGHT_VOTE_OK;
                src.net_opp = data.dest;
                src.addMessage("投票しました");

                if (!GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_OK))
                {
                    return;
                }
                

                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    p.message = "";
                }

                //全員選択が終わった
                allvote();
            }
            
        }
        

        private void allselect()
        {
            ShareData gm = GameFactory.getGame().shareData;
            gm.field.yes = 0;
            gm.field.no = 0;
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                if (p.net_yes)
                {
                    gm.field.yes++;
                }
                else
                {
                    gm.field.no++;
                }
            }

            if (gm.field.yes > gm.field.no)
            {
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if (p.fdead)
                    {
                        //死体は投票しない
                        p.state = PLAYER_STATE.NIGHT_VOTE_END;
                    }else
                    {
                        //投票へ
                        p.state = PLAYER_STATE.NIGHT_VOTE;
                    }
                }

                // ai
                if (GameFactory.getGame().localData.fgm)
                {
                    foreach (var p in GameFactory.getGame().shareData.players.players)
                    {
                        GameFactory.getAiManager().night_vote(p);
                    }
                }
            }
            else
            {
                //監禁なし
                gm.field.captivity = 0;
                gm.players.setAllState(PLAYER_STATE.NIGHT_SELECT_END);
            }


        }

        private void allvote()
        {
            ShareData gm = GameFactory.getGame().shareData;

            // 投票結果
            foreach (var p in gm.players.players)
            {
                if (p.fdead) continue;
                var p2 = gm.players.getPlayer(p.net_opp);
                if (p2 == null) continue;
                if (p2.fdead) continue;
                p2.dayNightVote++;
                
            }
            // 結果
            int max = 0;
            foreach (var p in gm.players.players)
            {
                if (max < p.dayNightVote)
                {
                    max = p.dayNightVote;
                }
            }
            List<Player> pl = new List<Player>();
            foreach (var p in gm.players.players)
            {
                if (max == p.dayNightVote)
                {
                    pl.Add(p);
                }
            }

            // 同数の場合はランダム
            int rand = MyRandom.rand(0, pl.Count - 1);
            gm.field.captivity = pl[rand].id;

            gm.players.setAllState(PLAYER_STATE.NIGHT_VOTE_END);
        }



    }
}
