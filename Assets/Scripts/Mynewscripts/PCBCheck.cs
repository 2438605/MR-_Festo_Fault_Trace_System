using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBCheck : MonoBehaviour
{

    public void FindPCBcheck()
    {
        FindAnyObjectByType<PCBFullCheck>().GetPCBFullCheck();
    }
}