using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class MidnightCodeObj
    {
        public bool fyes;

    }
    public class MidnightCode
    {
        public List<MidnightCodeObj> items = new List<MidnightCodeObj>();

        public void sync(MidnightCode o)
        {
            items.Clear();
            foreach (var o2 in o.items)
            {
                MidnightCodeObj obj = new MidnightCodeObj();
                obj.fyes = o2.fyes;
                items.Add(obj);
            }
        }
    }

}

