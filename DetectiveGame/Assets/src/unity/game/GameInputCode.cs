using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInputCode : MonoBehaviour
{
    InputField input;

	// Use this for initialization
	void Start ()
    {
        input = GetComponent<InputField>();

        GameObject.Find("Canvas/PlayerName").GetComponent<Text>().text = game.GameFactory.getGame().info.player_name;

    }

    // Update is called once per frame
    void Update () {


    }

    /// <summary>
    /// clickされたら
    /// </summary>
    public void OnClick()
    {
        string s = net.Coder.dencode(input.text);
        Logger.info("input code=" + s);

        var args = s.Split('%');
        if(args.Length != 4)
        {
            Logger.info("code error.");
            return;
        }

        //-------------------------------
        //結果

        if (args[0] == "g")
        {
            game.Game g = common.JsonUtil.deserialize<game.Game>(args[2]);
            game.GameFactory.getGame().sync(g);
        }
        else if(args[0] == "i")
        {
            string name = args[1];
            int select = int.Parse(args[2]);

            game.GameFactory.getGame().getPlayer(name).action = select;
        }

        //-------------------------------
        input.text = "";
        game.GameFactory.getGame().onUpdate();
    }


}
