using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace unity.main
{
    public class Night : MonoBehaviour
    {
        MyDropdown night_chara;

        GameObject night1;
        GameObject night2;

        Text text;

        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().night = this;

            night_chara = new MyDropdownUnity(GameObject.Find("Canvas/Night/Night2/chara").GetComponent<Dropdown>());

            night1 = GameObject.Find("Night1");
            night2 = GameObject.Find("Night2");
            night1.SetActive(false);
            night2.SetActive(false);

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
        public void onClickSendCodeVote()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NightVote(GameFactory.getGame().localData.myPlayer, night_chara.getSelect());
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);

        }
        public void onClickSendCodeYes()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NightYes(GameFactory.getGame().localData.myPlayer);
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }
        public void onClickSendCodeNo()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NightNo(GameFactory.getGame().localData.myPlayer);
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }
        

        //=====================================
        // update
        //=====================================
        public void updateDraw(bool first)
        {
            night1.SetActive(false);
            night2.SetActive(false);

            var myp = GameFactory.getGame().getMyPlayer();
            if (myp.state == game.db.PLAYER_STATE.NONE)
            {
                night1.SetActive(true);

            }else if (myp.state == game.db.PLAYER_STATE.NIGHT_VOTE)
            {
                night2.SetActive(true);

                night_chara.clear();
                night_chara.add("投票しない", -1);
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if (p.fdead) continue;
                    night_chara.add("" + p.name, p.id);
                }
                night_chara.updateDraw(first);

            }
        }
        
        
    }
}

