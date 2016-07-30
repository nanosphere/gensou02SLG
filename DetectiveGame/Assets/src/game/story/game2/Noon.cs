using game.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.story.game2
{
    public class Noon : AStory
    {
        public override void init()
        {
            Logger.info("story.Noon():init.");

            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                p.message = "";
                if (p.fdead)
                {
                    p.state = db.PLAYER_STATE.NOON_END;
                }
                else
                {
                    p.state = db.PLAYER_STATE.NONE;
                }
                p.dayNoonCount = 0;
            }

            // ai
            if (GameFactory.getGame().localData.fgm)
            {
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    GameFactory.getAiManager().noon_end(p);
                }
            }
        }
        

        public override bool end()
        {
            Logger.info("story.Noon():end.");

            if(GameFactory.getGame().shareData.players.isAllPlayerState(PLAYER_STATE.NOON_END))
            {
                return true;
            }
            return false;

        }


        public override void run(game.net.NetworkData data)
        {
            Logger.info("story.Noon():run.");

            var src = GameFactory.getGame().shareData.players.getPlayer(data.src);
            var dest = GameFactory.getGame().shareData.players.getPlayer(data.dest);


            if ( src.state == PLAYER_STATE.NOON_END )
            {
                return;
            }

            if (data.cmd == game.net.NET_COMMAND.NOON_REQUEST)
            {
                request(src,dest,data);
            }
            else if (data.cmd == game.net.NET_COMMAND.NOON_ACK)
            {
                ack(src, dest, data);
            }
            else if (data.cmd == game.net.NET_COMMAND.NOON_ITEM)
            {
                item(src, dest, data);
            }
            else if (data.cmd == game.net.NET_COMMAND.NOON_END)
            {
                // 終わり
                src.state = PLAYER_STATE.NOON_END;
                src.addMessage("交換を終了しました");
            }

            

        }
        private void request(Player src,Player dest, game.net.NetworkData data)
        {
            // 交換要求
            if (dest == null)
            {
                return;
            }
            if (src.state != db.PLAYER_STATE.NONE)
            {
                src.addMessage("交換中です");
            }
            else if (dest.state != db.PLAYER_STATE.NONE)
            {
                src.addMessage(dest.name + "さんは交換中です");
            }
            else
            {
                src.state = db.PLAYER_STATE.NOON_WAIT_ACK;
                dest.state = db.PLAYER_STATE.NOON_REQUEST_RETURN;
                src.net_opp = dest.id;
                dest.net_opp = src.id;
                src.addMessage(dest.name + "さんに交換申請しました。返答を待っています。");
                dest.addMessage(src.name + "さんから交換申請がありました。返信してください。");
                src.dayNoonCount += 1;

                // ai
                if (GameFactory.getGame().localData.fgm)
                {
                    GameFactory.getAiManager().noon_ack(dest);
                }
            }
            
        }

        private void ack(Player src, Player dest, game.net.NetworkData data)
        {
            if (dest == null)
            {
                return;
            }
            if (dest.state != PLAYER_STATE.NOON_WAIT_ACK)
            {
                src.state = PLAYER_STATE.NONE;
                dest.state = PLAYER_STATE.NONE;
                src.addMessage("内部エラーです。交換前に戻します");
                dest.addMessage("内部エラーです。交換前に戻します");
                return;
            }
            if (src.state != PLAYER_STATE.NOON_REQUEST_RETURN)
            {
                src.state = PLAYER_STATE.NONE;
                dest.state = PLAYER_STATE.NONE;
                src.addMessage("内部エラーです。交換前に戻します");
                dest.addMessage("内部エラーです。交換前に戻します");
                return;
            }
            if (data.fyes)
            {
                src.state = db.PLAYER_STATE.NOON_ITEM;
                dest.state = db.PLAYER_STATE.NOON_ITEM;

                src.addMessage(dest.name + "さんと交換が成立しました。アイテムを選んでください。");
                dest.addMessage(src.name + "さんと交換が成立しました。アイテムを選んでください。");

                // ai
                if (GameFactory.getGame().localData.fgm)
                {
                    GameFactory.getAiManager().noon_item(dest, src);
                    GameFactory.getAiManager().noon_item(src, dest);
                }
            }
            else
            {
                src.state = db.PLAYER_STATE.NONE;
                dest.state = db.PLAYER_STATE.NONE;

                src.addMessage(dest.name + "さんとの交換はできませんでした");
                dest.addMessage(src.name + "さんとの交換はできませんでした");

                if (dest.dayNoonCount >= 1)
                {
                    //交換上限
                    dest.addMessage("交換回数の上限を超えました。交換は終了します");
                    dest.state = PLAYER_STATE.NOON_END;
                }
            }
        }

        private void item(Player src, Player dest, game.net.NetworkData data)
        {
            var oppo = GameFactory.getGame().shareData.players.getPlayer(src.net_opp);

            if (src.state != PLAYER_STATE.NOON_ITEM || oppo == null )
            {
                src.state = PLAYER_STATE.NONE;
                src.addMessage("内部エラーです。交換前に戻します");
                if (oppo != null)
                {
                    oppo.state = PLAYER_STATE.NONE;
                    oppo.addMessage("内部エラーです。交換前に戻します");
                }
                return;
            }

            src.state = db.PLAYER_STATE.NOON_ITEM_OK;
            src.net_item = data.item;
            src.addMessage("アイテムを選択しました");

            if (oppo.state == db.PLAYER_STATE.NOON_ITEM_OK && src.state == db.PLAYER_STATE.NOON_ITEM_OK)
            {

                src.addMessage(oppo.name + "さんとアイテムを交換しました");
                src.addMessage("　" + src.getItemStr(src.net_item) + "(" + src.name + ")");
                src.addMessage("　" + oppo.getItemStr(oppo.net_item) + "(" + oppo.name + ")");

                oppo.addMessage(src.name + "さんとアイテムを交換しました");
                oppo.addMessage("　" + oppo.getItemStr(oppo.net_item) + "(" + oppo.name + ")");
                oppo.addMessage("　" + src.getItemStr(src.net_item) + "(" + src.name + ")");


                //交換を実施
                ITEM tmp = src.getItem(src.net_item);
                src.setItem(src.net_item, oppo.getItem(oppo.net_item));
                oppo.setItem(oppo.net_item, tmp);

                src.state = db.PLAYER_STATE.NONE;
                oppo.state = db.PLAYER_STATE.NONE;


                if (src.dayNoonCount >= 1)
                {
                    //交換上限
                    src.addMessage("交換回数の上限を超えました。交換は終了します");
                    src.state = PLAYER_STATE.NOON_END;
                }
                if (oppo.dayNoonCount >= 1)
                {
                    //交換上限
                    oppo.addMessage("交換回数の上限を超えました。交換は終了します");
                    oppo.state = PLAYER_STATE.NOON_END;
                }

            }
        }

    }
}
