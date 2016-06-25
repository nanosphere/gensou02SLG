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

            for (int i = 1; i <= 9; i++)
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
            // player list
            game.GameFactory.getUnityManager().createPlayerDropdown(playerListDropdown);

            // item list
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            List<string> items = new List<string>();
            items.Add("交換なし");
            for (int i=0;i<myp.items.Length; i++)
            {
                items.Add("" + (i + 1) + "." + myp.getStrItem(i));
            }
            game.GameFactory.getUnityManager().createDropdown(itemListDropdown,items,true);
            itemListDropdown.value = myp.noon1.item + 1;

            // selects item list
            int j = 0;
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                objs[j].transform.FindChild("Name").GetComponent<Text>().text = p.name;
                game.GameFactory.getUnityManager().createDropdown(objs[j].transform.FindChild("ItemList").GetComponent<Dropdown>(), items, true);
                j++;
            }

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateCode()
        {
            if(game.GameFactory.getGame().story.state == 3)
            {
                CreateCode1();
            }else if (game.GameFactory.getGame().story.state == 4)
            {
                CreateCode2();
            }else
            {
                Logger.info("Noon.CreateCode():error state. state=" + game.GameFactory.getGame().story.state);
            }

        }
        private void CreateCode1() {
            // 1
            net.NoonCode1 noon = new net.NoonCode1();
            noon.item = itemListDropdown.value - 1;
            noon.name = playerListDropdown.captionText.text;
            game.GameFactory.getUnityManager().mainCamera.createCodeField.text = game.GameFactory.getNetworkManager().createCode(noon);
        }
        private void CreateCode2() {
            // 2
            net.NoonCode2 noon = new net.NoonCode2();
            foreach (var o in objs)
            {
                net.NoonCode2Obj n = new net.NoonCode2Obj();

                n.name = o.transform.FindChild("Name").GetComponent<Text>().text;
                Dropdown d = o.transform.FindChild("ItemList").GetComponent<Dropdown>();
                n.item = d.value-1;

                noon.players.Add(n);
            }

            game.GameFactory.getUnityManager().mainCamera.createCodeField.text = game.GameFactory.getNetworkManager().createCode(noon);

        }
    }
}

