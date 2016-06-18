using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game
{
    public class Players
    {
        public List<Player> players = new List<Player>();

        public void sync(Players o)
        {
            foreach (var p in o.players)
            {
                if (isPlayer(p.name))
                {
                    p.sync(p);
                }
                else
                {
                    Player p2 = new Player();
                    p2.sync(p);
                    players.Add(p2);
                }
            }

        }

        public void addPlayer(string name)
        {
            if (isPlayer(name))
            {
                Logger.info("player is already.");
                return;
            }
            var p = new Player();
            p.name = name;

            players.Add(p);

        }

        public bool isPlayer(string name)
        {
            foreach (var o in players)
            {
                if (o.name == name)
                {
                    return true;
                }
            }
            return false;
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

        public Player getMyPlayer()
        {
            return getPlayer(GameFactory.getGame().info.player_name);
        }
    }
}
