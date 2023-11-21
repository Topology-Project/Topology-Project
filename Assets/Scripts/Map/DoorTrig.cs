using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrig : MonoBehaviour
{
    public bool isSet = false;
    
    void OnTriggerExit(Collider other)
    {
        // 플레이어를 진행중인 방으로 워프
        if(other.tag.Equals("Player") && !isSet)
        {
            GameManager.Instance.StageManager.mapManager.EnterRoom();
        }
    }
}
