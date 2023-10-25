using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrig : MonoBehaviour
{
    public bool isSet = false;
    
    void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Player") && !isSet)
        {
            GameManager.StageManager.mapManager.EnterRoom();
        }
    }
}
