using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    public StateModifier GetModifier();
}

public class Character : MonoBehaviour, CharacterInterface
{
    protected State movementSpeed;
    protected State maxHealthPoint;
    protected State maxArmorPoint;

    protected ProtectionType protectionType;
    protected float healthPoint;
    protected float armorPoint;

    protected StateModifier stateModifier = new();
    public Weapon weapon;
    // private int[] maxAmmuniitionsAmount = new int[3];
    // private int[] remainingAmmunitionsAmount = new int[3];

    void Start()
    {
        movementSpeed = new State(StateType.MovementSpeed, 10);
        maxHealthPoint = new State(StateType.MaxHealthPoint, 80);
        maxArmorPoint = new State(StateType.MaxArmorPoint, 80);
        
        stateModifier.AddHandler(movementSpeed);
        stateModifier.AddHandler(maxHealthPoint);
        stateModifier.AddHandler(maxArmorPoint);

        protectionType = ProtectionType.Shield;
        healthPoint = maxHealthPoint;
        armorPoint = maxArmorPoint;

        //weapon.OnWeapon(this);
    }

    protected void Move(Vector3 dir)
    {
        transform.Translate(dir * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime);
    }

    protected void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    public void Fire(Transform transform) => weapon.Fire(transform);
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
            if(!parent.tag.Equals(gameObject.tag))
                DamageCalc(parent.GetComponent<CharacterInterface>().GetModifier());
        }
    }
}
