using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class NightCode
    {
        public bool fyes;
        public string name;

        public void sync(NightCode o)
        {
            fyes = o.fyes;
            name = o.name;
        }
    }

}

