using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public class GameFactory
    {
        //-----------------------
        // 情報
        //-----------------------
        private static Game gm = null;
        private static unity.UnityManager um = null;

        public static Game getGame()
        {
            if (gm == null)
            {
                gm = new Game();
            }
            return gm;
        }
        public static unity.UnityManager getUnityManager()
        {
            if (um == null)
            {
                um = new unity.UnityManager();
            }
            return um;
        }
    }
}

