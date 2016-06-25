using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.story;

namespace game.story
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

            setState(state,true);
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
        public void setState(int state,bool frun)
        {
            this.state = state;
            Logger.info("StoryManager.setState():state="+state);

            if (state == 1){       story = new EarlyMorning(GameFactory.getGame()); }
            else if (state == 2) { story = new Morning(GameFactory.getGame()); }

            else if (state == 3) { story = new Noon(GameFactory.getGame(),0); }
            else if (state == 4) { story = new Noon(GameFactory.getGame(),1); }
            else if (state == 5) { story = new Noon(GameFactory.getGame(),2); }

            else if (state == 6) { story = new Night(GameFactory.getGame(),0); }
            else if (state == 7) { story = new Night(GameFactory.getGame(),1); }
            else if (state == 8) { story = new Night(GameFactory.getGame(),2); }

            else if (state == 9) { story = new MidNight(GameFactory.getGame(),0); }
            else if (state == 10) { story = new MidNight(GameFactory.getGame(),1); }
            else if (state == 11) { story = new MidNight(GameFactory.getGame(), 2); }
            else if (state == 12) { story = new MidNight(GameFactory.getGame(), 3); }

            else
            {
                story = null;
            }
            
            if (story != null && frun)
            {
                // turnを進める
                story.frun = true;
                story.init();
            }

        }
        public void nextTurn()
        {
            
            // 次へ
            state += 1;
            if(state >= 13)
            {
                turn += 1;
                state = 1;
            }
            setState(state,true);


            // messageをリセット
            foreach (var p in GameFactory.getGame().players.players)
            {
                p.message = "";
            }
        }


        //-------------------------------
        // 
        //-------------------------------
        public string toState()
        {
            if (state == 1) return "早朝フェーズ";
            if (state == 2) return "朝フェーズ";

            if (state == 3) return "昼フェーズ";
            if (state == 4) return "昼フェーズ";
            if (state == 5) return "昼フェーズ";

            if (state == 6) return "夜フェーズ";
            if (state == 7) return "夜フェーズ";
            if (state == 8) return "夜フェーズ";

            if (state == 9) return "深夜フェーズ";
            if (state == 10) return "深夜フェーズ";
            if (state == 11) return "深夜フェーズ";
            if (state == 12) return "深夜フェーズ";

            return "";
        }
        
    }
}
