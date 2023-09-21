using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    public StateModifier Get();
}

public class Character : MonoBehaviour, CharacterInterface
{
    [SerializeField] protected float maxHealthPoint;
    [SerializeField] protected ProtectionType armorType;
    [SerializeField] protected float maxArmorPoint;
    [SerializeField] protected float defaultSpeed;

    [SerializeField] protected float healthPoint;
    [SerializeField] protected float armorPoint;

    public StateModifier stateModifier { get; protected set; }
    public Weapon weapon;

    protected void Move(Vector3 dir)
    {
        transform.Translate(dir * defaultSpeed * Time.deltaTime);
    }

    protected void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    public StateModifier Get()
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
            if(!parent.tag.Equals(gameObject.tag)) DamageCalc(parent.GetComponent<CharacterInterface>().Get());
        }
    }
}
