using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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
    private State explosionDMGIncrease;
    private State elementalDMGIncrease;
    private State range;

    private State movementSpeed;

    private AmmunitionType ammunitionType;


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
        explosionDMGIncrease = new State(StateType.ExplosionDMGIncrease, 0);
        elementalDMGIncrease = new State(StateType.ElementalDMGIncrease, 0);
        range = new State(StateType.Range, 40);
        ammunitionType = AmmunitionType.Infinite;

        movementSpeed  = new State(StateType.MovementSpeed, 0);

        residualAmmunition = (int)magazine.value;
    }

    public void OnWeapon(Character character)
    {
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
        stateModifier.AddHandler(explosionDMGIncrease);;
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
        stateModifier.DelHandler(explosionDMGIncrease);;
        stateModifier.DelHandler(elementalDMGIncrease);
        stateModifier.DelHandler(range);

        stateModifier.DelHandler(movementSpeed);

        this.character = null;
        parent = null;
    }

    private bool isFire = false;
    public void Fire(Transform transform)
    {
        if(!isFire && residualAmmunition > 0) 
        {
            GameObject b = Instantiate(bullet, transform.position, transform.rotation);
            b.GetComponent<Bullet>().Set(parent,
                character.GetModifier().GetState(StateType.ProjectileSpeed),
                character.GetModifier().GetState(StateType.Range));
            isFire = true;
            StartCoroutine(FireReady());
            residualAmmunition--;
            Debug.Log(residualAmmunition + "/" + character.GetModifier().GetState(StateType.Magazine));
        }
        if(residualAmmunition <= 0) 
        {
            Debug.Log("need load");
            Reload();
        }
    }
    IEnumerator FireReady()
    {
        yield return new WaitForSeconds(0);
        float time = 1f / character.GetModifier().GetState(StateType.RateOfFire);
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
        yield return new WaitForSeconds(character.GetModifier().GetState(StateType.ReloadTime));
        int temp = (int)character.GetModifier().GetState(StateType.Magazine) - residualAmmunition;
        if(ammunitionType != AmmunitionType.Infinite); // 탄약 구현 후 수정
        residualAmmunition += temp;
    }
}
