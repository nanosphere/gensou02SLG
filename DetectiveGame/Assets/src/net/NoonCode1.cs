using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class NoonCode1
    {
        public int item;
        public string name;

        public void sync(NoonCode1 o)
        {
            item = o.item;
            name = o.name;
        }
    }
}

