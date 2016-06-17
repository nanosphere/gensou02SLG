using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace unity.main
{
    public class Timer : MonoBehaviour
    {
        Text text;
        Text timerButton;

        bool fstart = false;

        private float startTime = 0;
        private float limit_timer = 0;


        // Use this for initialization
        void Start()
        {
            limit_timer = game.GameFactory.getGame().info.timer_minutes;

            text = GameObject.Find("Canvas/Timer/TextTimer").GetComponent<Text>();
            timerButton = GameObject.Find("Canvas/Timer/StartTimer/Text").GetComponent<Text>();

            text.text = "" + limit_timer / 60 + ":00";
        }

        // Update is called once per frame
        void Update()
        {

            if (fstart)
            {
                text.text = getTimetStr();
            }

        }

        public string getTimetStr()
        {
            int sec = (int)Math.Floor(limit_timer - (Time.time - startTime));
            int min = (int)Math.Floor(sec / 60.0);
            sec = (int)(sec - min * 60);

            return "" + min + ":" + sec;
        }


        // 
        public void StartStopClick()
        {
            if (fstart)
            {
                // timer 終了
                text.text = "" + limit_timer / 60 + ":00";
                limit_timer = game.GameFactory.getGame().info.timer_minutes;

                timerButton.text = "Start Timer";
                fstart = false;
            }
            else
            {
                // timer 開始
                startTime = Time.time;
                timerButton.text = "Stop Timer";
                fstart = true;
            }
        }
        public void ResetClick()
        {
            timerButton.text = "Start Timer";
            fstart = false;
        }

    }
}