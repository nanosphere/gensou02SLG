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
        public History history = new History();

        public Players players = new Players();
        public List<int> item_pool = new List<int>();
        public SettingInfo info = new SettingInfo();
        
        public string message = "";

        public story.StoryManager story = new story.StoryManager();


        public void sync(Game o)
        {
            players.sync(o.players);

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
            int itemnum = players.players.Count * 4 - 1;
            item_pool.Add(1);

            for (int i = 0; i < itemnum/2+2 ; i++)
            {
                item_pool.Add(3);
            }
            for (int i = 0; i < itemnum/4; i++)
            {
                item_pool.Add(2);
            }
            for (int i = 0; i < itemnum/4; i++)
            {
                item_pool.Add(4);
            }
            for (int i = 0; i < itemnum/4; i++)
            {
                item_pool.Add(5);
            }
            shuffle();

            for(int i=0;i<players.players.Count; i++)
            {
                Player p = players.players[i];
                p.init();
                for (int j = 0; j < 4; j++)
                {
                    p.addItem(item_pool[i*4+j] );
                }
            }

            story.setState(1);
            
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
