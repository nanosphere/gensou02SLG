using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace net
{
    public class NetworkManager
    {
        public void askPlayers()
        {
            // set
            foreach(var p in game.GameFactory.getGame().players.players)
            {
                p.fnetWait = true;
            }
            //送信
            game.GameFactory.getUnityManager().mainCamera.CreateGameCode();

        }
        public bool isPlayerAllAck()
        {
            foreach (var p in game.GameFactory.getGame().players.players)
            {
                if (p.fnetWait)
                {
                    return false;
                }
            }
            return true;
        }

        public string createGameCode()
        {
            string code = "";
            code += "g%";
            code += game.GameFactory.getGame().info.player_name + "%";
            code += common.JsonUtil.serialize(game.GameFactory.getGame());
            Logger.info("create code=" + code);
            string str = common.Crypt.encode(code);
            return str;
        }
        public string createSelectCode(SelectCode select,string debugName)
        {
            string code = "";
            code += "i%";
            if (debugName == "")
            {
                code += game.GameFactory.getGame().info.player_name + "%";
            }else
            {
                code += debugName + "%";
            }
            code += common.JsonUtil.serialize(select);
            Logger.info("create code=" + code);
            string str = common.Crypt.encode(code);
            return str;

        }

        
        public void setCode(string code)
        {
            if (code == "") return;
            string s = common.Crypt.dencode(code);
            Logger.info("input code=" + s);

            var args = s.Split('%');
            if (args.Length < 3)
            {
                Logger.info("code error.");
                return;
            }

            //-------------------------------
            //結果
            //-------------------------------
            if (args[0] == "g")
            {
                game.Game g = common.JsonUtil.deserialize<game.Game>(args[2]);
                game.GameFactory.getGame().sync(g);
            }
            else if (args[0] == "i")
            {
                SelectCode g = common.JsonUtil.deserialize<SelectCode>(args[2]);

                var player = game.GameFactory.getGame().players.getPlayer(args[1]);
                player.selectItem = g.item;
                player.selectName = g.selectName;
                player.fnetWait = false;
            }

            //-------------------------------
            game.GameFactory.getUnityManager().update();


        }


    }
}