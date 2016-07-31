using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using game.story.game2;

namespace unity.main
{
    public class Noon : MonoBehaviour
    {

        MyDropdown noon1_chara;
        MyDropdown noon4_item;

        GameObject noon1;
        GameObject noon2;
        GameObject noon3;
        GameObject noon4;

        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().noon = this;


            noon1_chara = new MyDropdownUnity(GameObject.Find("Canvas/Noon/Noon1/chara").GetComponent<Dropdown>());
            noon4_item = new MyDropdownUnity(GameObject.Find("Canvas/Noon/Noon4/item").GetComponent<Dropdown>());


            noon1 = GameObject.Find("Noon1");
            noon2 = GameObject.Find("Noon2");
            noon3 = GameObject.Find("Noon3");
            noon4 = GameObject.Find("Noon4");
            noon1.SetActive(false);
            noon2.SetActive(false);
            noon3.SetActive(false);
            noon4.SetActive(false);


            if (GameFactory.getGame().shareData.field.state != game.FIELD_STATE.NONE)
            {
                updateDraw(GameFactory.getUnityManager().firstUpdate);
            }


        }

        // Update is called once per frame
        void Update()
        {
        }

        //=====================================
        // onClick
        //=====================================
        public void onClickSendCodeRequest()
        {
            if (GameFactory.getUnityManager().net == null) return;

            var dat = game.net.CreateStoryCode.NoonRequest(GameFactory.getGame().localData.myPlayer, noon1_chara.getSelect());
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);

        }
        public void onClickSendCodeYes()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NoonYes(GameFactory.getGame().localData.myPlayer);
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }
        public void onClickSendCodeNo()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NoonNo(GameFactory.getGame().localData.myPlayer);
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }
        public void onClickSendCodeItem()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NoonItem(GameFactory.getGame().localData.myPlayer, noon4_item.getSelect());
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }

        public void onClickEndCode()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.NoonEnd(GameFactory.getGame().localData.myPlayer);
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }

        //--------------------------------------------------
        // draw
        //--------------------------------------------------
        public void updateDraw(bool first)
        {
            noon1.SetActive(false);
            noon2.SetActive(false);
            noon3.SetActive(false);
            noon4.SetActive(false);

            var myp = GameFactory.getGame().getMyPlayer();

            if (myp.state == game.db.PLAYER_STATE.NOON_END)
            {
                //終了
            }
            else if (myp.state == game.db.PLAYER_STATE.NONE)
            {
                // 選択画面
                noon1.SetActive(true);

                noon1_chara.clear();
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if (p.id == GameFactory.getGame().localData.myPlayer) continue;
                    if (p.state == game.db.PLAYER_STATE.NOON_END) continue;

                    noon1_chara.add("" + p.name, p.id);
                }
                noon1_chara.updateDraw(first);

            }
            else if (myp.state == game.db.PLAYER_STATE.NOON_WAIT_ACK)
            {
                //待ち
                noon2.SetActive(true);
            }
            else if (myp.state == game.db.PLAYER_STATE.NOON_REQUEST_RETURN)
            {
                //返答
                noon3.SetActive(true);

                var opp_id = GameFactory.getGame().getMyPlayer().net_opp;
                var opp = GameFactory.getGame().shareData.players.getPlayer(opp_id);
                GameObject.Find("Canvas/Noon/Noon3/opp_text").GetComponent<Text>().text = opp.name + "から交換要望がありました";

            }
            else if (myp.state == game.db.PLAYER_STATE.NOON_ITEM)
            {
                //アイテム選択
                noon4.SetActive(true);

                noon4_item.clear();
                for (int i = 0; i < myp.items.Length; i++)
                {
                    if (myp.getItem(i) == game.db.ITEM.MURDERE_KNIFE) continue;
                    if (myp.getItem(i) == game.db.ITEM.NONE) continue;
                    noon4_item.add("" + myp.getItemStr(i), i);
                }
                noon4_item.updateDraw(first);

            }

        }
        
    }
}

