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

    public string userId;


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
        if (!isServer)
        {
            NetworkIdentity ni = NetworkClient.connection.identity;
            CS_ClientPlayGameMode.localPlayer = ni.gameObject;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
        {
            RegisterAClient(this.gameObject);
        }
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
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
        if (isOwned)
        {
            go.GetComponent<CS_Card>().SetCover(id);
            HoldCardAreaLeft.GetComponentInChildren<CS_HoldCardAreaSlot>().InsertACard(go);

            Cmd_SwitchTurn(CS_ClientPlayGameMode.localPlayer, CS_ClientPlayGameMode.remotePlayer);
        }
        else
        {
            go.GetComponent<CS_Card>().SetCover(id);
            go.GetComponent<CS_Card>().SetCover("0");
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
                    Cmd_SwitchTurn(CS_ClientPlayGameMode.localPlayer, CS_ClientPlayGameMode.remotePlayer);
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

    [Command]
    public void ClickOnOwned(GameObject card)
    {
        Rpc_ClickOnOwned(card.GetComponent<NetworkIdentity>().connectionToClient, card);
    }



    [Command]
    public void ClickOnOther(GameObject card)
    {
        Rpc_ClickOnOther(card.GetComponent<NetworkIdentity>().connectionToClient, card);
    }

    [TargetRpc]
    public void Rpc_ClickOnOwned(NetworkConnection conn, GameObject card)
    {
        //Debug.LogError(conn.identity.GetComponent<CS_Player>().name + " Clicked " + card.GetComponent<CS_Card>().CardId + ", he owned");
        Debug.LogError("Rpc_ClickOnOwned");
    }

    [TargetRpc]
    public void Rpc_ClickOnOther(NetworkConnection conn, GameObject card)
    {
        //Debug.LogError(conn.identity.GetComponent<CS_Player>().name + " Clicked " + card.GetComponent<CS_Card>().CardId + ", he not owned");
        Debug.LogError("Rpc_ClickOnOwned");
    }

    [Command]
    public void Cmd_AddOne()
    {
        GameObject.Find("GameMode").GetComponent<CS_ClientPlayGameMode>().ClickCount++;
    }

    [Command]
    public void RegisterAClient(GameObject go)
    {
        if (CS_ClientPlayGameMode.Player1 == null)
        {
            CS_ClientPlayGameMode.Player1 = go;
            BroadCastNotify("Player 1 In");

            BroadCastClient(1, CS_ClientPlayGameMode.Player1);
            return;
        }
        if (CS_ClientPlayGameMode.Player2 == null)
        {
            CS_ClientPlayGameMode.Player2 = go;
            BroadCastNotify("Player 2 In");

            BroadCastClient(2, CS_ClientPlayGameMode.Player2);
            BroadCastClient(1, CS_ClientPlayGameMode.Player1);
            Rpc_StartTurn(CS_ClientPlayGameMode.Player1.GetComponent<NetworkIdentity>().connectionToClient);
            Rpc_WaitTurn(CS_ClientPlayGameMode.Player2.GetComponent<NetworkIdentity>().connectionToClient);
            return;
        }
    }

    [ClientRpc]
    public void BroadCastNotify(string s)
    {
        Debug.LogError(s);
    }


    [ClientRpc]
    public void BroadCastClient(int index, GameObject go)
    {
        if (index == 1)
        {
            if (CS_ClientPlayGameMode.Player1 == null)
            {
                CS_ClientPlayGameMode.remotePlayer = go;
                CS_ClientPlayGameMode.Player1 = go;
            }
        }
        if (index == 2)
        {
            if (CS_ClientPlayGameMode.Player2 == null)
            {
                CS_ClientPlayGameMode.remotePlayer = go;
                CS_ClientPlayGameMode.Player2 = go;
            }
        }
    }

    [TargetRpc]
    public void Rpc_StartTurn(NetworkConnection conn)
    {
        //Debug.LogError(conn.identity.GetComponent<CS_Player>().name + " Clicked " + card.GetComponent<CS_Card>().CardId + ", he not owned");
        Debug.LogError("Your Trun");
        GameObject.Find("AllHover").GetComponent<Image>().enabled = false;
    }

    [TargetRpc]
    public void Rpc_WaitTurn(NetworkConnection conn)
    {
        //Debug.LogError(conn.identity.GetComponent<CS_Player>().name + " Clicked " + card.GetComponent<CS_Card>().CardId + ", he not owned");
        Debug.LogError("Other Trun");
        GameObject.Find("AllHover").GetComponent<Image>().enabled = true;
    }

    [Command]
    public void Cmd_SwitchTurn(GameObject ender, GameObject starter)
    {
        Rpc_StartTurn(starter.GetComponent<NetworkIdentity>().connectionToClient);
        Rpc_WaitTurn(ender.GetComponent<NetworkIdentity>().connectionToClient);
    }


}