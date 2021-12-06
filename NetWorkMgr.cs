using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetWorkMgr : MonoBehaviourPunCallbacks
{

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    //���� ������ ����
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        Debug.Log("no room, and create room, network successful");
        PhotonNetwork.CreateRoom("room", roomOptions);
    }

    //�濡 ������ �Ŀ� �÷��̾� ����
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("");
    }
}

