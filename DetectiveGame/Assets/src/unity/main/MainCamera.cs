using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace unity.main
{
    public class MainCamera : MonoBehaviour
    {
        InputField inputCodeField;
        InputField createCodeField;
        Dropdown itemListDropdown;
        Dropdown playerListDropdown;

        Dropdown debugPlayerListDropdown=null;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().mainCamera = this;

            inputCodeField = GameObject.Find("Canvas/InputCode").GetComponent<InputField>();
            createCodeField = GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>();
            itemListDropdown = GameObject.Find("Canvas/CreateCode/ItemList").GetComponent<Dropdown>();
            playerListDropdown = GameObject.Find("Canvas/CreateCode/PlayerList").GetComponent<Dropdown>();
            
            GameObject.Find("Canvas/PlayerName").GetComponent<Text>().text = game.GameFactory.getGame().info.player_name;


            // debug
            debugPlayerListDropdown = GameObject.Find("Canvas/DebugSelectPlayerList").GetComponent<Dropdown>();
            game.GameFactory.getGame().players.addPlayer(game.GameFactory.getGame().info.player_name);
            game.GameFactory.getGame().players.addPlayer("p1");
            game.GameFactory.getGame().players.addPlayer("p2");
            game.GameFactory.getGame().players.addPlayer("p3");


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
        }
        public void CreateGameCode()
        {
            createCodeField.text = game.GameFactory.getNetworkManager().createGameCode();
        }
        public void CreateSelectCode()
        {
            net.SelectCode select = new net.SelectCode();
            select.item = itemListDropdown.value;
            select.selectName = playerListDropdown.captionText.text;

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

        //=====================================
        // state
        //=====================================
        public void updateMessage()
        {
            GameObject.Find("Canvas/TextState").GetComponent<Text>().text = 
                game.GameFactory.getGame().story.toState();

            GameObject.Find("Canvas/Message").GetComponent<Text>().text = game.GameFactory.getGame().message;

            updateItemList();
            updatePlayerList();
        }

        private void updateItemList()
        {
            game.Player p = game.GameFactory.getGame().players.getPlayer(game.GameFactory.getGame().info.player_name);

            itemListDropdown.options.Clear();
            itemListDropdown.options.Add(new Dropdown.OptionData { text = "何もなし" });
            foreach (var item in p.items)
            {
                itemListDropdown.options.Add(new Dropdown.OptionData { text = game.Player.strItem(item) });
            }
            itemListDropdown.value = 1;
            itemListDropdown.value = 0;
            
            GameObject.Find("Canvas/ItemList").GetComponent<Text>().text = p.toItems();

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
                s += "\n";
            }
            GameObject.Find("Canvas/PlayerList").GetComponent<Text>().text = s;


            playerListDropdown.options.Clear();
            debugPlayerListDropdown.options.Clear();
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                playerListDropdown.options.Add(new Dropdown.OptionData { text = p.name });
                debugPlayerListDropdown.options.Add(new Dropdown.OptionData { text = p.name });
            }
            playerListDropdown.value = 1;
            playerListDropdown.value = 0;
            debugPlayerListDropdown.value = 1;
            debugPlayerListDropdown.value = 0;

        }

    }
}

