using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameFactory
{
    //-----------------------
    // 情報
    //-----------------------
    private static game.Game gm = null;
    private static unity.UnityManager um = null;
    private static game.net.NetworkManager nm = null;
    private static game.ai.AiManager am = null;
    public static bool debug = false;

    public static void initFacroty()
    {
        gm = null;
        um = null;
        nm = null;
        am = null;
    }
    public static game.Game getGame()
    {
        if (gm == null)
        {
            gm = new game.Game();
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
    public static game.net.NetworkManager getNetworkManager()
    {
        if (nm == null)
        {
            nm = new game.net.NetworkManager();
        }
        return nm;
    }
    public static game.ai.AiManager getAiManager()
    {
        if (am == null)
        {
            am = new game.ai.AiManager();
        }
        return am;
    }
}


