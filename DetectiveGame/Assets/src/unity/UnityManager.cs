using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace unity
{
    public class UnityManager
    {
        public void updateList()
        {
            string s = "";
            foreach( var p in game.GameFactory.getGame().players)
            {
                s += p.name + "\n";
            }
            GameObject.Find("Canvas/ListText").GetComponent<Text>().text = s;
        }
    }
}
