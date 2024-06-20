using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class CS_ClientLobbyGameMode : MonoBehaviour
{
    public GameObject Login;
    public GameObject Rooms;
    public GameObject RoomsParent;
    private GameObject LocalInfoND;


    public InputField username;
    public InputField password;

    public static string guiText;
    public GUIStyle guiStyle;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        DebugAText("起点");
        Rooms.SetActive(false);



        guiStyle = new GUIStyle();
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.red;

        LocalInfoND = GameObject.Find("LocalInfoND");
        DontDestroyOnLoad(LocalInfoND);

        DebugAText("判断Process id");
        yield return new WaitForSeconds(1);


        int processid = Process.GetCurrentProcess().Id;
        DebugAText("SELECT * FROM carddemo.tblprocess where process_id = '" + processid + "';");

        if (LocalFunc.GetMysqlQuery("SELECT * FROM carddemo.tblprocess where process_id = '" + processid + "';").Tables[0].Rows.Count == 1)
        {
            DebugAText("判断Process id成功，是自动Server");

            CS_LocalInfo.isServer = true;
            SceneManager.LoadScene("Scenes/OnlyPlay");
        }
        else
        {
            DebugAText("判断Process id失败，是手动Client");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Button_Login()
    {
        string str_username = username.text;
        string str_password = password.text;

        if (str_username == "" || str_password == "")
        {
            DebugAText("不能为空");
            return;
        }

        string sql = "select * from tbluser where id = \"" + str_username + "\" and password = \"" + str_password +
                     "\";";

        DataSet ds = LocalFunc.GetMysqlQuery(sql);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 1)
        {
            DebugAText("登录成功");
            Rooms.SetActive(true);
            Login.SetActive(false);
            StartCoroutine(UpdateRoomInfoPerSeconds());
            CS_LocalInfo.username = dt.Rows[0][0].ToString();
        }
        else
        {
            DebugAText("登录失败");
        }
    }

    public void Button_Register()
    {
        string str_username = username.text;
        string str_password = password.text;
        int a = 0;
        string sql = "INSERT INTO tbluser VALUES (\"" + str_username + "\",\"" + str_password + "\");";
        LocalFunc.ExecuteMysqlQuery(sql);

        if (a == 1)
        {
            DebugAText("注册成功");
        }
        else
        {
            DebugAText("注册失败");
        }
    }

    private void OnGUI()
    {
        if (guiStyle != null)
        {
            GUI.Label(new Rect(100, 100, 800, 60), guiText, guiStyle);
        }
    }

    public static void DebugAText(string text)
    {
        guiText = text;
        Debug.LogError(text);
    }

    public IEnumerator UpdateRoomInfoPerSeconds()
    {
        yield return new WaitForSeconds(2);
        string sql = "select * FROM `carddemo`.`tblroom` WHERE (`status` = 2);";


        DataSet ds = LocalFunc.GetMysqlQuery(sql);
        DataTable dt = ds.Tables[0];

        for (int i = RoomsParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(RoomsParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefab/RoomUnit"));
            go.transform.SetParent(RoomsParent.transform);
            go.transform.rotation = RoomsParent.transform.rotation;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            string HostText = dt.Rows[i][0].ToString();
            string AddressText = dt.Rows[i][3].ToString();
            string ClientText = dt.Rows[i][2].ToString();
            string StatusText = dt.Rows[i][1].ToString();

            go.GetComponent<CS_RoomUnit>().UpdateInfo(HostText, ClientText, AddressText, StatusText);
        }

        StartCoroutine(UpdateRoomInfoPerSeconds());
    }



    public void Button_CreateRoom()
    {
        string str_username = username.text;
        string str_password = password.text;
        string sql = "INSERT INTO tblroom VALUES ('" + CS_LocalInfo.username +
                     "', '0', '0', '0');";

        int result = LocalFunc.ExecuteMysqlQuery(sql);
        if (result == 1)
        {
            DebugAText("创建成功");
        }
        else
        {
            DebugAText("创建失败");

        }
    }

    public void Button_DeleteRoom()
    {
        string sql = "DELETE FROM `carddemo`.`tblroom` WHERE (`host_user_id` = '" +
                     CS_LocalInfo.username + "');";

        int result = LocalFunc.ExecuteMysqlQuery(sql);

        if (result == 1)
        {
            DebugAText("删除成功");
        }
        else
        {
            DebugAText("删除失败");
        }
    }

    public void Button_Server()
    {
        CS_LocalInfo.isServer = true;
        SceneManager.LoadScene("Scenes/ServerLobby");
    }

    public void Button_Client()
    {
        CS_LocalInfo.isServer = false;
        SceneManager.LoadScene("Scenes/OnlyPlay");
    }
}