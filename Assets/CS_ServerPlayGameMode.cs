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

    // Start is called before the first frame update
    void Start()
    {
        if (CS_LocalInfo.isServer)
        {
            _clientPlayGameMode = this.GetComponent<CS_ClientPlayGameMode>();

            DataSet ds = LocalFunc.GetMysqlQuery("SELECT * FROM carddemo.tblprocess where process_id = '" + Process.GetCurrentProcess().Id.ToString() + "';");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {

            }
            else
            {
                kcpTransport.Port = ushort.Parse(dt.Rows[0][1].ToString());
                networkManager.StartServer();
                LocalFunc.ExecuteMysqlQuery("UPDATE `carddemo`.`tblroom` SET `status` = '2' WHERE (`host_user_id` = '" + dt.Rows[0][2].ToString() + "');");
            }
        }
        if (CS_LocalInfo.isClient)
        {
            kcpTransport.Port = ushort.Parse(CS_LocalInfo.TargetPort);
            networkManager.networkAddress = CS_LocalInfo.TargetIP;
            networkManager.StartClient();

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}