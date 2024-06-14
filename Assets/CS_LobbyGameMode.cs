using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using System.Data;
using System;

public class CS_LobbyGameMode : MonoBehaviour
{
    public GameObject Login;
    public GameObject Rooms;
    public GameObject RoomsParent;
    public GameObject LocalInfoND;

    private MySqlConnection conn;

    public InputField username;
    public InputField password;

    private static string guiText;
    private GUIStyle guiStyle;


    // Start is called before the first frame update
    void Start()
    {
        Rooms.SetActive(false);

        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.UserID = "root";
        builder.Password = "P@ss1234";
        builder.Server = "101.132.190.13";
        builder.Database = "carddemo";
        conn = new MySqlConnection(builder.ConnectionString);
        conn.Open();
        DebugAText("Mysql连接成功");

        guiStyle = new GUIStyle();
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.red;

        DontDestroyOnLoad(LocalInfoND);
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
        string sql = "select * from tbluser where id = \"" + str_username + "\" and password = \"" + str_password + "\";";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        sda.Fill(ds, "Table1");
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 1)
        {
            DebugAText("登录成功");
            Rooms.SetActive(true);
            Login.SetActive(false);
            StartCoroutine(UpdateRoomInfoPerSeconds());
            LocalInfoND.GetComponent<CS_LocalInfo>().username = dt.Rows[0][0].ToString();
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
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        try
        {
            a = cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            DebugAText("注册失败");
            return;
        }
        if (a == 1)
        {
            DebugAText("注册成功");
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
        Debug.Log(text);
    }

    IEnumerator UpdateRoomInfoPerSeconds()
    {
        yield return new WaitForSeconds(2);
        string sql = "SELECT * FROM tblroom;";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        sda.Fill(ds, "Table1");
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
            string AddressText = dt.Rows[i][1].ToString();
            string ClientText = dt.Rows[i][2].ToString();
            string StatusText = dt.Rows[i][3].ToString();

            go.GetComponent<CS_RoomUnit>().UpdateInfo(HostText, ClientText, AddressText, StatusText);
        }
        StartCoroutine(UpdateRoomInfoPerSeconds());
    }

    private void OnDestroy()
    {
        conn.Close();
    }

    public void Button_CreateRoom()
    {
        string str_username = username.text;
        string str_password = password.text;
        int result = 0;
        string sql = "INSERT INTO tblroom VALUES ('" + LocalInfoND.GetComponent<CS_LocalInfo>().username + "', '0', '0', '0');";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        try
        {
            result = cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            DebugAText("创建失败");
            return;
        }
        if (result == 1)
        {
            DebugAText("创建成功");
        }
    }

    public void Button_DeleteRoom()
    {
        int result = 0;
        string sql = "DELETE FROM `carddemo`.`tblroom` WHERE (`host_user_id` = '" + LocalInfoND.GetComponent<CS_LocalInfo>().username + "');";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        try
        {
            result = cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            DebugAText("删除失败");
            return;
        }
        if (result == 1)
        {
            DebugAText("删除成功");
        }
    }
}
