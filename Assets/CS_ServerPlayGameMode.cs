using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using Mirror;

public class CS_ServerPlayGameMode : MonoBehaviour
{
    private CS_ClientPlayGameMode _clientPlayGameMode;
    public KcpTransport kcpTransport;
    public NetworkManager networkManager;
    public NetworkManagerHUD networkManagerHUD;

    // Start is called before the first frame update
    void Start()
    {
        _clientPlayGameMode = this.GetComponent<CS_ClientPlayGameMode>();
        networkManager.StartServer();
    }

    // Update is called once per frame
    void Update()
    {
    }
}