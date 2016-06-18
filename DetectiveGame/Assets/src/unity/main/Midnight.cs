using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class Midnight : MonoBehaviour
    {
        List<GameObject> objs = new List<GameObject>();
        

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().midnight = this;

            for(int i = 1; i <= 4; i++)
            {
                var obj = GameObject.Find("Canvas/Midnight/NightItem" + i);
                objs.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void create(game.Player player)
        {
            List<string> items = player.getStrItemList();

            int i = 0;
            foreach (var p in items)
            {
                objs[i].transform.FindChild("Item").GetComponent<Text>().text = p;
                i++;
            }
        }


        //=====================================
        // state
        //=====================================
        public void updateMessage()
        {
            
            var myp = game.GameFactory.getGame().players.getMyPlayer();
            var p = game.GameFactory.getGame().players.getPlayer(myp.killName);
            if (p == null)
            {
                foreach (var o in objs)
                {
                    o.transform.FindChild("Item").GetComponent<Text>().text = "";
                }
            }else
            {
                int i = 0;
                foreach (var o in objs)
                {
                    o.transform.FindChild("Item").GetComponent<Text>().text = p.getStrItem(i);
                    i++;
                }
            }

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateMidnightCode()
        {
            net.MidnightCode code = new net.MidnightCode();
            int i = 0;
            foreach (var o in objs)
            {
                net.MidnightCodeObj n = new net.MidnightCodeObj();
                
                Dropdown d = o.transform.FindChild("YesNo").GetComponent<Dropdown>();
                n.fyes = ( d.value == 1 );

                code.items.Add(n);
                i++;
            }

            var debugPlayerListDropdown = GameObject.Find("Canvas/DebugSelectPlayerList").GetComponent<Dropdown>();
            if (debugPlayerListDropdown == null)
            {
                GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>().text =
                    game.GameFactory.getNetworkManager().createMidnightCode(code,"");
            }
            else
            {
                GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>().text =
                    game.GameFactory.getNetworkManager().createMidnightCode(code, debugPlayerListDropdown.captionText.text);
            }
        }


    }
}

