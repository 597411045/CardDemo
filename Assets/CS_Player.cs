using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CS_Player : NetworkBehaviour
{
    public GameObject Button;
    public GameObject PlayAreaLeft;
    public GameObject PlayAreaRight;
    public GameObject HoldCardAreaLeft;
    public GameObject HoldCardAreaRight;

    public string name;
    public static CS_Player localPlayer;

    private void Awake()
    {
        Button = GameObject.Find("Button");
        PlayAreaLeft = GameObject.Find("PlayAreaLeft");
        PlayAreaRight = GameObject.Find("PlayAreaRight");
        HoldCardAreaLeft = GameObject.Find("HoldCardAreaLeft");
        HoldCardAreaRight = GameObject.Find("HoldCardAreaRight");
        Debug.Log("Awake");
    }

    private void Start()
    {
        NetworkIdentity ni = NetworkClient.connection.identity;
        localPlayer = ni.GetComponent<CS_Player>();
        Debug.LogError(localPlayer.name);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        name += "Client";
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        name += "Server";
    }

    [Command]
    public void GetACard()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefab/Card"));
        NetworkServer.Spawn(go, connectionToClient);

        string id = UnityEngine.Random.Range(1, 20).ToString();
        Rpc_GetACard(go, id);
    }

    [ClientRpc]
    void Rpc_GetACard(GameObject go, string id)
    {
        go.GetComponent<CS_Card>().SetCover(id);
        Debug.LogError(id);

        if (isOwned)
        {
            HoldCardAreaLeft.GetComponentInChildren<CS_HoldCardAreaSlot>().InsertACard(go);
        }
        else
        {
            HoldCardAreaRight.GetComponentInChildren<CS_HoldCardAreaSlot>().InsertACard(go);
        }
    }

    [Command]
    public void MoveACard(GameObject go, GameObject parent)
    {
        Rpc_MoveACard(go, parent);
    }

    [ClientRpc]
    void Rpc_MoveACard(GameObject go, GameObject parent)
    {
        if (isOwned)
        {
            foreach (var j in parent.gameObject.GetComponents<Component>())
            {
                if (j is ISlotActiviy)
                {
                    ((ISlotActiviy)j).InsertACard(go);
                    return;
                }
            }
        }
        else
        {
            string tmp = parent.name;
            GameObject area = GameObject.Find(parent.transform.parent.gameObject.name.Replace("Left", "Right"));
            foreach (var i in area.GetComponentsInChildren<Transform>())
            {
                if (i.gameObject.name == tmp)
                {   
                    parent = i.gameObject;
                    break;
                }
            }

            foreach (var j in parent.gameObject.GetComponents<Component>())
            {
                if (j is ISlotActiviy)
                {
                    ((ISlotActiviy)j).InsertACard(go);
                    return;
                }
            }
        }
    }
}