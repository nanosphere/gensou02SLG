using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateCode : MonoBehaviour {

    InputField field;
    InputField nameField;


    // Use this for initialization
    void Start () {
        field = GameObject.Find("Canvas/CodeField").GetComponent<InputField>();
        nameField = GameObject.Find("Canvas/NameField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update () {

        

    }

    /// <summary>
    /// clickされたら
    /// </summary>
    public void OnClick()
    {
        game.GameFactory.getGame().info.player_name = nameField.text;
        string s = nameField.text;

        string str = net.Coder.encode(s);
        field.text = str;

    }


}
