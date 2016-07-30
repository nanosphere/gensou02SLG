using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.net;

namespace game.story.net
{
    public class SendMessage
    { 
        public SendMessage(int srcPlayer , int destPlayer, string message)
        {
            Logger.info("story.SendMessage():run.");

            if (GameFactory.getGame().localData.myPlayer == destPlayer)
            {
                var p = GameFactory.getGame().shareData.players.getPlayer(srcPlayer);
                string s = "";
                s += "[個別メッセ]"+p.name + ":" + message;
                GameFactory.getNetworkManager().messages.addMessage(s);
                GameFactory.getUnityManager().updateDraw(false);
            }
            if (GameFactory.getGame().localData.myPlayer == srcPlayer)
            {
                var p = GameFactory.getGame().shareData.players.getPlayer(srcPlayer);
                string s = "";
                s += "[個別メッセ]" + p.name + ":" + message;
                GameFactory.getNetworkManager().messages.addMessage(s);
                GameFactory.getUnityManager().updateDraw(false);
            }
        }
        
    }
}
