using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    public StateModifier GetModifier();
}

public class Character : MonoBehaviour, CharacterInterface
{

    protected ProtectionType armorType;
    protected State maxHealthPoint;
    protected State maxProtectionPoint;

    protected State movement;
    protected State dashSpeed;
    protected State dashDuration;
    protected State dashCooltime;


    protected float healthPoint;
    protected float armorPoint;

    protected StateModifier stateModifier = new();
    public Weapon weapon;
    protected int equippedWeapon;

    protected virtual void Awake()
    {
        armorType = ProtectionType.Shield;
        maxHealthPoint = new State(StateType.MaxHealthPoint, 80);
        maxProtectionPoint = new State(StateType.MaxProtectionPoint, 80);

        movement = new State(StateType.MovementSpeed, 10);
        dashSpeed = new State(StateType.DashSpeed, 2);
        dashDuration = new State(StateType.DashDuration, 0.5f);
        dashCooltime = new State(StateType.DashCooltime, 3);

        stateModifier.AddHandler(maxHealthPoint);
        stateModifier.AddHandler(maxProtectionPoint);

        stateModifier.AddHandler(movement);
        stateModifier.AddHandler(dashSpeed);
        stateModifier.AddHandler(dashDuration);
        stateModifier.AddHandler(dashCooltime);

        equippedWeapon = 2;
        isDashReady = true;
    }

    public void Move(Vector3 dir)
    {
        if(isDash) dir *= stateModifier.GetState(StateType.DashSpeed);
        transform.Translate(dir * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime);
    }
    private bool isDash;
    private bool isDashReady;
    public void Dash()
    {
        if(!isDashReady) return;
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

    public void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    public virtual void Fire(Transform transfrom) => weapon.Fire(transform);
    public void Reload() => weapon.Reload();

    public StateModifier GetModifier()
    {
        return this.stateModifier;
    }

    public float DamageCalc(StateModifier stateModifier)
    {
        float damage = stateModifier.GetState(StateType.BaseDamage)
                    * (1+stateModifier.GetState(StateType.WPNUpgrade)*0.15f)
                    * (1+stateModifier.GetState(StateType.baseDMGIncrease)*0.01f)
                    * (1+stateModifier.GetState(StateType.ExplosionDMGIncrease)*0.01f)
                    * (1+stateModifier.GetState(StateType.ElementalDMGIncrease)*0.01f);
        
        Debug.Log("Damage : " + stateModifier.GetState(StateType.BaseDamage)
                    + "*" + (1+stateModifier.GetState(StateType.WPNUpgrade)*0.15f)
                    + "*" + (1+stateModifier.GetState(StateType.baseDMGIncrease)*0.01f)
                    + "*" + (1+stateModifier.GetState(StateType.ExplosionDMGIncrease)*0.01f)
                    + "*" + (1+stateModifier.GetState(StateType.ElementalDMGIncrease)*0.01f)
                    + "=" + damage);

        return damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Bullet"))
        {
            GameObject parent = other.GetComponent<Bullet>().parent;
            if(!parent.tag.Equals(gameObject.tag)) DamageCalc(parent.GetComponent<CharacterInterface>().GetModifier());
        }
    }
}
