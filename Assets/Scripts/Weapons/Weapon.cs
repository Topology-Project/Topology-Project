using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private AmmunitionType ammunitionType;
    public AmmunitionType AmmunitionType
    {
        get
        {
            return ammunitionType;
        }
        private set
        {
            ammunitionType = value;
        }
    }
    private WeaponType weaponType;
    private FireType fireType;
    private ElementalEffectType elementalEffectType;
    private bool isExplosion;

    private State baseDamage;
    private State criticalX;
    private State luckyShot;
    private State magazine; //
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
    private int residualAmmunition;// 현재 탄창(좌)
    public int ResidualAmmunition
    {
        get
        {
            return residualAmmunition;
        }
        private set
        {
            residualAmmunition = value;
        }
    }
    public GameObject bullet;
    private StateModifier stateModifier = new();
    Inscription.Data[] inscriptions = new Inscription.Data[2];

    void Awake()
    {
        baseDamage = new State(StateType.BaseDamage);
        criticalX = new State(StateType.CriticalX);
        luckyShot = new State(StateType.LuckyShot);
        magazine = new State(StateType.Magazine);
        projectiles = new State(StateType.Projectiles);
        projectileSpeed = new State(StateType.ProjectileSpeed);
        rateOfFire = new State(StateType.RateOfFire);
        reloadTime = new State(StateType.ReloadTime);
        upgrade = new State(StateType.WPNUpgrade);
        accuracy = new State(StateType.Accuracy);
        stability = new State(StateType.Stability);
        baseDMGIncrease = new State(StateType.BaseDMGIncrease);
        explosionRange = new State(StateType.ExplosionRange);
        explosionDMGIncrease = new State(StateType.ExplosionDMGIncrease);
        elementalRate = new State(StateType.ElementalRate);
        elementalDMGIncrease = new State(StateType.ElementalDMGIncrease);
        range = new State(StateType.Range);
        movementSpeed = new State(StateType.Range);

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

    void Start()
    {
        //bullet = Resources.Load("Bullet") as GameObject;
        ammunitionType = AmmunitionType.Nomal;
        weaponType = WeaponType.Pistol;
        fireType = FireType.Single;
        elementalEffectType = ElementalEffectType.None;
        isExplosion = false;

        baseDamage.ResetState(145);
        criticalX.ResetState(2);
        luckyShot.ResetState(1);
        magazine.ResetState(9);
        projectiles.ResetState(1);
        projectileSpeed.ResetState(60);
        rateOfFire.ResetState(30);
        reloadTime.ResetState(1.35f);
        upgrade.ResetState(1);
        accuracy.ResetState(1.2f);
        stability.ResetState(5f);
        baseDMGIncrease.ResetState(1);
        explosionRange.ResetState(0);
        explosionDMGIncrease.ResetState(1);
        elementalRate.ResetState(0);
        elementalDMGIncrease.ResetState(1);
        range.ResetState(20);

        movementSpeed.ResetState(-0.1f, State.AddOper);

        inscriptions = GameManager.Instance.Inscriptions.GetRandomDatas(2);
        foreach(Inscription.Data data in inscriptions) SetInscriprion(data);


        residualAmmunition = (int)stateModifier.GetState(StateType.Magazine);
    }

    private void Update()
    {
        // 총기 안정성 회복
        if (sumAccuracy > 0) sumAccuracy -= accuracy * 0.1f * Time.deltaTime;
        sumAccuracy = Math.Clamp(sumAccuracy, 0, accuracy);
    }

    public StateModifier GetModifier()
    {
        return stateModifier;
    }

    // 인장적용 (임시)
    private void SetInscriprion(Inscription.Data data)
    {
        foreach(State state in data.states) 
        {
            stateModifier.AddHandler(state);
            GameManager.Instance.TriggerManager.AddTrigger(data.stackTrigger, state.StackUp);
            GameManager.Instance.TriggerManager.AddTrigger(data.rateTrigger, state.RateUp);
        }
    }

    // 총기 장착(임시)
    public void OnWeapon(Character character)
    {
        isFire = false;
        isReload = false;

        this.character = character;
        parent = character.gameObject;
    }

    // 총기 해제(임시)
    public void OffWeapon()
    {
        this.character = null;
        parent = null;
    }

    private bool isFire = false;
    private bool isFireready = true;
    private bool isReload = false;
    private float sumAccuracy = 0;

    // 좌클릭 메서드
    public void Fire1(Transform transform)
    {
        if (!isReload && !isFire && isFireready && residualAmmunition > 0)
        {
            // 투사체 갯수 설정
            // ex) 1.3 = 1 (70%) or 2 (30%)
            int pj = (int)(character.GetModifier().GetState(StateType.Projectiles) % 1 > UnityEngine.Random.Range(0f, 1f) ?
                        character.GetModifier().GetState(StateType.Projectiles) + 1 : character.GetModifier().GetState(StateType.Projectiles));
            for (int i = 0; i < pj; i++) // 투사체 개수만큼 반복
            {
                var xError = GetRandomNormalDistribution(0f, sumAccuracy); // 탄퍼짐
                var yError = GetRandomNormalDistribution(0f, sumAccuracy); // 탄퍼짐
                Quaternion fireDirection = new();
                if (parent.tag.Equals("Player"))
                {
                    // 플레이어 카메라 반동
                    PlayerCamera playerCamera = GameManager.Instance.MainCamera.GetComponent<PlayerCamera>();
                    fireDirection = playerCamera.transform.rotation;
                    playerCamera.SetStability(stability);
                }

                sumAccuracy += sumAccuracy > accuracy ? 0 : accuracy * 0.1f; // 안정성 감소 (탄퍼짐 증가)

                fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
                fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

                // bullet인스턴스 생성 및 초기화 (임시)
                GameObject b = Instantiate(bullet, transform.position, fireDirection);
                b.GetComponent<Bullet>().Set(parent,
                    character.GetModifier().GetState(StateType.ProjectileSpeed),
                    character.GetModifier().GetState(StateType.Range));
            }
            isFire = true;
            if (fireType == FireType.Single || fireType == FireType.Bust) isFireready = false;
            StartCoroutine(FireReady());
            residualAmmunition--;
            // Debug.Log(residualAmmunition + "/" + character.GetModifier().GetState(StateType.Magazine));
        }
        if (residualAmmunition <= 0)
        {
            // 잔탄 소진 시 재장전
            // Debug.Log("need load");
            Reload();
        }
        if (Input.GetButtonUp("Fire1")) isFireready = true;
    }

    // 차탄 사격 딜레이 용
    IEnumerator FireReady()
    {
        float time = 1f / character.GetModifier().GetState(StateType.RateOfFire);
        yield return new WaitForSeconds(time);
        isFire = false;
    }

    // 재장전 메서드
    public void Reload()
    {
        if (isReload ||
        (int)character.GetModifier().GetState(StateType.Magazine) == residualAmmunition ||
        parent.GetComponent<CharacterInterface>().GetAmmo(ammunitionType) <= 0) return;

        isReload = true;
        StartCoroutine(Reloading());
    }
    // 장전 딜레이 용 코루틴
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(character.GetModifier().GetState(StateType.ReloadTime));
        CharacterInterface characterInterface = parent.GetComponent<CharacterInterface>();
        int maxAmmo = characterInterface.GetAmmo(ammunitionType);
        int magazineAmmo = (int)character.GetModifier().GetState(StateType.Magazine) - residualAmmunition;
        int temp = 0;
        if (magazineAmmo > maxAmmo) temp = maxAmmo;
        else temp = magazineAmmo;
        residualAmmunition += temp;
        characterInterface.SetAmmo(ammunitionType, -temp);
        isReload = false;
    }

    public static float GetRandomNormalDistribution(float mean, float standard)  // 정규 분포로 부터 랜덤값을 가져오는 함수 
    {
        var x1 = UnityEngine.Random.Range(0f, 1f);
        var x2 = UnityEngine.Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
