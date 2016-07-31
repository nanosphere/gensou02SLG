using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.story.game2;
using game.db;


namespace unity.main
{
    public class Midnight : MonoBehaviour
    {
        MyDropdown mid1_item;
        MyDropdown mid1_chara;

        MyDropdown[] mid2_item = new MyDropdown[4];

        GameObject midnight1;
        GameObject midnight2;

        bool fupdatenow = false;
        Player mid2_sendp = null;
        
        
        MidnightSub sub = new MidnightSub();

        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().midnight = this;

            

            mid1_item = new MyDropdownUnity(GameObject.Find("Canvas/Midnight/Midnight1/item").GetComponent<Dropdown>());
            mid1_chara = new MyDropdownUnity(GameObject.Find("Canvas/Midnight/Midnight1/chara").GetComponent<Dropdown>());
            
            for(int i=0;i< mid2_item.Length; i++)
            {
                mid2_item[i] = new MyDropdownUnity(GameObject.Find("Canvas/Midnight/Midnight2/item" + (i + 1)).GetComponent<Dropdown>());
            }

            midnight1 = GameObject.Find("Midnight1");
            midnight2 = GameObject.Find("Midnight2");
            midnight1.SetActive(false);
            midnight2.SetActive(false);

            if (GameFactory.getGame().shareData.field.state != game.FIELD_STATE.NONE)
            {
                updateDraw(GameFactory.getUnityManager().firstUpdate);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void onClickOk()
        {
            if (GameFactory.getUnityManager().net == null) return;
            var dat = game.net.CreateStoryCode.MidnightSelect(GameFactory.getGame().localData.myPlayer, mid1_chara.getSelect(), mid1_item.getSelect());
            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
        }
        public void onClickOk2()
        {
            if (GameFactory.getUnityManager().net == null) return;
            if (mid2_sendp == null) return;

            sub.nokoriItem(mid2_sendp, mid2_item[0].getSelect(), mid2_item[1].getSelect(), mid2_item[2].getSelect(), mid2_item[3].getSelect());
            var dat = game.net.CreateStoryCode.MidnightSelectItem(GameFactory.getGame().localData.myPlayer, mid2_sendp.id, sub.mySelectedItem.ToArray(), sub.enemySelectedItem.ToArray());

            string code = GameFactory.getNetworkManager().createCodeSendData(dat);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.Server);
            

        }
        public void onMid2Chenge()
        {
            if (!fupdatenow)
            {
                updateDraw(false);
            }
        }



        //--------------------------------------------------
        // draw
        //--------------------------------------------------
        public void updateDraw(bool first)
        {
            fupdatenow = true;
            mid2_sendp = null;

            midnight1.SetActive(false);
            midnight2.SetActive(false);

            var myp = GameFactory.getGame().getMyPlayer();

            // アイテム選択
            if (myp.state == game.db.PLAYER_STATE.NONE) {
                midnight1.SetActive(true);
                updateMidnight1(first);
            }
            else
            {   
                if (myp.state == PLAYER_STATE.MIDNIGHT_SELECT_OK_DISCOVERER_ITEM_SELECT)
                {
                    //アイテムを拾う画面
                    midnight2.SetActive(true);
                    mid2_sendp = GameFactory.getGame().shareData.players.getPlayer( myp.dayDiscovere );
                    updateMidnight2(myp, mid2_sendp, first);
                }
                if( myp.state == PLAYER_STATE.MIDNIGHT_KILL_ITEM_SELECT )
                {
                    //アイテムを拾う画面
                    midnight2.SetActive(true);
                    mid2_sendp = GameFactory.getGame().shareData.players.getPlayer(myp.dayKill);
                    updateMidnight2(myp, mid2_sendp, first);
                }
                
            }
            fupdatenow = false;
        }
        private void updateMidnight1(bool first)
        {
            {
                var myp = GameFactory.getGame().getMyPlayer();
                mid1_item.clear();
                mid1_item.add("使用しない", -1);
                for (int i = 0; i < myp.items.Length; i++)
                {
                    if (myp.getItem(i) == game.db.ITEM.NONE) continue;
                    mid1_item.add("" + myp.getItemStr(i), i);
                }
                mid1_item.updateDraw(first);
            }
            {
                mid1_chara.clear();
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if ( p.id == GameFactory.getGame().localData.myPlayer) continue;
                    mid1_chara.add(""+p.name , p.id);
                }
                mid1_chara.updateDraw(first);
            }
        }
        private void updateMidnight2(Player myp,Player opp,bool first)
        {

            // 敵のアイテムリスト
            for (int i = 0; i < opp.items.Length; i++)
            {
                var o = GameObject.Find("Canvas/Midnight/Midnight2/oppitem" + (i + 1));
                if (o == null) continue;
                o.GetComponent<Text>().text = opp.getItemStr(i);
            }

            // 自分のアイテムリスト選択候補
            for (int i = 0; i < 4; i++)
            {
                sub.setMyItemList(mid2_item[i],i,opp,first, mid2_item[0].getSelect(), mid2_item[1].getSelect(), mid2_item[2].getSelect(), mid2_item[3].getSelect());
                mid2_item[i].updateDraw(true);
            }
            
            //選択済みアイテムリストから予測する
            sub.nokoriItem(mid2_sendp, mid2_item[0].getSelect(), mid2_item[1].getSelect(), mid2_item[2].getSelect(), mid2_item[3].getSelect());
            for(int i = 0; i < sub.mySelectedItem.Count; i++)
            {
                var o = GameObject.Find("Canvas/Midnight/Midnight2/myitem" + (i + 1));
                o.GetComponent<Text>().text = "" + Player.getStr(sub.mySelectedItem[i]);
            }
            for (int i = 0; i < sub.enemySelectedItem.Count; i++)
            {
                var o = GameObject.Find("Canvas/Midnight/Midnight2/oppitem2_" + (i + 1));
                o.GetComponent<Text>().text = "" + Player.getStr(sub.enemySelectedItem[i]);
            }

        }


        

    }
}

