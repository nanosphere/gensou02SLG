using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace unity.title
{
    public class MainCamera : MonoBehaviour
    {
        InputField field;
        InputField nameField;
        InputField inputCodeField;
        Text listtext;

        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().title = this;

            field = GameObject.Find("Canvas/CodeField").GetComponent<InputField>();
            nameField = GameObject.Find("Canvas/Network/NameField").GetComponent<InputField>();
            inputCodeField = GameObject.Find("Canvas/InputCode").GetComponent<InputField>();
            listtext = GameObject.Find("Canvas/ListText").GetComponent<Text>();

        }


        // Update is called once per frame
        void Update()
        {
            string s = "";
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                s += p.name + "\n";
            }
            listtext.text = s;

            var net = game.GameFactory.getUnityManager().net;
            GameObject.Find("Canvas/Network/NetworkLog").GetComponent<Text>().text = net.messages.getMessage();

        }



        public void CreateGameCode()
        {
            var o = new net.AddPlayerCode();
            o.name = nameField.text;
            field.text = game.GameFactory.getNetworkManager().createCode(o);
        }
        public void InputCode()
        {
            game.GameFactory.getNetworkManager().setCode(inputCodeField.text);
            inputCodeField.text = "";
        }

        //------------------------------------
        public void clickServer()
        {
            var net = game.GameFactory.getUnityManager().net;
            net.port = GameObject.Find("Canvas/Network/Port").GetComponent<InputField>().text;
            net.fserver = true;
            net.init();
        }
        public void clickConnect()
        {
            var net = game.GameFactory.getUnityManager().net;
            net.ip = GameObject.Find("Canvas/Network/IpAddress").GetComponent<InputField>().text;
            net.port = GameObject.Find("Canvas/Network/Port2").GetComponent<InputField>().text;
            net.fserver = false;
            net.init();
        }
        public void addPlayerSend()
        {

            var o = new net.AddPlayerCode();
            o.name = nameField.text;
            game.GameFactory.getUnityManager().net.sendCodeAll( game.GameFactory.getNetworkManager().createCode(o));

        }


    }
}