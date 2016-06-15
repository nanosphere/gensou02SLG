using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCreateCode : MonoBehaviour {

    InputField field;
    Dropdown itemListDropdown;


    // Use this for initialization
    void Start () {
        field = GameObject.Find("Canvas/CreateCode/CodeField").GetComponent<InputField>();
        itemListDropdown = GameObject.Find("Canvas/CreateCode/ItemList").GetComponent<Dropdown>();

    }

    // Update is called once per frame
    void Update () {

        

    }

    /// <summary>
    /// clickされたら
    /// </summary>
    public void CreateGameCode()
    {
        string code = "";
        code += "g%";
        code += game.GameFactory.getGame().info.player_name + "%";
        code += common.JsonUtil.serialize(game.GameFactory.getGame());
        
        //---
        string str = common.Crypt.encode(code);
        field.text = str;

    }
    public void CreateUseItemCode()
    {
        //Logger.info("" + itemListDropdown.captionText.text);


        string code = "";
        code += "i%";
        code += game.GameFactory.getGame().info.player_name + "%";
        code += ""+itemListDropdown.value;

        //---
        string str = common.Crypt.encode(code);
        field.text = str;

    }

}
