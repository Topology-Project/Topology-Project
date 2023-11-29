using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject open;
    public GameObject close;
    public GameObject trig;
    public GameObject warp;
    public GameObject chest;
    public DoorTrig doorTrig;

    private bool isOpen;
    public bool IsOpen
    {
        set
        {
            open.SetActive(false);
            close.SetActive(false);

            isOpen = value;

            if(isOpen) open.SetActive(true);
            else close.SetActive(true);
        }
    }

    // 도어트리거 셋팅
    public void DTSet() => doorTrig.isSet = true;

    // 상자 언락
    public void ChestUnlock() => chest.GetComponent<Chest>().isOpen = true;

    // 상자 셋팅 메서드
    public void ChestSet()
    {
        if(chest != null) 
        {
            chest.SetActive(true);
            chest.GetComponent<Chest>().isOpen = false;
        }
    }

    // 워프용 게이트로 전환
    public void WarpSet()
    {
        trig.SetActive(false);
        warp.SetActive(true);
    }
}
