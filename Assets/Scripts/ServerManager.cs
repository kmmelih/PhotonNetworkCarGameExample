using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private Button baglanButton;
    [SerializeField] private InputField kullaniciadiInput;
    [SerializeField] private Text bilgiText;
    [SerializeField] private GameObject girisPanel;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        baglanButton.interactable = true;
        bilgiText.text = "";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("CARR", new Vector3(0,2,0), Quaternion.identity);
    }

    public void odayaGir()
    {
        PhotonNetwork.NickName = kullaniciadiInput.text;
        PhotonNetwork.JoinOrCreateRoom("oda", new RoomOptions{MaxPlayers = 15, IsOpen = true, IsVisible = true},TypedLobby.Default);
        girisPanel.SetActive(false);
    }
}
