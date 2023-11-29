using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // #region  Singleton
    // public static Inventory instance;
    // private void Awake()
    // {
    //     if (instance != null)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     instance = this;
    // }
    // #endregion
    public delegate void OnSlotCountChange(int val); //대리자 정의
    public OnSlotCountChange onSlotCountAdd; // 대리자 인스턴스화
    public OnSlotCountChange onSlot; // 대리자 인스턴스화
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            if(slotCnt < value)
            {
                int temp = slotCnt;
                slotCnt = value;
                onSlotCountAdd.Invoke(slotCnt-temp);
            }
            onSlot.Invoke(0);
        }
    }
    void Start()
    {
        SlotCnt = 20;
    }
}
