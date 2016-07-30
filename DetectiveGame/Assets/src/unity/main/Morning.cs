using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using game.story.game2;
using System;

namespace unity.main
{
    public class Morning : MonoBehaviour
    {
        Text text;
        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().morning = this;

            text = GameObject.Find("Canvas/Timer").GetComponent<Text>();

            if (GameFactory.getGame().shareData.field.state != game.FIELD_STATE.NONE)
            {
                updateDraw(GameFactory.getUnityManager().firstUpdate);
            }


        }

        // Update is called once per frame
        void Update()
        {
            GameFactory.getGame().shareData.field.now_time =
                (int)Math.Floor(GameFactory.getGame().shareData.field.timer -
                    (DateTime.Now - GameFactory.getGame().localData.start_time).TotalSeconds);

            text.text = getTimetStr(GameFactory.getGame().shareData.field.now_time);
        }
    
        public string getTimetStr(int sec)
        {
            if (sec < 0)
            {
                sec = 0;
            }
            int min = (int)Math.Floor(sec / 60.0);
            sec = (int)(sec - min * 60);

            return "" + min + ":" + sec;
        }


        //=====================================
        // onClick
        //=====================================


        //--------------------------------------------------
        // draw
        //--------------------------------------------------
        public void updateDraw(bool first)
        {

        }
        

    }
}

