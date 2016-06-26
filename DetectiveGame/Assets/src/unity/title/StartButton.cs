using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace unity.title
{
    public class StartButton : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GameObject.Find("Canvas/ListText").GetComponent<Text>().text = "";

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
            game.GameFactory.getGame().info.player_name = GameObject.Find("Canvas/Network/NameField").GetComponent<InputField>().text;
            game.GameFactory.getGame().info.fhost = (GameObject.Find("Canvas/GMBox").GetComponent<Dropdown>().value == 1);

            var p = game.GameFactory.getGame().players.getMyPlayer();
            if (p == null) return;
            
            SceneManager.LoadScene("game");

        }


    }
}
