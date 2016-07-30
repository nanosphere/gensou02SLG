using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.story.game2;
using game.db;


namespace unity.main
{
    public class DebugRoom : MonoBehaviour
    {
        List<GameObject> objs = new List<GameObject>();


        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().debug_room = this;

            
            GameFactory.getGame().localData.debug_room = true;

            GameObject canvasObject = GameObject.Find("Canvas");
            GameObject original = GameObject.Find("Canvas/Player");
            
            //var vec = original.transform.position;
            //for (int i = 0; i < 10; i++)
            for (int i = 0; i < GameFactory.getGame().shareData.players.players.Count; i++)
            {

                //GameObject copied = Instantiate(original, vec2, Quaternion.identity) as GameObject;
                GameObject copied = Instantiate(original) as GameObject;
                copied.transform.SetParent(canvasObject.transform, false);
                //copied.transform.localPosition = Camera.main.ScreenToWorldPoint(copied.transform.localPosition);
                copied.name = "Player" + i;
                //copied.transform.Translate(0, - copied.GetComponent<SpriteRenderer>().bounds.size.y /10 * i, 0);
                Vector3 v = copied.transform.position;
                v.y -= 100.0f * i;
                copied.transform.position = v;

                //copied.transform.Translate(0, -5 * i, 0, Camera.main.transform);
                //                copied.transform.Translate(0, -5 * i, 0, Space.World);

                //GameObject childObject = copied.transform.FindChild("Name").gameObject;
                //childObject.transform.Translate(0, -i*10, 0);

                objs.Add(copied);
                var p = GameFactory.getGame().shareData.players.players[i];
                updateDrawPlayer(copied, p);
            }
            original.SetActive(false);
            updateDraw();
        }

        // Update is called once per frame
        void Update()
        {
        }

        //--------------------------------------------------
        // click
        //--------------------------------------------------
        public void onClickInitGame()
        {
            new game.story.net.AddPlayer("p1");
            new game.story.net.AddPlayer("p2");
            new game.story.net.AddPlayer("p3");
            GameFactory.getGame().localData.fgm = true;
            GameFactory.getGame().localData.myPlayer = GameFactory.getGame().shareData.players.getPlayer("p1").id;
            new game.story.game1.InitGame();
        }



        //debug
        /*
        new game.story.net.AddPlayer("p1");
        new game.story.net.AddPlayer("p2");
        new game.story.net.AddPlayer("p3");
        GameFactory.getGame().localData.fgm = true;
        GameFactory.getGame().localData.myPlayer = GameFactory.getGame().shareData.players.getPlayer("p1").id;
        new game.story.game1.InitGame();
        new game.story.game1.NextTurn();
        new game.story.game1.NextTurn();
        { var dat = game.net.CreateStoryCode.NoonEnd(); dat.src = 1; new game.story.game2.Noon2(dat); }
        { var dat = game.net.CreateStoryCode.NoonEnd(); dat.src = 2; new game.story.game2.Noon2(dat); }
        { var dat = game.net.CreateStoryCode.NoonEnd(); dat.src = 3; new game.story.game2.Noon2(dat); }
        new game.story.game1.NextTurn();
        { var dat = game.net.CreateStoryCode.NightNo(); dat.src = 1; new game.story.game2.Night2(dat); }
        { var dat = game.net.CreateStoryCode.NightNo(); dat.src = 2; new game.story.game2.Night2(dat); }
        { var dat = game.net.CreateStoryCode.NightNo(); dat.src = 3; new game.story.game2.Night2(dat); }
        new game.story.game1.NextTurn();
        GameFactory.getGame().shareData.players.getPlayer(1).setItem(0, game.db.ITEM.KNIFE);
        { var dat = game.net.CreateStoryCode.MidnightSelect(2, 0); dat.src = 1; new game.story.game2.MidNight2(dat); }
        { var dat = game.net.CreateStoryCode.MidnightSelect(1, -1); dat.src = 2; new game.story.game2.MidNight2(dat); }
        { var dat = game.net.CreateStoryCode.MidnightSelect(1, -1); dat.src = 3; new game.story.game2.MidNight2(dat); }

         */

        //--------------------------------------------------
        // draw
        //--------------------------------------------------
        public void updateDrawPlayer(GameObject obj, Player p)
        {
            {
                string s = "";
                s += p.toLine();
                obj.transform.FindChild("State").gameObject.GetComponent<Text>().text = s;
            }
            {
                var o = obj.transform.FindChild("state0").gameObject.GetComponent<DebugRoomState>();
                o.player = p;
                MyDropdown drop = new MyDropdownUnity(obj.transform.FindChild("state0").gameObject.GetComponent<Dropdown>());
                for (int j = 0; j < (int)PLAYER_STATE.END; j++)
                {
                    drop.add(((PLAYER_STATE)j).ToString(), j);
                }
                drop.updateDraw(true);
                drop.select((int)p.state);
            }
            for (int i = 0; i < 4; i++)
            {
                var o = obj.transform.FindChild("item" + (i + 1)).gameObject.GetComponent<Dropdown>();
                var o2 = obj.transform.FindChild("item" + (i + 1)).gameObject.GetComponent<DebugRoomItem>();
                o2.item_index = i;
                o2.player = p;
                MyDropdown drop = new MyDropdownUnity(o);
                for (int j = 0; j < (int)ITEM.END; j++)
                {
                    drop.add(((ITEM)j).ToString(), j);
                }
                drop.updateDraw(true);
                drop.select((int)p.getItem(i));
            }

        }
        public void updateDraw()
        {
            {
                string s = "";
                s += GameFactory.getGame().shareData.field.toLine();
                GameObject.Find("Canvas/Field").GetComponent<Text>().text = s;
            }
        }
    }
}

