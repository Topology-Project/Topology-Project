using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, CharacterInterface
{
    protected ProtectionType armorType;
    protected State maxHealthPoint;
    protected State maxProtectionPoint;

    protected State movement;
    protected State dashSpeed;
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

    protected float healthPoint;
    protected float protectionPoint;

    protected StateModifier stateModifier = new();
    protected ArrayList effects = new();
    public Weapon weapon;

    protected Rigidbody rig;

    protected virtual void Awake()
    {
        armorType = ProtectionType.Shield;
        maxHealthPoint = new State(StateType.MaxHealthPoint, 8000);
        maxProtectionPoint = new State(StateType.MaxProtectionPoint, 80);

        movement = new State(StateType.MovementSpeed, 8);
        dashSpeed = new State(StateType.DashSpeed, 2);
        dashDuration = new State(StateType.DashDuration, 0.5f);
        dashCooltime = new State(StateType.DashCooltime, 3);

        ammoRate = new State(StateType.AmmoRate, 1);

        maxInfiniteAmmo = new Ammunition(AmmunitionType.Infinite, 999999);
        maxNomalAmmo = new Ammunition(AmmunitionType.Nomal, 300);
        maxLargeAmmo = new Ammunition(AmmunitionType.Large, 200);
        maxSpecialAmmo = new Ammunition(AmmunitionType.Special, 100);

        infiniteAmmo = maxInfiniteAmmo.Ammo;
        nomalAmmo = maxNomalAmmo.Ammo;
        largeAmmo = maxLargeAmmo.Ammo;
        specialAmmo = maxSpecialAmmo.Ammo;

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

    public virtual void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    public virtual void Fire1(Transform transfrom) => weapon.Fire1(transform);
    public void Reload() => weapon.Reload();

    public StateModifier GetModifier()
    {
        return this.stateModifier;
    }
    public int GetAmmo(AmmunitionType ammunitionType)
    {
        if (ammunitionType == AmmunitionType.Infinite) return infiniteAmmo;
        else if (ammunitionType == AmmunitionType.Nomal) return nomalAmmo;
        else if (ammunitionType == AmmunitionType.Large) return largeAmmo;
        else if (ammunitionType == AmmunitionType.Special) return specialAmmo;
        return 0;
    }
    public void SetAmmo(AmmunitionType ammunitionType, int ammo)
    {
        if (ammunitionType == AmmunitionType.Infinite) return;
        else if (ammunitionType == AmmunitionType.Nomal) nomalAmmo += ammo;
        else if (ammunitionType == AmmunitionType.Large) largeAmmo += ammo;
        else if (ammunitionType == AmmunitionType.Special) specialAmmo += ammo;
    }

    private void HPCalc(float hp) => healthPoint = hp;
    private void Damage(float hp) => HPCalc(healthPoint - hp);
    private void Heal(float hp) => HPCalc(healthPoint + hp);
    public float DamageCalc(StateModifier stateModifier)
    {
        int lsh = (int)(stateModifier.GetState(StateType.LuckyShot) % 1 > Random.Range(0f, 1f) ?
                    stateModifier.GetState(StateType.LuckyShot) + 1 : stateModifier.GetState(StateType.LuckyShot));
        float damage = stateModifier.GetState(StateType.BaseDamage)
                    * stateModifier.GetState(StateType.WPNUpgrade)
                    * stateModifier.GetState(StateType.BaseDMGIncrease)
                    * stateModifier.GetState(StateType.ExplosionDMGIncrease)
                    * stateModifier.GetState(StateType.ElementalDMGIncrease)
                    * lsh
                    * stateModifier.GetState(StateType.CriticalX);

        // Debug.Log("Damage : " + stateModifier.GetState(StateType.BaseDamage)
        //             + "*" + stateModifier.GetState(StateType.WPNUpgrade)
        //             + "*" + stateModifier.GetState(StateType.BaseDMGIncrease)
        //             + "*" + stateModifier.GetState(StateType.ExplosionDMGIncrease)
        //             + "*" + stateModifier.GetState(StateType.ElementalDMGIncrease)
        //             + "*" + lsh
        //             + "*" + stateModifier.GetState(StateType.CriticalX)
        //             + "=" + damage);
        Damage(damage);
        return damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag)) DamageCalc(parent.GetComponent<CharacterInterface>().GetModifier());
        }
    }
}
