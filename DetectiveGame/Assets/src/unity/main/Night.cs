using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace unity.main
{
    public class Night : MonoBehaviour
    {
        Dropdown playerListDropdown;
        Dropdown YesNoBox;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().night = this;

            YesNoBox = GameObject.Find("Canvas/Night/YesNoBox").GetComponent<Dropdown>();
            playerListDropdown = GameObject.Find("Canvas/Night/NightPlayerList").GetComponent<Dropdown>();
            
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void updateDraw()
        {
            // player list
            List<string> items = new List<string>();
            items.Add("選択しない");
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                items.Add(p.name);
            }
            game.GameFactory.getUnityManager().createDropdown(playerListDropdown, items, true);

        }

        //=====================================
        // onClick
        //=====================================
        public void CreateCode()
        {
            if (game.GameFactory.getGame().story.state == 6)
            {
                CreateCode1();
            }
            else if (game.GameFactory.getGame().story.state == 7)
            {
                CreateCode2();
            }
            else
            {
                Logger.info("Night.CreateCode():error state. state="+ game.GameFactory.getGame().story.state);
            }

        }
        private void CreateCode1() {
            // 1
            net.NightCode1 code = new net.NightCode1();
            code.fyes = (YesNoBox.value == 1);
            game.GameFactory.getUnityManager().mainCamera.createCodeField.text = game.GameFactory.getNetworkManager().createCode(code);
        }
        private void CreateCode2() {
            // 2
            net.NightCode2 code = new net.NightCode2();
            code.name = playerListDropdown.captionText.text;
            if (code.name == "選択しない")
            {
                code.name = "";
            }
            game.GameFactory.getUnityManager().mainCamera.createCodeField.text = game.GameFactory.getNetworkManager().createCode(code);

        }
    }
}

