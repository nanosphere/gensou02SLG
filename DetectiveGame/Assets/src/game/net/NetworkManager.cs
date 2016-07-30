

using System.Collections.Generic;

namespace game.net
{
    public class NetworkManager
    {

        public bool connected = false;
        public bool fserver = true;

        public common.MessageHistory messages = new common.MessageHistory(10);
        

        enum COMMAND
        {
            NONE,
            GAME_SYNC,
            ADD_PLAYER,
            SEND_DATA,
            SEND_MESSAGE,
        }
        
        class Header
        {
            public int src;
            public int dest;
            public COMMAND cmd;
        }

        public void addMessage(string s)
        {
            messages.addMessage(s);
            GameFactory.getUnityManager().updateDraw(false);
        }

        public string createCodeGameSync(bool first)
        {
            var s = GameFactory.getGame().shareData;
            s.fdrawNewScene = first;
            return createCode(COMMAND.GAME_SYNC, 0, s);
        }
        public string createCodeAddPlayer(string name)
        {
            return createCode(COMMAND.ADD_PLAYER, 0, name);
        }
        public string createCodeSendData(game.net.NetworkData data)
        {
            return createCode(COMMAND.SEND_DATA, 0,data);
        }
        public string createCodeSendMessage(int dest,string message)
        {
            return createCode(COMMAND.SEND_MESSAGE,dest, message);
        }
        
        private string createCode(COMMAND cmd,int dest,object o)
        {
            Header head = new Header();
            head.src = GameFactory.getGame().localData.myPlayer;
            head.dest = dest;
            head.cmd = cmd;
            
            //--- code
            string code = common.JsonUtil.serialize(head) + "%";
            code += common.JsonUtil.serialize(o);

            Logger.info("create code=" + code);
            return common.Crypt.encode(code);
        }
        


        public void setCode(string code)
        {
            if (code == "") return;
            string s = common.Crypt.dencode(code);
            Logger.info("input code=" + s);

            var args = s.Split('%');
            if (args.Length != 2)
            {
                Logger.error("NetworkManager.setCode():code error.");
                return;
            }
            var head = common.JsonUtil.deserialize<Header>(args[0]);

            //-------------------------------
            // not check
            //-------------------------------
            if (head.cmd == COMMAND.ADD_PLAYER)
            {
                var o = common.JsonUtil.deserialize<string>(args[1]);
                new game.story.net.AddPlayer(o);
                return;
            }
            else if (head.cmd == COMMAND.GAME_SYNC)
            {
                var g = common.JsonUtil.deserialize<game.ShareData>(args[1]);
                new game.story.net.SyncGame(g);
                return;
            }

            //-------------------------------
            // player check
            //-------------------------------
            var player = GameFactory.getGame().shareData.players.getPlayer(head.src);
            if (player == null)
            {
                Logger.error("NetworkManager.setCode():player name unknown. id=" + head.src);
                return;
            }

            //-------------------------------
            // set
            //-------------------------------
            if (head.cmd == COMMAND.SEND_DATA)
            {
                var g = common.JsonUtil.deserialize<game.net.NetworkData>(args[1]);
                new game.story.net.SendData(head.src,g);
                return;
            }

            if (head.cmd == COMMAND.SEND_MESSAGE)
            {
                var o = common.JsonUtil.deserialize<string>(args[1]);
                new game.story.net.SendMessage(head.src,head.dest,o);
                return;
            }
        }



        
    }
}