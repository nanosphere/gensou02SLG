﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace net
{
    public class SelectCode
    {
        public int item;
        public string name;

        public void sync(SelectCode o)
        {
            item = o.item;
            name = o.name;
        }
    }
}

