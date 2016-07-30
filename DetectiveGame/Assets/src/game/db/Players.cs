using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.db
{
    public class Players
    {
        public List<Player> players = new List<Player>();

        public void sync(Players o)
        {
            players.Clear();
            foreach (var p2 in o.players)
            {
                Player p = new Player();
                p.sync(p2);
                players.Add(p);
            }
        }

        public void addPlayer(string name,ai.AI_MODE aimode)
        {
            if (isPlayer(name))
            {
                Logger.info("player is already. name="+name);
                return;
            }
            var p = new Player();
            p.ai = aimode;
            p.id = players.Count + 1;
            p.name = name;
            players.Add(p);

        }
        public bool isPlayer(int id)
        {
            foreach (var o in players)
            {
                if (o.id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public bool isPlayer(string name)
        {
            foreach (var o in players)
            {
                if (o.name ==name)
                {
                    return true;
                }
            }
            return false;
        }
        public Player getPlayer(int id)
        {
            foreach (var o in players)
            {
                if (o.id == id)
                {
                    return o;
                }
            }
            return null;
        }
        public Player getPlayer(string name)
        {
            foreach (var o in players)
            {
                if (o.name == name)
                {
                    return o;
                }
            }
            return null;
        }

        public void setAllState(PLAYER_STATE state)
        {
            // set
            foreach (var p in players)
            {
                if (p.fdead) continue;
                p.state = state;
            }
        }
        public bool isAllPlayerState(PLAYER_STATE state)
        {
            foreach (var p in players)
            {
                if (p.fdead) continue;
                if (p.state != state)
                    return false;
            }
            return true;
        }
        public bool isAllPlayerState(PLAYER_STATE state, PLAYER_STATE state2)
        {
            foreach (var p in players)
            {
                if (p.fdead) continue;
                if ( !(p.state == state || p.state == state2 ) )
                    return false;
            }
            return true;
        }


        // 狂気の殺人包丁使用者を返す
        public Player getUseMurdererKnife()
        {
            foreach (var p in players)
            {
                if (p.fdead) continue;
                if (p.dayUseItem == ITEM.MURDERE_KNIFE)
                {
                    return p;
                }
            }
            return null;
        }

        // 指定アイテムのプレイヤーを返す
        public List<Player> getUseItemPlayers(ITEM item)
        {
            List<Player> pl = new List<Player>();
            foreach (var p in players)
            {
                if (p.fdead) continue;
                if (p.dayUseItem == item)
                {
                    pl.Add(p);
                }
            }
            return pl;
        }


    }
}
