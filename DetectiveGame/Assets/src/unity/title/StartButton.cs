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
            SceneManager.LoadScene("game");

        }


    }
}
