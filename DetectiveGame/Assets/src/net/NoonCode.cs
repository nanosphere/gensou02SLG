using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class NoonCodeObj
    {
        public int item;
        public string name;

    }
    public class NoonCode
    {
        public List<NoonCodeObj> players = new List<NoonCodeObj>();

        public void sync(NoonCode o)
        {
            players.Clear();
            foreach (var o2 in o.players)
            {
                NoonCodeObj obj = new NoonCodeObj();
                obj.item = o2.item;
                obj.name = o2.name;
                players.Add(obj);
            }
        }
    }
}

