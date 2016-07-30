using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using game.db;
using game.story;

namespace game
{
    public class ShareData
    {
        
        public History history = new History();
        public Players players = new Players();
        public Items items = new Items();
        public Field field = new Field();

        public bool fdrawNewScene = false;

        public void sync(ShareData o)
        {
            history.sync(o.history);
            players.sync(o.players);
            items.sync(o.items);
            field.sync(o.field);
            fdrawNewScene = o.fdrawNewScene;
        }
    }
    public class LocalData
    {
        public int myPlayer = 0;
        public bool fgm = false;
        public bool debug_room = false;

        public DateTime start_time;
        
        public common.MessageHistory game = new common.MessageHistory(10);
        public common.MessageHistory game2 = new common.MessageHistory(10);

    }

    public class Game
    {
        public ShareData shareData = new ShareData();
        public LocalData localData = new LocalData();

        public Player getMyPlayer()
        {
            return shareData.players.getPlayer(localData.myPlayer);
        }

        public void updateMessage()
        {
            localData.game.clear();
            localData.game2.clear();

            if (shareData.field.state == FIELD_STATE.EARLY_MORNING)
            {
                localData.game.addMessage("早朝フェイズです");
                localData.game.addMessage("状況とアイテムを確認して下さい");
                if (localData.fgm)
                {
                    localData.game.addMessage("進めるを押すと進みます");
                }


                // 状況確認
                if (shareData.field.turn >= 1)
                {
                    bool f = true;
                    foreach (var p in shareData.players.players)
                    {
                        if (p.dayDead)
                        {
                            f = false;

                            localData.game2.addMessage( p.name + "さんが無残な死体で発見されました");
                            localData.game2.addMessage("　所持アイテム");
                            for (int i=0;i<p.items.Length;i++)
                            {
                                if ( p.getItem(i) == ITEM.NONE) continue;
                                localData.game2.addMessage("　　" + p.getItemStr(i));
                            }
                        }
                    }
                    if (f)
                    {
                        localData.game2.addMessage("昨晩は何事もなかったようです");
                    }

                    //終了判定
                    // todo

                }
                
            }
            else if (shareData.field.state == FIELD_STATE.MORNING)
            {
                localData.game.addMessage("朝フェイズです");
                localData.game.addMessage("状況とアイテム交換について話し合ってください");
                if (localData.fgm)
                {
                    localData.game.addMessage("進めるを押すと進みます");
                }

            }
            else if (shareData.field.state == FIELD_STATE.NOON)
            {
                localData.game.addMessage("昼フェイズ(交換タイム)です");
                localData.game.addMessage("交換を希望する相手を選択して送信ボタンを押してください");
                if (localData.fgm && shareData.players.isAllPlayerState(PLAYER_STATE.NOON_END))
                {
                    localData.game.addMessage("全員の選択が終わりました、進めるを押してください");
                }
            }
            else if (shareData.field.state == FIELD_STATE.NIGHT)
            {
                localData.game.addMessage("夜フェイズ(監禁タイム)です");
                localData.game.addMessage("話し合いののち、監禁について投票してください");
                if (localData.fgm && shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_END))
                {
                    localData.game.addMessage("全員の選択が終わりました、進めるを押してください");
                }
                if (localData.fgm && shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_END))
                {
                    localData.game.addMessage("全員の選択が終わりました、進めるを押してください");
                }

                if (shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_SELECT_END) || shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE, PLAYER_STATE.NIGHT_VOTE_OK))
                {
                    localData.game2.addMessage("投票結果");
                    localData.game2.addMessage("　投票する　:" + shareData.field.yes + "票");
                    localData.game2.addMessage("　投票しない:" + shareData.field.no + "票");
                    if (shareData.field.yes > shareData.field.no)
                    {
                        localData.game2.addMessage("　監禁するとなりました。監禁者を投票してください");
                    }
                    else
                    {
                        localData.game2.addMessage("　監禁しないとなりました。");
                    }
                }
                else if (shareData.players.isAllPlayerState(PLAYER_STATE.NIGHT_VOTE_END))
                {
                    localData.game2.addMessage("投票結果");
                    foreach (var p2 in shareData.players.players)
                    {
                        if (p2.fdead) continue;
                        localData.game2.addMessage("　" + p2.name + "：" + p2.dayNightVote + "票");
                    }
                    var p = shareData.players.getPlayer(shareData.field.captivity);
                    if (p == null)
                    {
                        localData.game2.addMessage("　error:id=" + shareData.field.captivity);
                    }
                    else
                    {
                        localData.game2.addMessage("　監禁者は" + p.name + "さんに決まりました");
                    }
                }
            }
            else if (shareData.field.state == FIELD_STATE.MIDNIGHT)
            {
                localData.game.addMessage("深夜フェイズ(アイテム使用)です");
                localData.game.addMessage("使うアイテムを選択してください");
                if (localData.fgm && shareData.players.isAllPlayerState(PLAYER_STATE.MIDNIGHT_END))
                {
                    localData.game.addMessage("全員の選択が終わりました、進めるを押してください");
                }

            }
        }

    }
}
