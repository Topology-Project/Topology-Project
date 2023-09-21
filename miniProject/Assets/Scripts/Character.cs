using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface
{
    public StateModifire Get();
}

public class Character : MonoBehaviour, CharacterInterface
{
    [SerializeField] protected float maxHealthPoint;
    [SerializeField] protected ProtectionType armorType;
    [SerializeField] protected float maxArmorPoint;
    [SerializeField] protected float defaultSpeed;

    [SerializeField] protected float healthPoint;
    [SerializeField] protected float armorPoint;

    public StateModifire stateModifire { get; protected set; }
    public Weapon weapon;

    protected void Move(Vector3 dir)
    {
        transform.Translate(dir * defaultSpeed * Time.deltaTime);
    }

    protected void Angle(Vector3 dir)
    {
        transform.localEulerAngles += new Vector3(0, dir.y, 0);
    }

    public StateModifire Get()
    {
        return this.stateModifire;
    }

    public float DamageCalc(StateModifire stateModifire)
    {
        float damage = stateModifire.GetState(StateType.BaseDamage)
                    * (1+stateModifire.GetState(StateType.WPNUpgrade)*0.15f)
                    * (1+stateModifire.GetState(StateType.baseDMGIncrease)*0.01f)
                    * (1+stateModifire.GetState(StateType.ExplosionDMGIncrease)*0.01f)
                    * (1+stateModifire.GetState(StateType.ElementalDMGIncrease)*0.01f);
        
        Debug.Log("Damage : " + stateModifire.GetState(StateType.BaseDamage)
                    + "*" + (1+stateModifire.GetState(StateType.WPNUpgrade)*0.15f)
                    + "*" + (1+stateModifire.GetState(StateType.baseDMGIncrease)*0.01f)
                    + "*" + (1+stateModifire.GetState(StateType.ExplosionDMGIncrease)*0.01f)
                    + "*" + (1+stateModifire.GetState(StateType.ElementalDMGIncrease)*0.01f)
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
