using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class NetworkLobbyManagerPrototype : NetworkLobbyManager
    {
        
        
        public override void OnLobbyClientEnter()
        {
            Debug.Log("Client enter lobby" +client.connection.connectionId);
        }

        public override void OnLobbyClientExit()
        {
            Debug.Log("Client exit lobby"+client.connection.connectionId);
        }

        public override void OnLobbyServerConnect(NetworkConnection conn)
        {
            Debug.Log("Server connect " + conn.connectionId);
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            Debug.Log("Server disconnect " + conn.connectionId);
        }
    }
}