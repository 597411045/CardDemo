using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LocalInfo : MonoBehaviour
{
    public string username = "Not Login";
    public bool isServer;

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
