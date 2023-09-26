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

        baseDamage = new State(StateType.BaseDamage, 145, State.BaseOper);
        criticalX = new State(StateType.CriticalX, 2, State.BaseOper);
        luckyShot = new State(StateType.LuckyShot, 1, State.BaseOper);
        magazine = new State(StateType.Magazine, 9, State.BaseOper);
        projectiles = new State(StateType.Projectiles, 1, State.BaseOper);
        projectileSpeed = new State(StateType.ProjectileSpeed, 20, State.BaseOper);
        rateOfFire = new State(StateType.RateOfFire, 3, State.BaseOper);
        reloadTime = new State(StateType.ReloadTime, 1.35f, State.BaseOper);
        upgrade = new State(StateType.WPNUpgrade, 1, State.BaseOper);
        accuracy = new State(StateType.Accuracy, 0.2f, State.BaseOper);
        stability = new State(StateType.Stability, 10, State.BaseOper);
        baseDMGIncrease = new State(StateType.BaseDMGIncrease, 1, State.BaseOper);
        explosionRange = new State(StateType.ExplosionRange, 0, State.BaseOper);
        explosionDMGIncrease = new State(StateType.ExplosionDMGIncrease, 1, State.BaseOper);
        elementalRate = new State(StateType.ElementalRate, 0, State.BaseOper);
        elementalDMGIncrease = new State(StateType.ElementalDMGIncrease, 1, State.BaseOper);
        range = new State(StateType.Range, 20, State.BaseOper);

        movementSpeed  = new State(StateType.MovementSpeed, -0.1f);

        residualAmmunition = (int)magazine.value;
    }

    private void Update()
    {
        if(sumAccuracy > 0) sumAccuracy -= accuracy * 0.1f * Time.deltaTime;
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
    private float sumAccuracy = 0;
    public void Fire1(Transform transform)
    {
        if(!isReroad && !isFire && residualAmmunition > 0) 
        {
            int pj = (int)(character.GetModifier().GetState(StateType.Projectiles)%1 > Random.Range(0f, 1f) ? 
                        character.GetModifier().GetState(StateType.Projectiles)+1 : character.GetModifier().GetState(StateType.Projectiles));
            for(int i=0; i<pj; i++)
            {
                var xError = GetRandomNormalDistribution(0f, accuracy+sumAccuracy);
                var yError = GetRandomNormalDistribution(0f, accuracy+sumAccuracy);
                var fireDirection = transform.rotation;

                sumAccuracy += accuracy+sumAccuracy > accuracy*2 ? 0 : accuracy*0.1f;

                fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
                fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

                GameObject b = Instantiate(bullet, transform.position, fireDirection);
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

    public static float GetRandomNormalDistribution(float mean, float standard)  // 정규 분포로 부터 랜덤값을 가져오는 함수 
    {
        var x1 = Random.Range(0f, 1f);
        var x2 = Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
