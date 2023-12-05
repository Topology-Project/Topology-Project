using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;
    public GameObject slotObg;
    Player player;
    Weapon weapon;
    public List<Slot> slots;
    public Transform slotHolder;
    public GameObject targetUI;
    public GameObject ScrollUI;
    public GameObject weaponUI;

    private void Awake()
    {
        inven = GameManager.Inventory;
        inven.onSlotCountAdd += AddSlot;
        inven.onSlot += SetSlots;
        player = GameManager.Instance.Player;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void AddSlot(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            Slot go = Instantiate(slotObg, slotHolder).GetComponent<Slot>();
            slots.Add(go);
        }
    }
    // public void DelSlot(Slot var)
    // {
    //     int idx = slots.IndexOf(var);
    //     slots.RemoveAt(idx);
    // }

    public void SetSlots(int a)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (player.inventory.Count <= i) break;
            slots[i].SetData(player.inventory[i]);
            slots[i].SetScrollUI(ScrollUI.GetComponent<ScrollUI>());
        }
    }
    void Update()
    {
        
    }

    public void SetUI(int i)
    {
        ScrollUI.SetActive(false);
        weaponUI.SetActive(false);
        switch (i)
        {
            case 0:
                ScrollUI.SetActive(true);
                break;
            case 1:
                weaponUI.SetActive(true);
                break;
            default:
                break;
        }
    }
}