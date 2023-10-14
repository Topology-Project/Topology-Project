using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject open;
    public GameObject close;

    public Map map0;
    public Map map1;

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

    void OnTriggerExit(Collider other)
    {
        
    }
}
