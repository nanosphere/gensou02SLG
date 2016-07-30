using UnityEngine;
using System.Collections;
using UnityEngine.UI;




namespace unity.title
{
    public class MainCamera : MonoBehaviour
    {
        InputField nameField;
        Text listtext;

        // Use this for initialization
        void Start()
        {
            GameFactory.getUnityManager().title = this;
            
            nameField = GameObject.Find("Canvas/Network/NameField").GetComponent<InputField>();
            listtext = GameObject.Find("Canvas/ListText").GetComponent<Text>();
            listtext.text = "";

        }


        // Update is called once per frame
        void Update()
        {
            
            string s = "";
            foreach (var p in GameFactory.getGame().shareData.players.players)
            {
                s += p.name + "\n";
            }
            //s += "myplayer:"+ GameFactory.getGame().localData.myPlayer + "\n";
            listtext.text = s;

            GameObject.Find("Canvas/Network/NetworkLog").GetComponent<Text>().text = 
                GameFactory.getNetworkManager().messages.getMessage(true,true);
            
        }

        
        //------------------------------------
        public void clickServer()
        {
            var net = GameFactory.getUnityManager().net;
            net.port = GameObject.Find("Canvas/Network/Port").GetComponent<InputField>().text;
            GameFactory.getNetworkManager().fserver = true;
            GameFactory.getGame().localData.fgm = true;
            net.init();
        }
        public void clickConnect()
        {
            var net = GameFactory.getUnityManager().net;
            net.ip = GameObject.Find("Canvas/Network/IpAddress").GetComponent<InputField>().text;
            net.port = GameObject.Find("Canvas/Network/Port2").GetComponent<InputField>().text;
            GameFactory.getNetworkManager().fserver = false;
            GameFactory.getGame().localData.fgm = false;
            net.init();

            GameObject.Find("Canvas/com").SetActive(false);

        }

        public void OnClickStartButton()
        {
            if(GameFactory.getNetworkManager().fserver)
            {
                GameFactory.getGame().localData.fgm = true;
            }
            else
            {
                GameFactory.getGame().localData.fgm = false;
            }

            // add user
            string name = nameField.text;
            string code = GameFactory.getNetworkManager().createCodeAddPlayer(name);
            GameFactory.getUnityManager().net.sendCode(code, RPCMode.All);

            var myp = GameFactory.getGame().shareData.players.getPlayer(name);
            if (myp == null) return;
            GameFactory.getGame().localData.myPlayer = myp.id;
            
            if ( GameFactory.getGame().localData.fgm)
            {
                GameFactory.getGame().shareData.field.aiNum = GameObject.Find("Canvas/com").GetComponent<Dropdown>().value;
                new game.story.game1.InitGame();

                // シーン移動
                GameFactory.getUnityManager().updateDraw(true);
            }
            GameObject.Find("Canvas/StartButton").SetActive(false);
            

        }
    }
}