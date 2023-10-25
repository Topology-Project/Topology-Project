using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject AimUI;
    public Text BulletText;
    private bool activeInventory = false;
    private bool activeAim = true;
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
        // InventoryOnOff();
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
            activeAim = false;
            AimUI.SetActive(false);
        }
        else
        {
            if (activeInventory)
            {
                activeInventory = false;
                inventoryPanel.SetActive(false);
                activeAim = true;
                AimUI.SetActive(true);
            }
        }
    }
}
