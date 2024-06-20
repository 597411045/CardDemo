using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using System.Data;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.IO;

public class CS_ServerLobbyGameMode : MonoBehaviour
{
    private CS_ClientLobbyGameMode _clientLobbyGameMode;



    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        _clientLobbyGameMode = this.GetComponent<CS_ClientLobbyGameMode>();
        _clientLobbyGameMode.Rooms.SetActive(true);
        _clientLobbyGameMode.Login.SetActive(false);


        StartCoroutine(ProcessMainServerFunctionPerSec());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ProcessMainServerFunctionPerSec()
    {
        yield return new WaitForSeconds(1);
        DataSet ds = LocalFunc.GetMysqlQuery("SELECT * FROM tblroom;");
        DataTable dt = ds.Tables[0];

        for (int i = _clientLobbyGameMode.RoomsParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(_clientLobbyGameMode.RoomsParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefab/RoomUnit"));
            go.transform.SetParent(_clientLobbyGameMode.RoomsParent.transform);
            go.transform.rotation = _clientLobbyGameMode.RoomsParent.transform.rotation;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            string HostText = dt.Rows[i][0].ToString();
            string AddressText = dt.Rows[i][1].ToString();
            string ClientText = dt.Rows[i][2].ToString();
            string StatusText = dt.Rows[i][3].ToString();

            if (StatusText == "0")
            {
                if (LocalFunc.ExecuteMysqlQuery("UPDATE `carddemo`.`tblroom` SET `status` = '1' WHERE (`host_user_id` = '" + HostText + "');") == 1)
                {
                    bool flag = true;
                    int port = 7000;

                    while (flag)
                    {
                        string tmpStr = LocalFunc.UseCmd("netstat -ano|findstr " + port + "&exit");
                        if (tmpStr.Contains("UPD") || tmpStr.Contains("TCP"))
                        {
                            port++;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    int pid = LocalFunc.StartExe("C:/Build/CardDemo1.exe");

                    if (LocalFunc.ExecuteMysqlQuery("INSERT INTO `carddemo`.`tblprocess` (`process_id`, `port`, `host_name`) VALUES ('" + pid + "', '" + port + "', '" + HostText + "');") == 1)
                    {

                    }

                }

                //string sql2 = "SELECT * FROM tblprocess;";
                //DataSet ds2 = LocalFunc.GetMysqlQuery(sql2);
                //DataTable dt2 = ds2.Tables[0];


            }

            go.GetComponent<CS_RoomUnit>().UpdateInfo(HostText, ClientText, AddressText, StatusText);
        }

        StartCoroutine(ProcessMainServerFunctionPerSec());
    }


}