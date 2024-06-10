using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_HoldCardAreaSlot : MonoBehaviour, ISlotActiviy
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

   

    public void InsertACard(GameObject go)
    {
        go.transform.SetParent(this.transform);
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }
}