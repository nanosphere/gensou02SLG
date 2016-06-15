using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextTurn : MonoBehaviour {

    Text itemList;
    Text state;

    Dropdown itemListDropdown;


    // Use this for initialization
    void Start ()
    {

        // debug
        game.GameFactory.getGame().addPlayer(game.GameFactory.getGame().info.player_name);
        game.GameFactory.getGame().addPlayer("p1");
        game.GameFactory.getGame().addPlayer("p2");
        game.GameFactory.getGame().addPlayer("p3");
        game.GameFactory.getGame().addPlayer("p4");

        

        game.GameFactory.getGame().init();

        itemList = GameObject.Find("Canvas/ItemList").GetComponent<Text>();
        itemListDropdown = GameObject.Find("Canvas/CreateCode/ItemList").GetComponent<Dropdown>();
        

        state = GameObject.Find("Canvas/TextState").GetComponent<Text>();
        state.text = game.GameFactory.getGame().toState();

        updateItemList();
    }

    // Update is called once per frame
    void Update () {

        

    }

    /// <summary>
    /// clickされたら
    /// </summary>
    public void OnClick()
    {
        game.GameFactory.getGame().nextTurn();

        state.text = game.GameFactory.getGame().toState();

        string s = "";
        foreach (var p in game.GameFactory.getGame().players)
        {
            s += p.name;
            if(p.state != 0)
            {
                s += " " + p.state;
            }
            s += "\n";
        }
        GameObject.Find("Canvas/PlayerList").GetComponent<Text>().text = s;
        GameObject.Find("Canvas/Message").GetComponent<Text>().text = game.GameFactory.getGame().message;

    }

    private void updateItemList()
    {
        game.Player p = game.GameFactory.getGame().getPlayer(game.GameFactory.getGame().info.player_name);

        itemListDropdown.options.Clear();
        itemListDropdown.options.Add(new Dropdown.OptionData { text = "何もなし" });
        foreach (var item in p.items)
        {
            itemListDropdown.options.Add(new Dropdown.OptionData { text = game.Player.strItem(item) });
        }
        itemListDropdown.value = 1;
        itemListDropdown.value = 0;


        itemList.text = p.toItems();

    }
}
