using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private Camera playerCamera;
    private bool isJump;
    public List<Scroll.Data> inventory = new();
    public int scroll_cnt { get; }
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerCamera = GameManager.Instance.MainCamera.GetComponent<Camera>();
    }

    protected override void Update()
    {
        // base.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        isJump = false; // 점프 초기화 용
    }

    public override void Move(Vector3 dir) // 플레이어 position 이동 메서드
    {
        if (isDash) dir *= stateModifier.GetState(StateType.DashSpeed);
        Vector3 temp = transform.position + transform.TransformDirection(dir) * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime;
        temp = new Vector3(temp.x, transform.position.y, temp.z);
        rig.MovePosition(temp);
    }

    public override void Angle(Vector3 dir) // 플레이어 Rotation 변경 메서드
    {
        transform.localEulerAngles = new Vector3(0, dir.y, 0);
    }

    // 점프 메서드
    public void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            rig.AddForce(Vector3.up * 4, ForceMode.VelocityChange);
        }
    }

    // 인벤토리 스크롤 추가용 메서드 (임시)
    public void AddInventory(Scroll.Data scroll)
    {
        inventory.Add(scroll);
        scroll.sc.Active();
        GameManager.Inventory.SlotCnt = inventory.Count;
        foreach (State state in scroll.sc.GetState())
            stateModifier.AddHandler(state);
    }
}
