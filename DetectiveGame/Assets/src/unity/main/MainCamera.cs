using UnityEngine;
using System.Collections;
using UnityEngine.UI;




namespace unity.main
{
    public class MainCamera : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            if (GameFactory.getGame().localData.debug_room == false) {
                if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.Alpha5))
                {
                    Logger.info("debug room on");
                    GameFactory.getGame().localData.debug_room = true;
                    GameFactory.getUnityManager().updateDraw(true);
                }
            } else {
                if (Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Alpha4) && Input.GetKey(KeyCode.Alpha6))
                {
                    Logger.info("debug room off");
                    GameFactory.getGame().localData.debug_room = false;
                    GameFactory.getUnityManager().updateDraw(true);
                }
            }
        }

        
    }
}