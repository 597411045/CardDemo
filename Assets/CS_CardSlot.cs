using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CardSlot : MonoBehaviour, ISlotActiviy
{
    void ISlotActiviy.InsertACard(GameObject go)
    {
        go.transform.SetParent(this.transform);
        go.transform.rotation = this.transform.rotation;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale=Vector3.one;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
