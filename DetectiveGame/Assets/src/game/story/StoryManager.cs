using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.story;

namespace game
{
    public class StoryManager
    {
        public int state = 0;
        public int turn = 0;
        public int netState = 0;

        private AStory story = null;

        
        public void sync(StoryManager o)
        {
            state = o.state;
            turn = o.turn;
            netState = o.netState;

            setState(state);
        }
        

        //-------------------------------
        // update
        //-------------------------------
        public void update()
        {
            if (story != null)
            {
                story.update();
            }
        }
        

        //-------------------------------
        // set
        //-------------------------------
        public void setState(int state)
        {
            this.state = state;

            if (state == 1){      story = new EarlyMorning(GameFactory.getGame()); }
            else if (state == 2){            }
            else if (state == 3)
            {
            }
            else if (state == 4)
            {
            }
            else if (state == 5){ story = new MidNight(GameFactory.getGame()); }


            // messageをリセット
            foreach (var p in GameFactory.getGame().players.players)
            {
                p.message = "";
            }
            
            
        }
        public void nextTurn()
        {
            
            // 次へ
            state += 1;
            if(state >= 6)
            {
                turn += 1;
                state = 1;
            }
            setState(state);

            // turnを進める
            story.frun = true;

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
        
    }
}
