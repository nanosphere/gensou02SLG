using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using game.db;

namespace game.net
{

    public class CreateStoryCode
    {
        public static NetworkData NoonRequest(int src , int dest)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = dest;
            dat.cmd = NET_COMMAND.NOON_REQUEST;

            return dat;
        }
        public static NetworkData NoonYes(int src)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = GameFactory.getGame().shareData.players.getPlayer(dat.src).net_opp;
            dat.fyes = true;
            dat.cmd = NET_COMMAND.NOON_ACK;

            return dat;
        }
        public static NetworkData NoonNo( int src )
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = GameFactory.getGame().shareData.players.getPlayer(dat.src).net_opp;
            dat.fyes = false;
            dat.cmd = NET_COMMAND.NOON_ACK;

            return dat;
        }
        public static NetworkData NoonItem(int src,int item)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = GameFactory.getGame().shareData.players.getPlayer(dat.src).net_opp;
            dat.item = item;
            dat.cmd = NET_COMMAND.NOON_ITEM;
            return dat;
        }
        public static NetworkData NoonEnd(int src)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.cmd = NET_COMMAND.NOON_END;
            return dat;
        }
        public static NetworkData NightVote(int src,int dest)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = dest;
            dat.cmd = NET_COMMAND.NIGHT_VOTE;
            return dat;
        }
        public static NetworkData NightYes(int src)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.fyes = true;
            dat.cmd = NET_COMMAND.NIGHT_ACK;
            return dat;
        }
        public static NetworkData NightNo(int src)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.fyes = false;
            dat.cmd = NET_COMMAND.NIGHT_ACK;
            return dat;
        }
        public static NetworkData MidnightSelect(int src,int dest, int item)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = dest;
            dat.item = item;
            dat.cmd = NET_COMMAND.MIDNIGHT_SELECT;
            return dat;
        }
        public static NetworkData MidnightSelectItem(int src,int dest, ITEM[] srcitem, ITEM[] destItem)
        {
            NetworkData dat = new NetworkData();
            dat.src = src;
            dat.dest = dest;
            for (int i = 0; i < srcitem.Length; i++)
            {
                dat.srcItem[i] = srcitem[i];
                dat.destItem[i] = destItem[i];
            }
            dat.cmd = NET_COMMAND.MIDNIGHT_SELECT_ITEM;
            return dat;
        }
    }

}

