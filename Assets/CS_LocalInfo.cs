using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LocalInfo : MonoBehaviour
{
    public static string username = "Not Login";
    public static bool isServer;
    public static bool isClient;
    public static string TargetIP = "101.132.190.13";
    public static string TargetPort;

    private GUIStyle guiStyle;
    void Start()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (guiStyle != null)
        {
            GUI.Label(new Rect(0, 0, 800, 60), username, guiStyle);
        }
    }
}
