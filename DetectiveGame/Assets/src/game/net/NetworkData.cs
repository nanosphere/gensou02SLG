using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace game.net
{
    public enum NET_COMMAND
    {
        NONE,
        NOON_REQUEST,
        NOON_ACK,
        NOON_ITEM,
        NOON_END,
        MIDNIGHT_SELECT,
        MIDNIGHT_SELECT_ITEM,
        NIGHT_ACK,
        NIGHT_VOTE,
    }

    public class NetworkData
    {
        public NET_COMMAND cmd = NET_COMMAND.NONE;
        public int src;
        public int dest;

        public int item = -1;
        public bool fyes;

        public db.ITEM[] srcItem = new db.ITEM[8];
        public db.ITEM[] destItem = new db.ITEM[8];

        public void sync(NetworkData o)
        {
            cmd = o.cmd;
            src = o.src;
            dest = o.dest;
            item = o.item;
            fyes = o.fyes;
            for(int i = 0; i < srcItem.Length; i++)
            {
                srcItem[i] = o.srcItem[i];
                destItem[i] = o.destItem[i];
            }

        }
    }

}

