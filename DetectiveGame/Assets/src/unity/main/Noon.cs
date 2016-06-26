using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class Noon : MonoBehaviour
    {
        List<GameObject> objs = new List<GameObject>();

        Dropdown itemListDropdown;
        Dropdown playerListDropdown;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().noon = this;

            itemListDropdown = GameObject.Find("Canvas/Noon/ItemList").GetComponent<Dropdown>();
            playerListDropdown = GameObject.Find("Canvas/Noon/PlayerList").GetComponent<Dropdown>();

            for (int i = 1; i <= 4; i++)
            {
                var obj = GameObject.Find("Canvas/Noon/NoonPlayer" + i);
                objs.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void updateDraw()
        {
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            
            // player list
            {
                List<string> items = new List<string>();
                foreach (var p in game.GameFactory.getGame().players.players)
                {
                    if (p.name == myp.name) continue;
                    items.Add(p.name);
                }
                game.GameFactory.getUnityManager().createDropdown(playerListDropdown, items, true);
                
            }

            // item list
            {
                List<string> items = new List<string>();
                items.Add("交換なし");
                for (int i = 0; i < myp.items.Length; i++)
                {
                    if (myp.items[i] == 1) continue;
                    items.Add("" + (i + 1) + "." + myp.getStrItem(i));
                }
                game.GameFactory.getUnityManager().createDropdown(itemListDropdown, items, true);
                itemListDropdown.value = myp.noon1.item + 1;

            }

            // player list
            {
                List<string> items = new List<string>();
                items.Add("選択しない");
                foreach (var p in game.GameFactory.getGame().players.players)
                {
                    if (p.name == myp.name) continue;
                    if (p.noon1.name == myp.name)
                    {
                        items.Add(p.name);
                    }
                }

                List<string> items2 = new List<string>();
                items2.Add("選択しない");

                // selects item list
                for (int i = 0; i < myp.items.Length; i++)
                {
                    if (myp.items[i] == 1)
                    {
                        objs[i].transform.FindChild("Name").GetComponent<Text>().text = "選べません";
                        game.GameFactory.getUnityManager().createDropdown(objs[i].transform.FindChild("ItemList").GetComponent<Dropdown>(), items2, true);
                    }
                    if (myp.noon1.item == i)
                    {
                        objs[i].transform.FindChild("Name").GetComponent<Text>().text = "交換にて選択済み";
                        game.GameFactory.getUnityManager().createDropdown(objs[i].transform.FindChild("ItemList").GetComponent<Dropdown>(), items2, true);
                    }
                    else
                    {
                        objs[i].transform.FindChild("Name").GetComponent<Text>().text = myp.getStrItem(i);
                        game.GameFactory.getUnityManager().createDropdown(objs[i].transform.FindChild("ItemList").GetComponent<Dropdown>(), items, true);
                    }

                }
            }

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateCode()
        {
            if(game.GameFactory.getGame().story.state == 3)
            {
                game.GameFactory.getUnityManager().mainCamera.createCodeField.text = CreateCode1();
            }else if (game.GameFactory.getGame().story.state == 4)
            {
                game.GameFactory.getUnityManager().mainCamera.createCodeField.text = CreateCode2();
            }else
            {
                Logger.info("Noon.CreateCode():error state. state=" + game.GameFactory.getGame().story.state);
            }

        }
        private string CreateCode1() {
            // 1
            net.NoonCode1 noon = new net.NoonCode1();
            noon.item = itemListDropdown.value - 1;
            noon.name = playerListDropdown.captionText.text;
            return game.GameFactory.getNetworkManager().createCode(noon);
        }
        private string CreateCode2() {
            // 2
            net.NoonCode2 noon = new net.NoonCode2();
            for(int i=0;i<objs.Count;i++)
            {
                net.NoonCode2Obj n = new net.NoonCode2Obj();

                n.item = i;
                //n.name = o.transform.FindChild("Name").GetComponent<Text>().text;
                n.name = objs[i].transform.FindChild("ItemList").GetComponent<Dropdown>().captionText.text;
                if(n.name == "選択しない")
                {
                    n.name = "";
                }
                noon.players.Add(n);
            }

            return game.GameFactory.getNetworkManager().createCode(noon);

        }

        //=====================================
        // onClick
        //=====================================
        public void sendCode()
        {
            string s = "";
            if (game.GameFactory.getGame().story.state == 3)
            {
                s = CreateCode1();
            }
            else if (game.GameFactory.getGame().story.state == 4)
            {
                s = CreateCode2();
            }
            else
            {
                Logger.info("Noon.CreateCode():error state. state=" + game.GameFactory.getGame().story.state);
            }
            if (s != "")
            {
                game.GameFactory.getUnityManager().net.sendCodeAll(s);
            }

        }

    }
}

