

using System.Collections.Generic;

namespace net
{
    public class NetworkCodeManager
    {
        

        public void askPlayers()
        {
            // set
            foreach(var p in game.GameFactory.getGame().players.players)
            {
                p.fnetWait = true;
            }
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
            return createCode(game.GameFactory.getGame().info.player_name, "g", game.GameFactory.getGame());
        }
        public string createCode(object code)
        {
            return createCode(game.GameFactory.getGame().info.player_name , getId(code), code);
        }
        public string debug_createCode(object code, string debugName)
        {
            return createCode(debugName, getId(code), code);
        }
        private string createCode(string name , string id, object obj)
        {
            string code = id + "%";
            code += name + "%";
            code += common.JsonUtil.serialize(obj);
            Logger.info("create code=" + code);
            return common.Crypt.encode(code);
        }


        public void setCode(string code)
        {
            if (code == "") return;
            string s = common.Crypt.dencode(code);
            Logger.info("input code=" + s);

            var args = s.Split('%');
            if (args.Length != 4)
            {
                Logger.error("NetworkManager.setCode():code error.");
                return;
            }

            //-------------------------------
            // player
            //-------------------------------
            if (args[0] == "p")
            {
                var o = common.JsonUtil.deserialize<AddPlayerCode>(args[2]);
                game.GameFactory.getGame().players.addPlayer(o.name);

                game.GameFactory.getUnityManager().update();
                return;
            }
            else if (args[0] == "g")
            {
                game.Game g = common.JsonUtil.deserialize<game.Game>(args[2]);
                game.GameFactory.getGame().sync(g);

                game.GameFactory.getUnityManager().update();
                return;
            }

            //-------------------------------
            // player check
            //-------------------------------
            var player = game.GameFactory.getGame().players.getPlayer(args[1]);
            if (player == null)
            {
                Logger.error("NetworkManager.setCode():player name unknown. name=" + args[1]);
                return;
            }

            //-------------------------------
            // set
            //-------------------------------
            if (!newCodeDeserialize(player, args[0], args[2]))
            {
                Logger.error("NetworkManager.setCode():id is not found.");
                return;
            }
            player.fnetWait = false;
            Logger.info("z");

            //-------------------------------
            game.GameFactory.getUnityManager().update();


        }

        private bool newCodeDeserialize(game.Player player, string id, string code)
        {
            if (id == "1") {      player.noon1 = common.JsonUtil.deserialize<NoonCode1>(code); }
            else if (id == "2") { player.noon2 = common.JsonUtil.deserialize<NoonCode2>(code); }
            else if (id == "3") { player.night1 = common.JsonUtil.deserialize<NightCode1>(code); }
            else if (id == "4") { player.night2 = common.JsonUtil.deserialize<NightCode2>(code); }
            else if (id == "5") { player.midnight1 = common.JsonUtil.deserialize<MidnightCode1>(code); }
            else if (id == "6") { player.midnight2 = common.JsonUtil.deserialize<MidnightCode2>(code); }
            else if (id == "7") { player.midnight3 = common.JsonUtil.deserialize<MidnightCode3>(code); }
            else
            {
                return false;
            }
            return true;
        }
        private string getId(object code)
        {
            if (code is AddPlayerCode) return "p";
            else if (code is NoonCode1) return "1";
            else if (code is NoonCode2) return "2";
            else if (code is NightCode1) return "3";
            else if (code is NightCode2) return "4";
            else if (code is MidnightCode1) return "5";
            else if (code is MidnightCode2) return "6";
            else if (code is MidnightCode3) return "7";
            

            return "";
        }
    }
}