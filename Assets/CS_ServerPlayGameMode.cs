using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using Mirror;
using System.Data;
using System.Diagnostics;

public class CS_ServerPlayGameMode : MonoBehaviour
{
    private CS_ClientPlayGameMode _clientPlayGameMode;
    public KcpTransport kcpTransport;
    public NetworkManager networkManager;
    public NetworkManagerHUD networkManagerHUD;
    private GameObject LocalInfoND;

    // Start is called before the first frame update
    void Start()
    {
        LocalInfoND = GameObject.Find("LocalInfoND");
        DontDestroyOnLoad(LocalInfoND);

        if (LocalInfoND.GetComponent<CS_LocalInfo>().isServer)
        {
            _clientPlayGameMode = this.GetComponent<CS_ClientPlayGameMode>();

            DataSet ds = LocalFunc.GetMysqlQuery("SELECT * FROM carddemo.tblprocess where process_id = '" + Process.GetCurrentProcess().Id.ToString() + "';");
            DataTable dt = ds.Tables[0];

            kcpTransport.Port = ushort.Parse(dt.Rows[0][1].ToString());
            networkManager.StartServer();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}