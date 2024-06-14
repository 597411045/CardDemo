using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_RoomUnit : MonoBehaviour
{
    public Text HostText;
    public Text ClientText;
    public Text StatusText;
    public Text AddressText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateInfo(string s1, string s2, string s3, string s4)
    {
        HostText.text = s1;
        ClientText.text = s2;
        StatusText.text = s3;
        AddressText.text = s4;
    }
}
