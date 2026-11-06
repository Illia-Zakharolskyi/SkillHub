using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SkillHub.TaskPhoton
{
    public class SimpleNetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Prefab Name for Player")]
    [SerializeField] private string playerPrefabName = "Player";

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, Quaternion.identity);
    }
}
}
