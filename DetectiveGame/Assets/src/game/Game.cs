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

        public List<Player> players = new List<Player>();
        public List<int> item_pool = new List<int>();

        public SettingInfo info = new SettingInfo();

        public int state = 0;
        public int turn = 0;

        public string message = "";

        public void sync(Game o)
        {
            foreach (var p in o.players)
            {
                if( isPlayer(p.name))
                {
                    p.sync(p);
                }else
                {
                    Player p2 = new Player();
                    p2.sync(p);
                    players.Add(p2);
                }
            }

            item_pool.Clear();
            foreach(var s in o.item_pool)
            {
                item_pool.Add(s);
            }
            state = o.state;
            turn = o.turn;
            info.sync(o.info);

        }

        //--------------------------------------------------------------
        private AStory story = null;


        public void addPlayer(string name)
        {
            if (isPlayer(name))
            {
                Logger.info("player is already.");
                return;
            }
            var p = new Player();
            p.name = name;

            players.Add(p);

        }

        public bool isPlayer(string name)
        {
            foreach (var o in players)
            {
                if (o.name == name)
                {
                    return true;
                }
            }
            return false;
        }
        public Player getPlayer(string name)
        {
            foreach (var o in players)
            {
                if (o.name == name)
                {
                    return o;
                }
            }
            return null;
        }

        //-------------------------------
        // init
        //-------------------------------
        public void init()
        {
            int itemnum = players.Count * 4 - 1;
            item_pool.Add(1);

            for (int i = 0; i < itemnum/2 ; i++)
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

            for(int i=0;i<players.Count; i++)
            {
                Player p = players[i];
                p.init();
                for (int j = 0; j < 4; j++)
                {
                    p.addItem(item_pool[i*4+j] );
                }
            }

            setState(1);
            
        }
        
        public void setState(int state)
        {
            this.state = state;

            if (state == 1){      story = new EarlyMorning(this); }
            else if (state == 2){            }
            else if (state == 3)
            {
            }
            else if (state == 4)
            {
            }
            else if (state == 5){ story = new MidNight(this); }


            //turnの初期化
            story.init();
            
        }
        public void nextTurn()
        {
            // turnを進める
            story.doit();

            // 次へ
            state += 1;
            if(state >= 6)
            {
                state = 1;
            }
            setState(state);
        }


        public void onUpdate()
        {
            story.onUpdate();
        }
        

        //-------------------------------
        // 
        //-------------------------------
        public string toState()
        {
            if (state == 1) return "早朝フェーズ";
            if (state == 2) return "朝フェーズ";
            if (state == 3) return "昼フェーズ";
            if (state == 4) return "夜フェーズ";
            if (state == 5) return "深夜フェーズ";

            return "";
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
