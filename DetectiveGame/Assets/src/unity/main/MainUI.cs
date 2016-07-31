using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class MainUI : MonoBehaviour
    {
        // publicで宣言し、inspectorで設定可能にする
        public Sprite none;
        public Sprite knife;
        public Sprite murderer_knife;
        public Sprite chein;
        public Sprite kensiki;
        public Sprite kensikit;

        GameObject nextTurn;
        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().mainui = this;

            nextTurn = GameObject.Find("MainUI/NextTurn");

            if (GameFactory.getGame().shareData.field.state != game.FIELD_STATE.NONE)
            {
                updateDraw();
            }

        }

        // Update is called once per frame
        void Update()
        {
            
        }


        //=====================================
        // onClick
        //=====================================
        public void onClickSendMessage(int id)
        {
            var obj = GameObject.Find("MainUI/Player/PlayerInput" + id + "/Text");
            if (obj == null) return;
            Text text = obj.GetComponent<Text>();
            if (text.text == "") return;
            string s = GameFactory.getNetworkManager().createCodeSendMessage(id,text.text);
            GameObject.Find("MainUI/Player/PlayerInput" + id + "").GetComponent<InputField>().text = "";
            GameFactory.getUnityManager().net.sendCode(s,RPCMode.All);

            GameFactory.getUnityManager().updateDraw(false);
        }
        public void onClickNextTurn()
        {
            
            if (GameFactory.getGame().localData.fgm)
            {
                new game.story.game1.NextTurn();

                
            }
            GameFactory.getUnityManager().updateDraw(true);
            
        }


        //=====================================
        // state
        //=====================================
        public void updateDraw()
        {
            updateItemList();
            updatePlayerList();
            updateLog();

            // dust box
            {
                string s = "";
                s += "ダストボックス("+game.db.Player.getStr(GameFactory.getGame().shareData.field.dustItem)+")：";
                s += ""+GameFactory.getGame().shareData.field.dustItems[(int)GameFactory.getGame().shareData.field.dustItem] + "枚";
                GameObject.Find("MainUI/DustBox").GetComponent<Text>().text = s;
            }
            //start button
            if (GameFactory.getGame().localData.fgm)
            {
                nextTurn.SetActive(true);
            }else
            {
                nextTurn.SetActive(false);
            }
        }
        private void updateItemList()
        {
            var myp = GameFactory.getGame().getMyPlayer();
            for (int i=0;i<myp.items.Length;i++)
            {
                {
                    var obj = GameObject.Find("MainUI/Item/Item" + (i + 1));
                    if (obj == null) continue;
                    var text = obj.GetComponent<Text>();
                    string s = myp.getItemStr(i);
                    if (s == "")
                    {
                        s += "所持なし";
                    }
                    text.text = s;
                }
                {
                    var obj = GameObject.Find("MainUI/Item/ItemImg" + (i + 1));
                    if (obj == null) continue;
                    var img = obj.GetComponent<Image>();
                    switch (myp.getItem(i))
                    {
                        case game.db.ITEM.MURDERE_KNIFE: img.sprite = murderer_knife; break;
                        case game.db.ITEM.KNIFE: img.sprite = knife; break;
                        case game.db.ITEM.CHEAN_LOCK: img.sprite = chein; break;
                        case game.db.ITEM.KENSIKIT: img.sprite = kensikit; break;
                        case game.db.ITEM.KENTIKI: img.sprite = kensiki; break;
                        default:
                            img.sprite = none;
                            break;
                    }
                }
            }
            
        }
        private void updatePlayerList()
        {
            for (int i = 0; i < 8; i++)
            {
                {
                    var obj = GameObject.Find("MainUI/Player/Player" + (i + 1));
                    if (obj != null)
                    {

                        var p = GameFactory.getGame().shareData.players.getPlayer(i + 1);
                        if (p == null)
                        {
                            obj.GetComponent<Text>().text = "";
                        }
                        else
                        {
                            obj.GetComponent<Text>().text = p.name;
                        }
                    }
                }
                {
                    var obj = GameObject.Find("MainUI/Player/PlayerState" + (i + 1));
                    if (obj != null)
                    {

                        var p = GameFactory.getGame().shareData.players.getPlayer(i + 1);
                        if (p == null)
                        {
                            obj.GetComponent<Text>().text = "";
                        }
                        else
                        {
                            string s = "";
                            if (p.id == GameFactory.getGame().localData.myPlayer)
                            {
                                // 自分のみの情報
                                if (p.murderer)
                                {
                                    s += "<color=#ff0000ff>マーダー(" + (p.murdererTurn)+ ")</color> ";
                                }
                                else
                                {
                                    s += "正常 ";
                                }
                            }
                            if (p.isSelectState(GameFactory.getGame().shareData.field.state))
                            {
                                s += "<color=#ffff00ff>選択中</color> ";
                            }
                            
                            if (p.fdead)
                            {
                                //その日の深夜は更新しない
                                if (!(p.dayDead && GameFactory.getGame().shareData.field.state == game.FIELD_STATE.MIDNIGHT))
                                {
                                    s += "<color=#ff0000ff>死体</color> ";
                                }
                            }
                            if (GameFactory.getGame().shareData.field.captivity == p.id)
                            {
                                s += "監禁中 ";
                            }
                            obj.GetComponent<Text>().text = s;
                        }
                    }
                }
                {
                    var obj = GameObject.Find("MainUI/Player/PlayerInput" + (i + 1));
                    if (obj != null)
                    {
                        if( i >= GameFactory.getGame().shareData.players.players.Count)
                        {
                            obj.SetActive(false);
                        }
                    }

                }
            }
            
        }
        private void updateLog()
        {
            var text1 = GameObject.Find("MainUI/Message/Panel1/Message1").GetComponent<Text>();
            var text2 = GameObject.Find("MainUI/Message/Panel2/Message2").GetComponent<Text>();
            var text3 = GameObject.Find("MainUI/Message/Panel3/Message3").GetComponent<Text>();
            var text4 = GameObject.Find("MainUI/Message/Panel4/Message4").GetComponent<Text>();
            text1.text = "";
            text2.text = "";
            text3.text = "";
            text4.text = "";

            var gm = GameFactory.getGame();
            gm.updateMessage();
            var myp = gm.getMyPlayer();

            // net関係
            if (GameFactory.getUnityManager().net != null)
            {
                string s1 = "";
                s1 = GameFactory.getNetworkManager().messages.getMessage(true,true);
                text4.text = s1;
            }

            // player
            if (myp.message != "")
            {
                text2.text = myp.message;
            }

            //game 関係
            {
                text1.text = gm.localData.game.getMessage(false, false);
            }
            {
                text3.text = gm.localData.game2.getMessage(false,false);
            }
        }
    }
}

