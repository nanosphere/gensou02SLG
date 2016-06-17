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
        private static net.NetworkManager nm = null;

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
        public static net.NetworkManager getNetworkManager()
        {
            if (nm == null)
            {
                nm = new net.NetworkManager();
            }
            return nm;
        }
    }
}

