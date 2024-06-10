using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CS_Card : NetworkBehaviour
{
    private string CardId;
    private Image image;
    private bool isDraging;


    private GameObject lastParent;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Awake()
    {
        image = this.GetComponent<Image>();
        hits = new List<RaycastResult>();
    }

    public void SetCover(string id)
    {
        if (image != null)
        {
            CardId = id;
            image.sprite = Resources.Load<Sprite>("Cards/" + CardId);
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
                    CS_Player.localPlayer.MoveACard(this.gameObject, j.gameObject);

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
}