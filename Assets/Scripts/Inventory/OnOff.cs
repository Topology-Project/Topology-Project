using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OnOff : MonoBehaviour
{
    public GameObject targetUI;


    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryOnOff();
        }
    }
    private void InventoryOnOff()
    {
        if(targetUI.activeSelf)
        {
            targetUI.SetActive(false);
            GameManager.Instance.IsPlay = true;
        }
        else 
        {
            targetUI.SetActive(true);
            GameManager.Instance.IsPlay = false;
        }
    }
}