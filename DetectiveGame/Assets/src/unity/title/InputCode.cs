using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputCode : MonoBehaviour
{
    InputField input;

	// Use this for initialization
	void Start ()
    {
        input = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update () {


    }

    /// <summary>
    /// clickされたら
    /// </summary>
    public void OnClick()
    {
        string s = common.Crypt.dencode(input.text);
        Logger.info("input code=" + s);

        var args = s.Split(',');

        game.GameFactory.getGame().addPlayer(args[0]);
        game.GameFactory.getUnityManager().updateList();


        input.text = "";
    }


}
