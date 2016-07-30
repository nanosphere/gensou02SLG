using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story.game1
{
    public class InitGame
    {
        public InitGame()
        {
            Logger.info("story.InitGame():run.");

            //親のみ
            if (!GameFactory.getGame().localData.fgm)
            {
                return;
            }

            string[] name = new string[]
            {
                "ピエロ = アンセルミ",
                "キスリツィン",
                "グントラム",
                "アレクサンデル",
                "メイ",
                "サエンス",
                "ゾーイ",
                "チアル",
                "ガープ",
                "ストライト",
                "那須 陽子",
                "長瀬 まひる",
                "宮島 優",
                "足立 みあ",
                "森山 亮介",
                "パンツェッタ 拓郎",
                "入江 莉沙",
                "岩間 寛",
                "内海 なつみ",
                "宮本 ケンイチ",
                "日高 メイサ",
                "原口 和之",
                "藤沢 ジローラモ",
                "青木 一哉",
                "アドルフ ・アイスナー",
                "カール ・クレブス",
                "エヴァ ・キャンドラー",
                "アントン ・ウィリス",
                "ヌニル",
                "ミエリワシ",
                "カーリテー",
                "ベス帝国","クレイリル"

            };
            int[] rand = MyRandom.shuffleArray(name.Length);

            // AIを追加
            for(int i = 0; i < GameFactory.getGame().shareData.field.aiNum; i++)
            {
                Logger.info("add ai");
                GameFactory.getGame().shareData.players.addPlayer(name[ rand[i] ] , ai.AI_MODE.AI1);
            }

            // pool set
            var data = GameFactory.getGame().shareData;
            data.items.setDefaultItemNum(data.players.players.Count);
            data.items.setItemPool();

            // dustアイテムを決定
            for(int i = 0; i < GameFactory.getGame().shareData.field.dustItems.Length; i++)
            {
                GameFactory.getGame().shareData.field.dustItems[i] = 0;
            }
            GameFactory.getGame().shareData.field.dustItem = (db.ITEM)MyRandom.rand(1, (int)db.ITEM.END - 1);

            // itemを配る
            foreach (var p in data.players.players)
            {
                for (int j = 0; j < 4; j++)
                {
                    p.addItem(data.items.popItem());
                }
            }

            // 早朝
            GameFactory.getGame().shareData.field.state = FIELD_STATE.EARLY_MORNING;

            // update view
            if (!GameFactory.debug)
            {
                GameFactory.getUnityManager().updateDraw(true);
            }
        }
    }
}
