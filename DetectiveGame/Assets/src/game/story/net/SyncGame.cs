using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace game.story.net
{
    public class SyncGame
    {
        public SyncGame(ShareData o)
        {
            Logger.info("story.SyncGame():run.");

            GameFactory.getGame().shareData.sync(o);
            GameFactory.getGame().updateMessage();
            GameFactory.getUnityManager().updateDraw(GameFactory.getGame().shareData.fdrawNewScene);
        }
    }
}
