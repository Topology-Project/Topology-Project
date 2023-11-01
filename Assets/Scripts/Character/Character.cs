using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, CharacterInterface
{
    /* ------------- 기본 스탯 -------------- */
    protected ProtectionType armorType; // 보호막, 장갑
    protected State maxHealthPoint; // 최대 체력
    protected State maxProtectionPoint; // 최대 보호막

    protected State movement; // 플레이어 속도
    protected State dashSpeed; // 대시 속도
    protected State dashDuration;
    protected State dashCooltime;

    protected State ammoRate;
    protected Ammunition maxInfiniteAmmo; // 탄창(우)
    protected Ammunition maxNomalAmmo;
    protected Ammunition maxLargeAmmo;
    protected Ammunition maxSpecialAmmo;
    protected int infiniteAmmo;
    protected int nomalAmmo;
    protected int largeAmmo;
    protected int specialAmmo;

    protected float healthPoint; // 현재 체력
    protected float protectionPoint; // 현재 보호막
    /* ------------------------------------------- */

    protected StateModifier stateModifier = new();
    protected ArrayList effects = new();
    public Weapon weapon;

    protected Rigidbody rig;

    protected virtual void Awake()
    {
        maxHealthPoint = new State(StateType.MaxHealthPoint);
        maxProtectionPoint = new State(StateType.MaxProtectionPoint);

        movement = new State(StateType.MovementSpeed);
        dashSpeed = new State(StateType.DashSpeed);
        dashDuration = new State(StateType.DashDuration);
        dashCooltime = new State(StateType.DashCooltime);

        ammoRate = new State(StateType.AmmoRate);

        stateModifier.AddHandler(maxHealthPoint);
        stateModifier.AddHandler(maxProtectionPoint);

        stateModifier.AddHandler(movement);
        stateModifier.AddHandler(dashSpeed);
        stateModifier.AddHandler(dashDuration);
        stateModifier.AddHandler(dashCooltime);

        stateModifier.AddHandler(ammoRate);
    }

    protected virtual void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();

        armorType = ProtectionType.Shield;

        maxHealthPoint.ResetState(30);
        maxProtectionPoint.ResetState( 80);

        movement.ResetState(8);
        dashSpeed.ResetState(2);
        dashDuration.ResetState(0.5f);
        dashCooltime.ResetState(3);

        ammoRate.ResetState(1);

        maxInfiniteAmmo = new Ammunition(AmmunitionType.Infinite, 999999999);
        maxNomalAmmo = new Ammunition(AmmunitionType.Nomal, 300);
        maxLargeAmmo = new Ammunition(AmmunitionType.Large, 200);
        maxSpecialAmmo = new Ammunition(AmmunitionType.Special, 100);

        infiniteAmmo = maxInfiniteAmmo.Ammo;
        nomalAmmo = maxNomalAmmo.Ammo;
        largeAmmo = maxLargeAmmo.Ammo;
        specialAmmo = maxSpecialAmmo.Ammo;

        stateModifier.AddHandler(weapon.GetModifier());
        weapon.OnWeapon(this);
        isDashReady = true;

        healthPoint = maxHealthPoint;
        protectionPoint = maxProtectionPoint;
    }

    protected virtual void Update()
    {

    }

    public virtual void Move(Vector3 dir)
    {
        if (isDash) dir *= stateModifier.GetState(StateType.DashSpeed);
        Vector3 temp = transform.position + transform.TransformDirection(dir) * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime;
        rig.MovePosition(temp);
    }
    protected bool isDash;
    private bool isDashReady;

    // 대시 메서드
    public void Dash()
    {
        if (!isDashReady) return;
        isDash = true;
        isDashReady = false;
        StartCoroutine(Dashing());
    }
    IEnumerator Dashing()
    {
        yield return new WaitForSeconds(stateModifier.GetState(StateType.DashDuration));
        isDash = false;
        StartCoroutine(DashCooltimer());
    }
    IEnumerator DashCooltimer()
    {
        yield return new WaitForSeconds(stateModifier.GetState(StateType.DashCooltime));
        isDashReady = true;
    }

    // 캐릭터 축 회전 메서드
    public virtual void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    // 좌클릭 메서드
    public virtual void Fire1(Transform transfrom) => weapon.Fire1(transform);

    // 재장전 메서드
    public void Reload() => weapon.Reload();
    // 모디파이어 반환 메서드
    public StateModifier GetModifier()
    {
        return this.stateModifier;
    }
    // 잔탄량 반환 메서드
    public int GetAmmo(AmmunitionType ammunitionType)
    {
        if (ammunitionType == AmmunitionType.Infinite) return infiniteAmmo;
        else if (ammunitionType == AmmunitionType.Nomal) return nomalAmmo;
        else if (ammunitionType == AmmunitionType.Large) return largeAmmo;
        else if (ammunitionType == AmmunitionType.Special) return specialAmmo;
        return 0;
    }
    // 잔탄량 설정 메서드
    public void SetAmmo(AmmunitionType ammunitionType, int ammo)
    {
        if (ammunitionType == AmmunitionType.Infinite) return;
        else if (ammunitionType == AmmunitionType.Nomal) nomalAmmo += ammo;
        else if (ammunitionType == AmmunitionType.Large) largeAmmo += ammo;
        else if (ammunitionType == AmmunitionType.Special) specialAmmo += ammo;
    }

    // 체력 설정 메서드
    private void HPCalc(float hp) => healthPoint = hp;
    // 체력 설정 (감소)
    private void Damage(float hp) => HPCalc(healthPoint - hp);
    // 체력 설정 (증가)
    private void Heal(float hp) => HPCalc(healthPoint + hp);
    // 대미지 계산 메서드
    public float DamageCalc(StateModifier stateModifier)
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

        Debug.Log("Damage : " + stateModifier.GetState(StateType.BaseDamage)
                    + "*" + stateModifier.GetState(StateType.WPNUpgrade)
                    + "*" + stateModifier.GetState(StateType.BaseDMGIncrease)
                    + "*" + stateModifier.GetState(StateType.ExplosionDMGIncrease)
                    + "*" + stateModifier.GetState(StateType.ElementalDMGIncrease)
                    + "*" + lsh
                    + "*" + stateModifier.GetState(StateType.CriticalX)
                    + "=" + damage);
        Damage(damage);
        return damage;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            // 부모의 모디파이어 객체를 가져옴
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag))
            {
                DamageCalc(parent.GetComponent<CharacterInterface>().GetModifier());
            }
        }
    }
}
