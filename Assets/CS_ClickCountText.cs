using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_ClickCountText : MonoBehaviour
{
    private GameObject GameMode;
    private Text text;


    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMode == null)
        {

            GameMode = GameObject.Find("GameMode");
            return;
        }

        if (text != null)
        {
            text.text = GameMode.GetComponent<CS_ClientPlayGameMode>().ClickCount.ToString();
        }
    }
}
