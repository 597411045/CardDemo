using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using System.Data;
using System;

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
        string sql = "SELECT * FROM tblroom;";
        MySqlCommand cmd = new MySqlCommand(sql, _clientLobbyGameMode.conn);
        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        sda.Fill(ds, "Table1");
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

            go.GetComponent<CS_RoomUnit>().UpdateInfo(HostText, ClientText, AddressText, StatusText);
        }

        StartCoroutine(ProcessMainServerFunctionPerSec());
    }
}