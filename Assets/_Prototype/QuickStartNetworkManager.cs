using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class QuickStartNetworkManager : NetworkManager
    {
        private void Start()
        {
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
            TeamManager.Instance.AddTeam(playerControllerId);
        }
    }
}