using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace unity.title
{
    public class MainCamera : MonoBehaviour
    {
        InputField field;
        InputField nameField;


        // Use this for initialization
        void Start()
        {
            field = GameObject.Find("Canvas/CodeField").GetComponent<InputField>();
            nameField = GameObject.Find("Canvas/NameField").GetComponent<InputField>();
        }


        // Update is called once per frame
        void Update()
        {
            
        }



        /// <summary>
        /// clickされたら
        /// </summary>
        public void OnClick()
        {
            game.GameFactory.getGame().info.player_name = nameField.text;
            string s = nameField.text;

            string str = common.Crypt.encode(s);
            field.text = str;

        }


        /*
        /// <summary>
        /// clickされたら
        /// </summary>
        public void OnClick()
        {
            string s = common.Crypt.dencode(input.text);
            Logger.info("input code=" + s);

            var args = s.Split(',');

            game.GameFactory.getGame().addPlayer(args[0]);
            game.GameFactory.getUnityManager().updateList();


            input.text = "";
        }*/
    }
}