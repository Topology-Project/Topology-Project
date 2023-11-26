using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;
    public GameObject inventoryPanel;
    public GameObject UICanvas;
    public Text BulletText;
    private bool activeInventory = false;
    private bool Canvas = true;
    Player player;
    Weapon weapon;
    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        player = GameManager.Instance.Player;
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
    }
    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }
    private void Update()
    {
        InventoryOnOff();
        weapon = player.weapon;
        int a = player.GetAmmo(weapon.AmmunitionType);
        int b = weapon.ResidualAmmunition;
        BulletText.text = b + " / " + a;
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {

            }
        }
    }

    public void AddSlot()
    {
        inven.SlotCnt++;
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
