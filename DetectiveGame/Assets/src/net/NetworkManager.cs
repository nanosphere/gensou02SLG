

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
            return createCode("g", game.GameFactory.getGame());
        }
        public string createSelectCode(SelectCode select,string debugName)
        {
            if(debugName != "")
            {
                return debug_createCode("i", select, debugName);
            }
            return createCode("i", select);
        }
        public string createNoonCode(NoonCode select, string debugName)
        {
            if (debugName != "")
            {
                return debug_createCode("n", select, debugName);
            }
            return createCode("n",select);
        }
        public string createNightCode(NightCode select, string debugName)
        {
            if (debugName != "")
            {
                return debug_createCode("m", select, debugName);
            }
            return createCode("m", select);
        }
        public string createMidnightCode(MidnightCode select, string debugName)
        {
            if (debugName != "")
            {
                return debug_createCode("mid", select, debugName);
            }
            return createCode("mid", select);
        }
        private string createCode(string id,object obj)
        {
            string code = id + "%";
            code += game.GameFactory.getGame().info.player_name + "%";
            code += common.JsonUtil.serialize(obj);
            Logger.info("create code=" + code);
            return common.Crypt.encode(code);
        }
        private string debug_createCode(string id, object obj,string debugname)
        {
            string code = id + "%";
            code += debugname + "%";
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
                Logger.error("code error.");
                return;
            }

            var player = game.GameFactory.getGame().players.getPlayer(args[1]);
            if (player == null)
            {
                Logger.error("NetworkManager.setCode():player name unknown. name=" + args[1]);
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
                
                player.fnetWait = false;
                player.select = g;
            }
            else if (args[0] == "n")
            {
                NoonCode g = common.JsonUtil.deserialize<NoonCode>(args[2]);

                player.fnetWait = false;
                player.noon = g;

            }
            else if (args[0] == "m")
            {
                NightCode g = common.JsonUtil.deserialize<NightCode>(args[2]);

                player.fnetWait = false;
                player.night = g;

            }
            else if (args[0] == "mid")
            {
                MidnightCode g = common.JsonUtil.deserialize<MidnightCode>(args[2]);

                player.fnetWait = false;
                player.midnight = g;

            }


            //-------------------------------
            game.GameFactory.getUnityManager().update();


        }


    }
}