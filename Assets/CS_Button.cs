using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CS_Button : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            CS_PlayGameMode.localPlayer.GetComponent<CS_Player>().GetACard();
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}