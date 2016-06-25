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
            foreach (var p2 in o.players)
            {
                var p = getPlayer(p2.name);
                if (p != null)
                {
                    p.sync(p2);
                }
                else
                {
                    p = new Player();
                    p.sync(p2);
                    players.Add(p);
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
