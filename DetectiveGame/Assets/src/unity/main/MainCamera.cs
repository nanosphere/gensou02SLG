using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class MainCamera : MonoBehaviour
    {
        InputField inputCodeField;
        InputField createCodeField;
        Dropdown itemListDropdown;
        Dropdown playerListDropdown;
        Dropdown nightPlayerListDropdown;
        Dropdown YesNoBox;


        Dropdown debugPlayerListDropdown =null;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().mainCamera = this;

            inputCodeField = GameObject.Find("Canvas/InputCode").GetComponent<InputField>();
            createCodeField = GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>();
            itemListDropdown = GameObject.Find("Canvas/CreateCode/ItemList").GetComponent<Dropdown>();
            playerListDropdown = GameObject.Find("Canvas/CreateCode/PlayerList").GetComponent<Dropdown>();
            nightPlayerListDropdown = GameObject.Find("Canvas/NightPlayerList").GetComponent<Dropdown>();
            YesNoBox = GameObject.Find("Canvas/YesNoBox").GetComponent<Dropdown>();

            GameObject.Find("Canvas/PlayerName").GetComponent<Text>().text = game.GameFactory.getGame().info.player_name;
            
            // debug
            debugPlayerListDropdown = GameObject.Find("Canvas/DebugSelectPlayerList").GetComponent<Dropdown>();
            //game.GameFactory.getGame().players.addPlayer(game.GameFactory.getGame().info.player_name);
            //game.GameFactory.getGame().players.addPlayer("p1");
            //game.GameFactory.getGame().players.addPlayer("p2");
            //game.GameFactory.getGame().players.addPlayer("p3");


            // init
            game.GameFactory.getGame().init();
            updateMessage();
            

        }

        // Update is called once per frame
        void Update()
        {
            // update game
            game.GameFactory.getGame().update();
        }


        //=====================================
        // onClick
        //=====================================
        public void InputCode()
        {
            game.GameFactory.getNetworkManager().setCode(inputCodeField.text);
            inputCodeField.text = "";
        }
        public void CreateGameCode()
        {
            createCodeField.text = game.GameFactory.getNetworkManager().createGameCode();
        }
        public void CreateSelectCode()
        {
            net.SelectCode select = new net.SelectCode();
            select.item = itemListDropdown.value - 1;
            select.name = playerListDropdown.captionText.text;

            if (debugPlayerListDropdown == null)
            {
                createCodeField.text = game.GameFactory.getNetworkManager().createSelectCode(select, "");
            }else
            {
                createCodeField.text = game.GameFactory.getNetworkManager().createSelectCode(select, debugPlayerListDropdown.captionText.text);
            }
        }
        public void onClickNextTurn()
        {
            
            game.GameFactory.getGame().story.nextTurn();
            updateMessage();
        }
        public void CreateNightCode()
        {
            net.NightCode code = new net.NightCode();
            code.fyes = (YesNoBox.value == 1);
            code.name = nightPlayerListDropdown.captionText.text;


            if (debugPlayerListDropdown == null)
            {
                createCodeField.text = game.GameFactory.getNetworkManager().createNightCode(code, "");
            }
            else
            {
                createCodeField.text = game.GameFactory.getNetworkManager().createNightCode(code, debugPlayerListDropdown.captionText.text);
            }
        }

        //=====================================
        // state
        //=====================================
        public void updateMessage()
        {
            GameObject.Find("Canvas/TextState").GetComponent<Text>().text = 
                game.GameFactory.getGame().story.toState();

            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            GameObject.Find("Canvas/Message").GetComponent<Text>().text = game.GameFactory.getGame().message;
            GameObject.Find("Canvas/Message2").GetComponent<Text>().text = myp.message;

            
            game.GameFactory.getUnityManager().midnight.updateMessage();

            updateItemList();
            updatePlayerList();
        }

        private void updateItemList()
        {
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            List<string> items = myp.getStrItemList();
            
            game.GameFactory.getUnityManager().createDropdown(itemListDropdown,items,true);
            GameObject.Find("Canvas/ItemList").GetComponent<Text>().text = myp.toItems();

        }
        private void updatePlayerList()
        {
            string s = "";
            foreach (var p in game.GameFactory.getGame().players.players)
            {

                s += p.name;
                if (p.fdead)
                {
                    s += " dead";
                }
                if (!p.fnetWait)
                {
                    s += " selected";
                }
                s += "\n";
            }
            GameObject.Find("Canvas/PlayerList").GetComponent<Text>().text = s;

            List<string> items = new List<string>();
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                items.Add(p.name);
            }
            
            game.GameFactory.getUnityManager().createDropdown(nightPlayerListDropdown, items, true);
            game.GameFactory.getUnityManager().createDropdown(playerListDropdown, items, true);
            game.GameFactory.getUnityManager().createDropdown(debugPlayerListDropdown, items, true);
            

        }

    }
}

