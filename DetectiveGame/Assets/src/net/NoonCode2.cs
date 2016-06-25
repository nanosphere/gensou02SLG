using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class NoonCode2Obj
    {
        public int item;
        public string name;
        public bool fyes;
    }
    public class NoonCode2
    {
        public List<NoonCode2Obj> players = new List<NoonCode2Obj>();

        public void sync(NoonCode2 o)
        {
            players.Clear();
            foreach (var o2 in o.players)
            {
                NoonCode2Obj obj = new NoonCode2Obj();
                obj.item = o2.item;
                obj.name = o2.name;
                obj.fyes = o2.fyes;
                players.Add(obj);
            }
        }
    }
}

