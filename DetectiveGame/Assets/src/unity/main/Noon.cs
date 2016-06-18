using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class Noon : MonoBehaviour
    {
        List<GameObject> objs = new List<GameObject>();
        

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().noon = this;

            for(int i = 1; i <= 9; i++)
            {
                var obj = GameObject.Find("Canvas/Noon/NoonPlayer" + i);
                objs.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void create()
        {
            /*foreach(var o in objs)
            {
                Destroy(o);
            }
            objs.Clear();
            */

            //
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            List<string> items = myp.getStrItemList();
            
            //
            //var parentObject = GameObject.Find("Canvas/Noon/");
            //GameObject original = GameObject.Find("Canvas/Noon/NoonPlayer");
            int i = 0;
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                /*GameObject copied = Instantiate(original) as GameObject;
                copied.name = "NoonPlayer" + i;
                copied.transform.SetParent(parentObject.transform, false);
                copied.transform.position = new Vector3( 297 , 223 - 130 - i * 28, 0);
                //copied.transform.Translate(0, -100 - i * 18, 0);

                copied.transform.FindChild("Name").GetComponent<Text>().text = p.name;
                game.GameFactory.getUnityManager().createDropdown(copied.transform.FindChild("ItemList").GetComponent<Dropdown>(), items);
                
                objs.Add(copied);
                */

                objs[i].transform.FindChild("Name").GetComponent<Text>().text = p.name;
                game.GameFactory.getUnityManager().createDropdown(objs[i].transform.FindChild("ItemList").GetComponent<Dropdown>(), items, true);
                
                i++;
            }

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateNoonCode()
        {
            net.NoonCode noon = new net.NoonCode();
            foreach (var o in objs)
            {
                net.NoonCodeObj n = new net.NoonCodeObj();

                n.name = o.transform.FindChild("Name").GetComponent<Text>().text;
                Dropdown d = o.transform.FindChild("ItemList").GetComponent<Dropdown>();
                n.item = d.value-1;

                noon.players.Add(n);
            }

            var debugPlayerListDropdown = GameObject.Find("Canvas/DebugSelectPlayerList").GetComponent<Dropdown>();
            if (debugPlayerListDropdown == null)
            {
                GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>().text =
                game.GameFactory.getNetworkManager().createNoonCode(noon,"");
            }
            else
            {
                GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>().text =
                game.GameFactory.getNetworkManager().createNoonCode(noon, debugPlayerListDropdown.captionText.text);
            }
        }


    }
}

