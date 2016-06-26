using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class Midnight : MonoBehaviour
    {
        List<GameObject> objs = new List<GameObject>();

        Dropdown itemListDropdown;
        Dropdown playerListDropdown;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().midnight = this;

            itemListDropdown = GameObject.Find("Canvas/Midnight/ItemList").GetComponent<Dropdown>();
            playerListDropdown = GameObject.Find("Canvas/Midnight/PlayerList").GetComponent<Dropdown>();
            
            for (int i = 1; i <= 8; i++)
            {
                var obj = GameObject.Find("Canvas/Midnight/NightItem" + i);
                objs.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void updateDraw()
        {
            updateItemList();
            updatePlayerList();
            updateItemList2();
        }
        private void updateItemList()
        {
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            List<string> items = new List<string>();
            items.Add("使用しない");
            for (int i = 0; i < myp.items.Length; i++)
            {
                objs[i].transform.FindChild("Item").GetComponent<Text>().text = ""+(i+1)+"."+myp.getStrItem(i);
                items.Add("" + (i + 1) + "." + myp.getStrItem(i));
            }
            game.GameFactory.getUnityManager().createDropdown(itemListDropdown, items, true);
        }
        private void updatePlayerList()
        {
            game.GameFactory.getUnityManager().createPlayerDropdown(playerListDropdown);
        }
        
        private void updateItemList2()
        {
            var myp = game.GameFactory.getGame().players.getMyPlayer();
            game.Player enep = null;
            if (game.GameFactory.getGame().story.state == 10) {
                enep = game.GameFactory.getGame().players.getPlayer(myp.killName);
            }
            else if (game.GameFactory.getGame().story.state == 11)
            {
                foreach (var p in game.GameFactory.getGame().players.players)
                {
                    if ( p.fdeadToday && p.firstDiscoverer == myp.name)
                    {
                        enep = p;
                        break;
                    }
                }
            }
            if (enep == null)
            {
                foreach (var o in objs)
                {
                    o.transform.FindChild("Item").GetComponent<Text>().text = "";
                }
                return;
            }
            int i = 0;
            for (int j=0; j<4;j++)
            {
                objs[i].transform.FindChild("Item").GetComponent<Text>().text = myp.name +":"+myp.getStrItem(j);
                objs[i].transform.FindChild("YesNo").GetComponent<Dropdown>().value = 1;
                i++;
            }
            for (int j = 0; j < 4; j++)
            {
                objs[i].transform.FindChild("Item").GetComponent<Text>().text = enep.name + ":" + enep.getStrItem(j);
                objs[i].transform.FindChild("YesNo").GetComponent<Dropdown>().value = 0;
                i++;
            }

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateCode()
        {
            string s = "";
            if (game.GameFactory.getGame().story.state == 9){       CreateCode1();  }
            else if (game.GameFactory.getGame().story.state == 10) { CreateCode2(); }
            else if (game.GameFactory.getGame().story.state == 11) { CreateCode2(); }
            else
            {
                Logger.info("Midnight.CreateCode():error state. state=" + game.GameFactory.getGame().story.state);
            }
            if (s != "")
            {
                game.GameFactory.getUnityManager().mainCamera.createCodeField.text = s;
            }
        }
        private string CreateCode1()
        {
            net.MidnightCode1 code = new net.MidnightCode1();
            code.item = itemListDropdown.value - 1;
            code.name = playerListDropdown.captionText.text;
            return game.GameFactory.getNetworkManager().createCode(code);
        }
        private string CreateCode2()
        {
            net.MidnightCode2 code = new net.MidnightCode2();
            if (objs[0].transform.FindChild("Item").GetComponent<Text>().text != "")
            {
                int i = 0;
                foreach (var o in objs)
                {
                    bool fyes = (o.transform.FindChild("YesNo").GetComponent<Dropdown>().value == 1);
                    string itemstr = objs[i].transform.FindChild("Item").GetComponent<Text>().text;
                    int item = game.Player.intItem(itemstr.Split(':')[1]);
                    if (fyes)
                    {
                        code.myItems.Add(item);
                    }
                    else
                    {
                        code.deadItems.Add(item);
                    }

                    i++;
                }
                if (code.deadItems.Count != 4) return "";
                if (code.myItems.Count != 4) return "";
            }

            return game.GameFactory.getNetworkManager().createCode(code);
        }

        public void SendCode()
        {
            string s = "";
            if (game.GameFactory.getGame().story.state == 9) { s=CreateCode1(); }
            else if (game.GameFactory.getGame().story.state == 10) { s = CreateCode2(); }
            else if (game.GameFactory.getGame().story.state == 11) { s = CreateCode2(); }
            else
            {
                Logger.info("Midnight.CreateCode():error state. state=" + game.GameFactory.getGame().story.state);
            }

            if (s != "")
            {
                game.GameFactory.getUnityManager().net.sendCodeAll(s);
            }
        }
    }
}

