using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.story;

namespace game
{
    public class Game
    {
        public story.StoryManager story = new story.StoryManager();
        public History history = new History();

        public Players players = new Players();
        public List<int> item_pool = new List<int>();
        public SettingInfo info = new SettingInfo();

        public string message = "";
        public string midnightMessage = "";

        public string captivityName = "";
        public bool fcapativity = false;

        public void sync(Game o)
        {
            captivityName = o.captivityName;
            fcapativity = o.fcapativity;
            players.sync(o.players);
            message = o.message;
            midnightMessage = o.midnightMessage;
            item_pool.Clear();
            foreach(var s in o.item_pool)
            {
                item_pool.Add(s);
            }
            info.sync(o.info);
            story.sync(o.story);
            history.sync(o.history);
        }
        //--------------------------------------------------------------

        //-------------------------------
        // init
        //-------------------------------
        public void init()
        {
            initItemPool();

            for (int i=0;i<players.players.Count; i++)
            {
                Player p = players.players[i];
                p.init();
                for (int j = 0; j < 4; j++)
                {
                    p.setItem(j,item_pool[i*4+j] );
                }
            }

            story.setState(1);
        }
        private void initItemPool()
        {
            //-------------------------暫定
            if(players.players.Count==2) info.setTotalItem(1,1,4,1,1);//8
            else if (players.players.Count == 3) info.setTotalItem(1, 2, 6, 2, 1);  //12
            else if (players.players.Count == 4) info.setTotalItem(1, 3, 8, 2, 2);  //16
            else if (players.players.Count == 5) info.setTotalItem(1, 4, 10, 3, 2);  //20
            else if (players.players.Count == 6) info.setTotalItem(1, 4, 12, 4, 3);  //24
            else if (players.players.Count == 7) info.setTotalItem(1, 5, 14, 4, 4);  //28
            //-------------------------

            item_pool.Clear();
            for (int j = 0; j < info.totalItem.Length; j++)
            {
                for (int i = 0; i < info.totalItem[j]; i++)
                {
                    item_pool.Add(j+1);
                }
            }

            //足りない分
            int itemnum = players.players.Count * 4;
            for(int i=item_pool.Count; i<itemnum; i++)
            {
                item_pool.Add( MyRandom.rand(3,5));
            }

            shuffle();

        }

        //-------------------------------
        // update
        //-------------------------------
        public void update()
        {
            story.update();
        }
        
        //-------------------------------
        // シャッフル
        //-------------------------------
        public void shuffle()
        {
            var cRandom = new System.Random(Environment.TickCount);

            for (int i = item_pool.Count; i > 1; --i)
            {
                int a = i - 1;
                int b = cRandom.Next() % i;

                var tmp = item_pool[a];
                item_pool[a] = item_pool[b];
                item_pool[b] = tmp;
            }
            
        }
    }
}
