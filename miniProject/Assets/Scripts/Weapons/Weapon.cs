using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private AmmunitionType ammunitionType;
    private WeaponType weaponType;
    private ElementalEffectType elementalEffectType;
    private bool isExplosion;

    private State baseDamage;
    private State criticalX;
    private State luckyShot;
    private State magazine;
    private State projectiles;
    private State projectileSpeed;
    private State rateOfFire;
    private State reloadTime;
    private State upgrade;
    private State accuracy;
    private State stability;
    private State baseDMGIncrease;
    private State explosionRange;
    private State explosionDMGIncrease;
    private State elementalRate;
    private State elementalDMGIncrease;
    private State range;

    private State movementSpeed;


    private Character character;
    private GameObject parent;
    private int residualAmmunition;
    public GameObject bullet;

    void Awake()
    {
        //bullet = Resources.Load("Bullet") as GameObject;
        ammunitionType = AmmunitionType.Infinite;
        weaponType = WeaponType.Pistol;
        elementalEffectType = ElementalEffectType.None;
        isExplosion = false;

        baseDamage = new State(StateType.BaseDamage, 145);
        criticalX = new State(StateType.CriticalX, 2);
        luckyShot = new State(StateType.LuckyShot, 0);
        magazine = new State(StateType.Magazine, 9);
        projectiles = new State(StateType.Projectiles, 1);
        projectileSpeed = new State(StateType.ProjectileSpeed, 10);
        rateOfFire = new State(StateType.RateOfFire, 3);
        reloadTime = new State(StateType.ReloadTime, 1.35f);
        upgrade = new State(StateType.WPNUpgrade, 1);
        accuracy = new State(StateType.Accuracy, 10);
        stability = new State(StateType.Stability, 10);
        baseDMGIncrease = new State(StateType.BaseDMGIncrease, 0);
        explosionRange = new State(StateType.ExplosionRange, 0);
        explosionDMGIncrease = new State(StateType.ExplosionDMGIncrease, 0);
        elementalRate = new State(StateType.ElementalRate, 0);
        elementalDMGIncrease = new State(StateType.ElementalDMGIncrease, 0);
        range = new State(StateType.Range, 40);

        movementSpeed  = new State(StateType.MovementSpeed, 0.5f, State.MulOper);

        residualAmmunition = (int)magazine.value;
    }

    public void OnWeapon(Character character)
    {
        isFire = false;
        isReroad = false;

        this.character = character;
        parent = character.gameObject;
        StateModifier stateModifier = character.GetModifier();

        stateModifier.AddHandler(baseDamage);
        stateModifier.AddHandler(criticalX);
        stateModifier.AddHandler(luckyShot);
        stateModifier.AddHandler(magazine);
        stateModifier.AddHandler(projectiles);
        stateModifier.AddHandler(projectileSpeed);
        stateModifier.AddHandler(rateOfFire);
        stateModifier.AddHandler(reloadTime);
        stateModifier.AddHandler(upgrade);
        stateModifier.AddHandler(accuracy);
        stateModifier.AddHandler(stability);
        stateModifier.AddHandler(baseDMGIncrease);
        stateModifier.AddHandler(explosionRange);
        stateModifier.AddHandler(explosionDMGIncrease);
        stateModifier.AddHandler(elementalRate);
        stateModifier.AddHandler(elementalDMGIncrease);
        stateModifier.AddHandler(range);

        stateModifier.AddHandler(movementSpeed);
    }
    public void OffWeapon(Character character)
    {
        StateModifier stateModifier = character.GetModifier();
        stateModifier.DelHandler(baseDamage);
        stateModifier.DelHandler(criticalX);
        stateModifier.DelHandler(luckyShot);
        stateModifier.DelHandler(magazine);
        stateModifier.DelHandler(projectiles);
        stateModifier.DelHandler(projectileSpeed);
        stateModifier.DelHandler(rateOfFire);
        stateModifier.DelHandler(reloadTime);
        stateModifier.DelHandler(upgrade);
        stateModifier.DelHandler(accuracy);
        stateModifier.DelHandler(stability);
        stateModifier.DelHandler(baseDMGIncrease);
        stateModifier.DelHandler(explosionRange);
        stateModifier.DelHandler(explosionDMGIncrease);
        stateModifier.DelHandler(elementalRate);
        stateModifier.DelHandler(elementalDMGIncrease);
        stateModifier.DelHandler(range);

        stateModifier.DelHandler(movementSpeed);

        this.character = null;
        parent = null;
    }

    private bool isFire = false;
    private bool isReroad = false;
    public void Fire1(Transform transform)
    {
        if(!isReroad && !isFire && residualAmmunition > 0) 
        {
            int pj = (int)(character.GetModifier().GetState(StateType.Projectiles)%1 > Random.Range(0f, 1f) ? 
                        character.GetModifier().GetState(StateType.Projectiles)+1 : character.GetModifier().GetState(StateType.Projectiles));
            for(int i=0; i<pj; i++)
            {
                GameObject b = Instantiate(bullet, transform.position, transform.rotation);
                b.GetComponent<Bullet>().Set(parent,
                    character.GetModifier().GetState(StateType.ProjectileSpeed),
                    character.GetModifier().GetState(StateType.Range));
            }
            isFire = true;
            StartCoroutine(FireReady());
            residualAmmunition--;
            Debug.Log(residualAmmunition + "/" + character.GetModifier().GetState(StateType.Magazine));
        }
        if(residualAmmunition <= 0) 
        {
            Debug.Log("need load");
            Reload();
            return;
        }
    }
    IEnumerator FireReady()
    {
        float time = 1f / character.GetModifier().GetState(StateType.RateOfFire);
        yield return new WaitForSeconds(time);
        isFire = false;
    }

    public void Reload()
    {
        Debug.Log("reloading");
        isReroad = true;
        StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds( character.GetModifier().GetState(StateType.ReloadTime));
        residualAmmunition = (int)character.GetModifier().GetState(StateType.Magazine);
        isReroad = false;
    }
}
