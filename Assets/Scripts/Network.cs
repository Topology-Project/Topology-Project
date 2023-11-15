using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; 
    }
    void Start()
    {
	    PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
	    Debug.Log("On Connected To " + PhotonNetwork.CloudRegion + " Server!");
    }
}
