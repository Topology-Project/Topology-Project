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

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>().ToList();
        inven.onSlotCountAdd += AddSlot;
        inven.onSlotCountDel += DelSlot;
        player = GameManager.Instance.Player;
    }
    public void AddSlot(Slot var)
    {
        Slot go = Instantiate(slotObg, slotHolder).GetComponent<Slot>();
        // go.data = var;
        slots.Add(go);
    }
    public void DelSlot(Slot var)
    {
        int idx = slots.IndexOf(new Slot());
        Destroy(slots[idx].gameObject, 0.1f);
        slots.RemoveAt(idx);
    }
}