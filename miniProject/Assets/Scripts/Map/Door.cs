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

    public void DTSet() => doorTrig.isSet = true;
    public void WarpSet()
    {
        trig.SetActive(false);
        warp.SetActive(true);
    }
}
