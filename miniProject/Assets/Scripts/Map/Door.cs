using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject open;
    public GameObject close;

    public Transform wp0;
    public Transform wp1;

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
}
