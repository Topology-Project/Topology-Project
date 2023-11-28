using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region  Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion
    public delegate void OnSlotCountChange(Slot val); //대리자 정의
    public OnSlotCountChange onSlotCountAdd; // 대리자 인스턴스화
    public OnSlotCountChange onSlotCountDel; // 대리자 인스턴스화
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            int temp = slotCnt;
            slotCnt = value;
        }
    }
    void Start()
    {
        // SlotCnt = 20;
    }
}
