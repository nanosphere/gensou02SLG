using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class MidnightCode1
    {
        public int item = -1;
        public string name;

        public void sync(MidnightCode1 o)
        {
            item = o.item;
            name = o.name;
        }



    }

}

