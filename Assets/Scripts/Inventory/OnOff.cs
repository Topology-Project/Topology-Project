using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OnOff : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject UICanvas;
    public Text BulletText;
    private bool activeInventory = false;
    private bool Canvas = true;
    Player player;
    Weapon weapon;

    private void Start()
    {
        player = GameManager.Instance.Player;
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
    }

    private void Update()
    {
        InventoryOnOff();
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
    }
    private void InventoryOnOff()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            activeInventory = true;
            inventoryPanel.SetActive(true);
            Canvas = false;
            UICanvas.SetActive(false);
        }
        else
        {
            if (activeInventory)
            {
                activeInventory = false;
                inventoryPanel.SetActive(false);
                Canvas = true;
                UICanvas.SetActive(true);
            }
        }
    }
}
