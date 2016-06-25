using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace net
{
    public class MidnightCode2
    {
        public List<int> myItems = new List<int>();
        public List<int> deadItems = new List<int>();
        
        public void sync(MidnightCode2 o)
        {
            myItems.Clear();
            foreach (var o2 in o.myItems)
            {
                myItems.Add(o2);
            }
            deadItems.Clear();
            foreach (var o2 in o.deadItems)
            {
                deadItems.Add(o2);
            }
        }
    }

}

