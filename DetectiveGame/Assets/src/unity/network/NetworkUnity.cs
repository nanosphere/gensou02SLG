using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace unity.network
{
    public class NetworkUnity : MonoBehaviour
    {

        public string ip = "127.0.0.1";
        public string port = "25565";
        
        public NetworkView nView = null;

        void Awake()
        {
            GameFactory.getUnityManager().net = this;
            nView = this.GetComponent<NetworkView>();
            
        }
        // Use this for initialization
        void Start()
        {
            
        }

        [RPC]
        public void recieveCode(string s1, string s2, string s3, string s4, string s5)
        {
            Logger.info("NetworkUnity.recieveCode()");
            string s = s1 + s2 + s3 + s4 + s5;
            GameFactory.getNetworkManager().setCode(s);
        }

        public void sendCode(string s, RPCMode mode)
        {
            if (!GameFactory.getNetworkManager().connected) return;

            if (mode == RPCMode.Server && GameFactory.getNetworkManager().fserver)
            {
                GameFactory.getNetworkManager().setCode(s);
                return;
            }

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            string s5 = "";
            if (s.Length < 4000)
            {
                s1 = s;
            }
            else if (s.Length < 8000)
            {
                s1 = s.Substring(0, 4000);
                s2 = s.Substring(4000);
            }
            else if (s.Length < 12000)
            {
                s1 = s.Substring(0, 4000);
                s2 = s.Substring(4000, 4000);
                s3 = s.Substring(8000);
            }
            else if (s.Length < 16000)
            {
                s1 = s.Substring(0, 4000);
                s2 = s.Substring(4000, 4000);
                s3 = s.Substring(8000, 4000);
                s4 = s.Substring(12000);
            }
            else if (s.Length < 20000)
            {
                s1 = s.Substring(0, 4000);
                s2 = s.Substring(4000, 4000);
                s3 = s.Substring(8000, 4000);
                s4 = s.Substring(12000, 4000);
                s5 = s.Substring(16000);
            }
            else
            {
                Logger.error("NetworkUnity.sencCode():string is large 20000 over. len=" + s.Length);
            }


            object[] args = new object[]
                {
                s1,s2,s3,s4,s5
                };
            Logger.info("NetworkUnity.sendCode()");
            nView.RPC("recieveCode", mode, args);
            
        }

        public void init()
        {
            if (GameFactory.getNetworkManager().connected)
            {
                return;
            }
            if (GameFactory.getNetworkManager().fserver)
            {
                GameFactory.getNetworkManager().addMessage("サーバの作成開始");

                //サーバーを立てる
                // 接続可能人数,port,false
                Network.InitializeServer(10, int.Parse(port), false);
                Logger.info("NetworkManager.init():サーバを立てました。port=" + port);

            }
            else
            {
                GameFactory.getNetworkManager().addMessage("サーバへの接続開始");
                Logger.info("NetworkManager.init():サーバへ接続開始。ip=" + ip + " port=" + port);

                // クライアントの接続
                Network.Connect(ip, int.Parse(port));
                
            }

        }


        //クライアント側で、クライアント接続時
        public void OnConnectedToServer()
        {
            GameFactory.getNetworkManager().connected = true;
            Logger.info("NetworkManager.init():client connect success.");

            GameFactory.getNetworkManager().addMessage("サーバへの接続に成功しました");

        }

        // サーバ立ち上げ時に生成
        public void OnServerInitialized()
        {
            GameFactory.getNetworkManager().connected = true;
            Logger.info("NetworkManager.init():server success.");
            GameFactory.getNetworkManager().addMessage("サーバの作成に成功しました");
        }

        // プレイヤーが接続されたとき、サーバー側で呼び出されます。
        public void OnPlayerConnected(NetworkPlayer player)
        {
            Logger.info("Connected from " + player.ipAddress + ":" + player.port);
            GameFactory.getNetworkManager().connected = true;
            GameFactory.getNetworkManager().addMessage("クライアントからの接続に成功しました player="+player);
        }

        //プレイヤーが切断されたとき、サーバー側で呼び出されます。
        public void OnPlayerDisconnected(NetworkPlayer player)
        {
            Logger.info("Clean up after player " + player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
            GameFactory.getNetworkManager().addMessage("クライアントが切断されました player="+ player);
        }

        //サーバーから切断したとき、クライアント側で呼び出されます。
        public void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            GameFactory.getNetworkManager().connected = false;
            if (Network.isServer)
            {
                Logger.info("Local server connection disconnected");
            }
            else if (info == NetworkDisconnection.LostConnection)
            {
                Logger.info("Lost connection to the server");
            }
            else
            {
                Logger.info("Successfully diconnected from the server");
            }
            GameFactory.getNetworkManager().addMessage("サーバから切断されました");
        }

        //サーバーの接続に失敗したとき、クライアント側で呼び出されます。
        public void OnFailedToConnect(NetworkConnectionError error)
        {
            Logger.info("Could not connect to server: " + error);
            GameFactory.getNetworkManager().addMessage("サーバへの接続に失敗しました error="+error);
        }
        

    }
}
