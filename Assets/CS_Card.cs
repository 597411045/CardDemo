using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CS_Card : NetworkBehaviour
{
    public string CardId;
    private string CoverId;
    private Image image;
    private bool isDraging;


    private GameObject lastParent;
    private Vector3 lastPosition;

    public static GameObject ZoomCard;
    public static GameObject GlobalLU;
    public static GameObject GlobalRD;
    public GameObject LU;
    public GameObject RD;


    // Start is called before the first frame update
    void Awake()
    {
        image = this.GetComponent<Image>();
        hits = new List<RaycastResult>();

        if (ZoomCard == null)
        {
            ZoomCard = GameObject.Find("ZoomCard");
        }
        if (GlobalLU == null)
        {
            GlobalLU = GameObject.Find("GlobalLU");
        }
        if (GlobalRD == null)
        {
            GlobalRD = GameObject.Find("GlobalRD");
        }
    }

    public void SetCover(string id)
    {
        if (image != null)
        {
            if (id == "0")
            {
                CoverId = id;
                image.sprite = Resources.Load<Sprite>("Cards/" + id);
            }
            else
            {
                CardId = id;
                image.sprite = Resources.Load<Sprite>("Cards/" + id);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDraging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void BeginDrag()
    {
        if (!isOwned) return;
        EndHover();

        isDraging = true;
        lastParent = this.transform.parent.gameObject;
        lastPosition = this.transform.position;
        this.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
    }


    List<RaycastResult> hits;

    public void EndDrag()
    {
        if (!isOwned) return;

        isDraging = false;

        if (hits != null)
        {
            hits.Clear();
        }

        PointerEventData tmp = new PointerEventData(EventSystem.current);
        tmp.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        EventSystem.current.RaycastAll(tmp, hits);
        foreach (var i in hits)
        {
            if (i.gameObject.transform.parent.gameObject.name.Contains("Right")) continue;

            foreach (var j in i.gameObject.GetComponents<Component>())
            {
                if (j is ISlotActiviy)
                {
                    CS_GameMode.localPlayer.GetComponent<CS_Player>().MoveACard(this.gameObject, j.gameObject);

                    //((ISlotActiviy)j).InsertACard(this.gameObject);
                    return;
                }
            }
            //ExecuteEvents.Execute(i.gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }

        foreach (var j in lastParent.gameObject.GetComponents<Component>())
        {
            if (j is ISlotActiviy)
            {
                ((ISlotActiviy)j).InsertACard(this.gameObject);

                return;
            }
        }
    }

    public void OnHover()
    {
        if (ZoomCard == null) return;
        if (isDraging) return;
        if (CoverId == "0") return;

        ZoomCard.GetComponent<Image>().enabled = true;
        ZoomCard.GetComponent<CS_Card>().SetCover(CardId);
        ZoomCard.transform.position = this.transform.position;

        Vector3 deltaLU = GlobalLU.transform.position - ZoomCard.GetComponent<CS_Card>().LU.transform.position;
        Vector3 deltaRD = GlobalRD.transform.position - ZoomCard.GetComponent<CS_Card>().RD.transform.position;

        if (deltaLU.x > 0)
        {
            ZoomCard.transform.position += new Vector3(deltaLU.x, 0, 0);
        }
        if (deltaLU.y < 0)
        {
            ZoomCard.transform.position += new Vector3(0, deltaLU.y, 0);
        }
        if (deltaRD.x < 0)
        {
            ZoomCard.transform.position += new Vector3(deltaRD.x, 0, 0);
        }
        if (deltaRD.y > 0)
        {
            ZoomCard.transform.position += new Vector3(0, deltaRD.y, 0);
        }

    }

    public void EndHover()
    {
        if (ZoomCard == null) return;

        ZoomCard.GetComponent<Image>().enabled = false;
    }

    public void OnClick()
    {
        if (isOwned)
        {
            CS_GameMode.localPlayer.GetComponent<CS_Player>().ClickOnOwned(this.gameObject);
        }
        else
        {
            CS_GameMode.localPlayer.GetComponent<CS_Player>().ClickOnOther(this.gameObject);
        }

        CS_GameMode.localPlayer.GetComponent<CS_Player>().Cmd_AddOne();
    }
}