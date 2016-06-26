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
        public bool connected = false;
        public bool fserver = true;

        public common.MessageHistory messages = new common.MessageHistory(10,true,true);

        public NetworkView nView = null;


        // Use this for initialization
        void Start()
        {
            game.GameFactory.getUnityManager().net = this;
            nView = this.GetComponent<NetworkView>();
        }

        [RPC]
        public void recieveCode(string s)
        {
            game.GameFactory.getNetworkManager().setCode(s);
        }

        public void sendCodeAll(string s)
        {
            object[] args = new object[]
            {
                s
            };
            nView.RPC("recieveCode", RPCMode.All, args);
        }
        public void sendCodeOthers(string s)
        {
            object[] args = new object[]
            {
                s
            };
            nView.RPC("recieveCode", RPCMode.Others, args);
        }


        public void init()
        {
            if (connected)
            {
                return;
            }
            if (fserver)
            {
                messages.addMessage("サーバの作成開始");

                //サーバーを立てる
                // 接続可能人数,port,false
                Network.InitializeServer(10, int.Parse(port), false);
                Logger.info("NetworkManager.init():サーバを立てました。port=" + port);

            }
            else
            {
                messages.addMessage("サーバへの接続開始");
                Logger.info("NetworkManager.init():サーバへ接続開始。ip=" + ip + " port=" + port);

                // クライアントの接続
                Network.Connect(ip, int.Parse(port));
                
            }

        }


        //クライアント側で、クライアント接続時
        public void OnConnectedToServer()
        {
            connected = true;
            Logger.info("NetworkManager.init():client connect success.");

            messages.addMessage("サーバへの接続に成功しました");

        }

        // サーバ立ち上げ時に生成
        public void OnServerInitialized()
        {
            connected = true;
            Logger.info("NetworkManager.init():server success.");
            messages.addMessage("サーバの作成に成功しました");
        }

        // プレイヤーが接続されたとき、サーバー側で呼び出されます。
        public void OnPlayerConnected(NetworkPlayer player)
        {
            Logger.info("Connected from " + player.ipAddress + ":" + player.port);
            connected = true;
            messages.addMessage("クライアントからの接続に成功しました player="+player);
        }

        //プレイヤーが切断されたとき、サーバー側で呼び出されます。
        public void OnPlayerDisconnected(NetworkPlayer player)
        {
            Logger.info("Clean up after player " + player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
            messages.addMessage("クライアントが切断されました player="+ player);
        }

        //サーバーから切断したとき、クライアント側で呼び出されます。
        public void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            connected = false;
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
            messages.addMessage("サーバから切断されました");
        }

        //サーバーの接続に失敗したとき、クライアント側で呼び出されます。
        public void OnFailedToConnect(NetworkConnectionError error)
        {
            Logger.info("Could not connect to server: " + error);
            messages.addMessage("サーバへの接続に失敗しました error="+error);
        }
        

    }
}
