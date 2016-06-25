using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class NightCode2
    {
        public string name;

        public void sync(NightCode2 o)
        {
            name = o.name;
        }
    }

}

