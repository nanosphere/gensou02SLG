using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class MainCamera : MonoBehaviour
    {
        InputField inputCodeField;
        public InputField createCodeField;


        Dropdown debugPlayerListDropdown =null;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().mainCamera = this;
            
            inputCodeField = GameObject.Find("Canvas/Code/InputCode").GetComponent<InputField>();
            createCodeField = GameObject.Find("Canvas/Code/CodeField").GetComponent<InputField>();
           

            // debug
            //debugPlayerListDropdown = GameObject.Find("Canvas/DebugSelectPlayerList").GetComponent<Dropdown>();
            //game.GameFactory.getGame().players.addPlayer(game.GameFactory.getGame().info.player_name);
            //game.GameFactory.getGame().players.addPlayer("p1");
            //game.GameFactory.getGame().players.addPlayer("p2");
            //game.GameFactory.getGame().players.addPlayer("p3");


            // init
            game.GameFactory.getGame().init();
            updateDraw();
            

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
            updateDraw();
        }
        public void CreateGameCode()
        {
            createCodeField.text = game.GameFactory.getNetworkManager().createGameCode();
            updateDraw();
        }

        public void onClickNextTurn()
        {
            if (!game.GameFactory.getGame().info.fhost) return;
            game.GameFactory.getGame().story.nextTurn();
            updateDraw();
        }

        //=====================================
        // state
        //=====================================
        public void updateDraw()
        {
            GameObject.Find("Canvas/GameInfo/PlayerName").GetComponent<Text>().text = game.GameFactory.getGame().info.player_name;
            GameObject.Find("Canvas/GameInfo/TextState").GetComponent<Text>().text = game.GameFactory.getGame().story.toState();
            updateItemList();
            updatePlayerList();
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            GameObject.Find("Canvas/GameInfo/Message").GetComponent<Text>().text = game.GameFactory.getGame().message;
            GameObject.Find("Canvas/GameInfo/Message2").GetComponent<Text>().text = myp.message;
            GameObject.Find("Canvas/GameInfo/MidnightMessage").GetComponent<Text>().text = game.GameFactory.getGame().midnightMessage;

            
        }

        private void updateItemList()
        {
            game.Player myp = game.GameFactory.getGame().players.getMyPlayer();
            string s = "";
            for (int i=0;i<myp.items.Length;i++)
            {
                s += myp.getStrItem(i) + "\n";
            }
            GameObject.Find("Canvas/GameInfo/ItemList").GetComponent<Text>().text = s;
            
        }
        private void updatePlayerList()
        {
            //深夜は更新しない
            if (game.GameFactory.getGame().story.state > 9)
            {
                return;
            }

            string s = "";
            foreach (var p in game.GameFactory.getGame().players.players)
            {

                s += p.name;
                if (!p.fnetWait)
                {
                    s += " selected";
                }

                if (p.fdead)
                {
                    s += " dead";
                }
                
                
                if (game.GameFactory.getGame().captivityName == p.name)
                {
                    s += " 監禁中";
                }
                s += "\n";
            }
            GameObject.Find("Canvas/GameInfo/PlayerList").GetComponent<Text>().text = s;
            
        }

    }
}

