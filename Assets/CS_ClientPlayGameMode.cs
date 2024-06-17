using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ClientPlayGameMode : NetworkBehaviour
{
    [SyncVar]
    public int ClickCount;

    public string output = "";
    public string stack = "";

    public static GameObject Player1;
    public static GameObject Player2;
    public static GameObject localPlayer;
    public static GameObject remotePlayer;
    public static GameObject GameMode;


    // Start is called before the first frame update
    private void Start()
    {
        
    }


    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string s1, string s2, LogType l1)
    {
        output = s1;
        stack = s2;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(150, 5, 800, 60), output);
        GUI.Label(new Rect(150, 65, 800, 60), stack);

    }

    private void Awake()
    {
        GameMode = this.gameObject;
    }


}
