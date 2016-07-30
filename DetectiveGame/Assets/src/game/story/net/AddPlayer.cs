using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story.net
{
    public class AddPlayer
    {

        public AddPlayer(string name)
        {
            Logger.info("story.AddPlayer():name="+name);
            GameFactory.getGame().shareData.players.addPlayer(name, ai.AI_MODE.NONE);
        }
    }
}
