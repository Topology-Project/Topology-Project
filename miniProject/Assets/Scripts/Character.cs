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
    protected ArrayList effects = new();
    public Weapon weapon;
    
    protected Rigidbody rig;

    protected virtual void Awake()
    {
        armorType = ProtectionType.Shield;
        maxHealthPoint = new State(StateType.MaxHealthPoint, 80, State.BaseOper);
        maxProtectionPoint = new State(StateType.MaxProtectionPoint, 80, State.BaseOper);

        movement = new State(StateType.MovementSpeed, 8, State.BaseOper);
        dashSpeed = new State(StateType.DashSpeed, 2, State.BaseOper);
        dashDuration = new State(StateType.DashDuration, 0.5f, State.BaseOper);
        dashCooltime = new State(StateType.DashCooltime, 3, State.BaseOper);

        stateModifier.AddHandler(maxHealthPoint);
        stateModifier.AddHandler(maxProtectionPoint);

        stateModifier.AddHandler(movement);
        stateModifier.AddHandler(dashSpeed);
        stateModifier.AddHandler(dashDuration);
        stateModifier.AddHandler(dashCooltime);
    }

    protected virtual void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        weapon.OnWeapon(this);
        isDashReady = true;
    }

    public void Move(Vector3 dir)
    {
        if(isDash) dir *= stateModifier.GetState(StateType.DashSpeed);
        Vector3 temp = transform.position + transform.TransformDirection(dir) * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime;
        rig.MovePosition(temp);
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

    public virtual void Fire1(Transform transfrom) => weapon.Fire1(transform);
    public void Reload() => weapon.Reload();

    public StateModifier GetModifier()
    {
        return this.stateModifier;
    }

    public float DamageCalc(StateModifier stateModifier)
    {
        int lsh = (int)(stateModifier.GetState(StateType.LuckyShot)%1 > Random.Range(0f, 1f) ?
                    stateModifier.GetState(StateType.LuckyShot)+1 : stateModifier.GetState(StateType.LuckyShot));
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
