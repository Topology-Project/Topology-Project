using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    public HpFillAmount hpFillAmount;
    private Camera playerCamera;
    private bool isJump;
    public List<Scroll.Data> inventory = new();
    public int scroll_cnt { get { return inventory.Count; } }
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerCamera = GameManager.Instance.MainCamera.GetComponent<Camera>();

        maxHealthPoint.ResetState(3000);
        healthPoint = maxHealthPoint;
        maxProtectionPoint.ResetState(8000);
        protectionPoint = maxProtectionPoint;
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

    public override float DamageCalc(StateModifier stateModifier)
    {
        int lsh = (int)(stateModifier.GetState(StateType.LuckyShot) % 1 > UnityEngine.Random.Range(0f, 1f) ?
                    stateModifier.GetState(StateType.LuckyShot) + 1 : stateModifier.GetState(StateType.LuckyShot));
        float damage = stateModifier.GetState(StateType.BaseDamage)
                    * stateModifier.GetState(StateType.WPNUpgrade)
                    * stateModifier.GetState(StateType.BaseDMGIncrease)
                    * stateModifier.GetState(StateType.ExplosionDMGIncrease)
                    * stateModifier.GetState(StateType.ElementalDMGIncrease)
                    * lsh
                    * stateModifier.GetState(StateType.CriticalX);
        Damage(damage);
        hpFillAmount.Temp();
        return damage;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag.Equals("Bullet"))
        {
            // 부모의 모디파이어 객체를 가져옴
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag))
            {
                GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.PlayerHit);
                if (healthPoint <= 0 && GameManager.Instance.IsPlay)
                {
                    GameManager.Instance.IsPlay = false;
                    GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.PlayerDie);
                }
            }
        }
    }
}
