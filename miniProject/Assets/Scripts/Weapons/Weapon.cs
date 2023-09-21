using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public State baseDamage { private set; get; }
    public State criticalX { private set; get; }
    public State luckyShot { private set; get; }
    public State magazine { private set; get; }
    public State projectiles { private set; get; }
    public State projectileSpeed { private set; get; }
    public State rateOfFire { private set; get; }
    public State reloadTime { private set; get; }
    public State upgrade { private set; get; }
    public State accuracy { private set; get; }
    public State stability { private set; get; }
    public AmmunitionType ammunitionType { private set; get; }
    public State baseDMGIncrease { private set; get; }
    public State ExplosionDMGIncrease { private set; get; }
    public State ElementalDMGIncrease { private set; get; }

    public State movementSpeed { private set; get; }


    Character character;
    GameObject parent;
    int residualAmmunition;
    public GameObject bullet;

    void Awake()
    {
        //bullet = Resources.Load("Bullet") as GameObject;
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
        baseDMGIncrease = new State(StateType.baseDMGIncrease, 0);
        ExplosionDMGIncrease = new State(StateType.ExplosionDMGIncrease, 0);
        ElementalDMGIncrease = new State(StateType.ElementalDMGIncrease, 0);
        ammunitionType = AmmunitionType.Infinite;

        movementSpeed  = new State(StateType.MovementSpeed, 0);

        residualAmmunition = (int)magazine.value;
    }

    public void OnWeapon(Character character)
    {
        this.character = character;
        parent = character.gameObject;
        StateModifire stateModifire = character.stateModifire;

        stateModifire.AddHandler(baseDamage);
        stateModifire.AddHandler(criticalX);
        stateModifire.AddHandler(luckyShot);
        stateModifire.AddHandler(magazine);
        stateModifire.AddHandler(projectiles);
        stateModifire.AddHandler(projectileSpeed);
        stateModifire.AddHandler(rateOfFire);
        stateModifire.AddHandler(reloadTime);
        stateModifire.AddHandler(upgrade);
        stateModifire.AddHandler(accuracy);
        stateModifire.AddHandler(stability);
        stateModifire.AddHandler(baseDMGIncrease);
        stateModifire.AddHandler(ExplosionDMGIncrease);;
        stateModifire.AddHandler(ElementalDMGIncrease);

        stateModifire.AddHandler(movementSpeed);
    }
    public void OffWeapon(Character character)
    {
        StateModifire stateModifire = character.stateModifire;
        stateModifire.DelHandler(baseDamage);
        stateModifire.DelHandler(criticalX);
        stateModifire.DelHandler(luckyShot);
        stateModifire.DelHandler(magazine);
        stateModifire.DelHandler(projectiles);
        stateModifire.DelHandler(projectileSpeed);
        stateModifire.DelHandler(rateOfFire);
        stateModifire.DelHandler(reloadTime);
        stateModifire.DelHandler(upgrade);
        stateModifire.DelHandler(accuracy);
        stateModifire.DelHandler(stability);
        stateModifire.DelHandler(baseDMGIncrease);
        stateModifire.DelHandler(ExplosionDMGIncrease);;
        stateModifire.DelHandler(ElementalDMGIncrease);

        stateModifire.DelHandler(movementSpeed);

        this.character = null;
        parent = null;
    }

    private bool isFire = false;
    public void Fire(Transform transform)
    {
        if(!isFire && residualAmmunition > 0) 
        {
            GameObject b = Instantiate(bullet, transform.position, transform.rotation);
            b.GetComponent<Bullet>().Set(parent, character.stateModifire.GetState(StateType.ProjectileSpeed));
            isFire = true;
            StartCoroutine(FireReady());
            residualAmmunition--;
            Debug.Log(residualAmmunition + "/" + character.stateModifire.GetState(StateType.Magazine));
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
        yield return new WaitForSeconds(0);
        float time = 1f / character.stateModifire.GetState(StateType.RateOfFire);
        yield return new WaitForSeconds(time);
        isFire = false;
    }

    public void Reload()
    {
        Debug.Log("reloading");
        StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds( character.stateModifire.GetState(StateType.ReloadTime));
        residualAmmunition = (int)character.stateModifire.GetState(StateType.Magazine);
    }
}
