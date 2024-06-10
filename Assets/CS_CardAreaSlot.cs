using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CardAreaSlot : MonoBehaviour, ISlotActiviy
{
    public void InsertACard(GameObject go)
    {
        go.transform.rotation = this.transform.rotation;
        go.transform.SetParent(this.transform);
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
