using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace unity
{
    public class UnityManager
    {
        public network.NetworkUnity net = null;

        public main.MainUI mainui = null;
        public main.DebugRoom debug_room = null;

        public main.Noon noon = null;
        public main.Night night = null;
        public main.Midnight midnight = null;
        public title.MainCamera title = null;
        public main.Morning morning = null;

        public bool firstUpdate = true;

        public void updateDraw(bool first)
        {

            //firstUpdate = first;
            if (first)
            {
                moveScene();
            }else
            {
                updateSecne();
            }

            if (GameFactory.getGame().localData.fgm)
            {
                var code = GameFactory.getNetworkManager().createCodeGameSync(first);
                net.sendCode(code, RPCMode.Others);
            }

        }
        

        public void moveScene()
        {
            if (GameFactory.getGame().localData.debug_room)
            {
                SceneManager.LoadScene("debug_room");
                return;
            }
            switch (GameFactory.getGame().shareData.field.state)
            {
                case game.FIELD_STATE.EARLY_MORNING: SceneManager.LoadScene("early_morning"); break;
                case game.FIELD_STATE.MORNING: SceneManager.LoadScene("morning"); break;
                case game.FIELD_STATE.NOON: SceneManager.LoadScene("noon"); break;
                case game.FIELD_STATE.NIGHT: SceneManager.LoadScene("night"); break;
                case game.FIELD_STATE.MIDNIGHT: SceneManager.LoadScene("midnight"); break;
                default:
                    break;
            }
            
        }

        public void updateSecne()
        {
            if (GameFactory.getGame().localData.debug_room)
            {
                debug_room.updateDraw();
                return;
            }
            if(mainui!=null)mainui.updateDraw();
            switch (GameFactory.getGame().shareData.field.state)
            {
                case game.FIELD_STATE.EARLY_MORNING:  break;
                case game.FIELD_STATE.MORNING: morning.updateDraw(false); break;
                case game.FIELD_STATE.NOON: noon.updateDraw(false); break;
                case game.FIELD_STATE.NIGHT: night.updateDraw(false); break;
                case game.FIELD_STATE.MIDNIGHT: midnight.updateDraw(false); break;
                default:
                    break;
            }

        }


    }
}


