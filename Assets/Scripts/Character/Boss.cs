using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    private GameObject target;

    private Animator animator;

    private float WaitT = 0f;
    private float MaxWaitT = 5f;

    private bool isAttack = true;

    public GameObject warning;
    private List<GameObject> warning_list = new List<GameObject>();

    public GameObject razer;
    private LineRenderer LR;
    private bool isRazer = false;

    public GameObject pillar;
    private List<GameObject> pillar_list = new List<GameObject>();

    public GameObject meteor;
    private List<GameObject> meteor_list = new List<GameObject>();

    public GameObject[] punch;
    private List<GameObject> punch_list = new List<GameObject>();

    // 공격 시작 메소드
    private void BossAttack()
    {
        if (!isAttack)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("3909_Stand_InCombat") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                switch (GetRandomPattern())
                {
                    // case "Razer":
                    //     StartCoroutine(Razer());
                    //     break;
                    // case "Stone_Pillar":
                    //     StartCoroutine(Stone_Pillar());
                    //     break;
                    // case "Meteor":
                    //     StartCoroutine(Meteor());
                    //     break;
                    case "Rocket_Punch":
                        StartCoroutine(Rocket_Punch());
                        break;
                }
            }
        }
        else
        {
            Waiting();
        }
    }

    // 랜덤 패턴 값 가져오기
    private string GetRandomPattern()
    {
        string[] boss_Patterns = { "Razer", "Stone_Pillar", "Meteor", "Rocket_Punch" };
        int value = Random.Range(0, boss_Patterns.Length);
        return boss_Patterns[value];
    }

    // 대기 메소드
    private void Waiting()
    {
        if (WaitT < MaxWaitT)
        {
            WaitT += Time.deltaTime;
        }
        else
        {
            WaitT = 0f;
            isAttack = false;
        }
    }

    // 레이저 패턴
    private IEnumerator Razer()
    {
        isAttack = true;
        animator.SetTrigger("LaserStart");
        yield return new WaitForSecondsRealtime(1.5f);
        isRazer = true;
        yield return new WaitForSecondsRealtime(2.0f);
        isRazer = false;
        Destroy_Razer();
        animator.SetTrigger("LaserEnd");
    }
    private void Setup_Razer()
    {
        LR = razer.GetComponent<LineRenderer>();
        LR.positionCount = 2;
        LR.startWidth = 1f;
        LR.endWidth = 1f;
    }
    RaycastHit hit;
    private void Shot_Razer()
    {
        LR.enabled = true;

        Vector3 razerPoint = razer.transform.position;
        Vector3 endPoint = target.transform.position + Vector3.down;

        if (Physics.Linecast(razerPoint, endPoint, out hit))
        {
            endPoint = hit.transform.position + Vector3.down;
        }

        LR.SetPosition(0, razerPoint);
        LR.SetPosition(1, endPoint);
    }
    private void Destroy_Razer()
    {
        if (hit.transform.tag.Equals("Player")) hit.transform.GetComponent<Player>().DamageCalc(stateModifier);
        else if (hit.transform.tag.Equals("Pillar")) hit.transform.GetComponent<Stone_Pillar>().DestroyPillar();
        LR.enabled = false;
    }

    // 돌기둥 패턴
    private IEnumerator Stone_Pillar()
    {
        isAttack = true;
        animator.SetTrigger("HitGround");
        Warning_Destroy_Pillar();
        while (true)
        {
            yield return null;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("3909_Attack01 (2)") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            // if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                Destroy_Pillar();
                break;
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        animator.SetTrigger("Summon");
        Warning_Build_Pillar();
        while (true)
        {
            yield return null;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("3909_Attack01 (3)") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f)
            {
                Build_Pillar();
                break;
            }
        }
    }
    private void Warning_Build_Pillar()
    {
        for (float x = -30; x <= 20; x += 6f)
        {
            for (float z = -10; z <= 10; z += 10)
            {
                float randomX = Random.Range(-3.0f, 3.0f);
                float randomZ = Random.Range(-3.0f, 3.0f);

                if (x % 12 == 0 && z != 0)
                {
                    warning_list.Add(Instantiate(warning, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 12 == 6f && z == 0)
                {
                    warning_list.Add(Instantiate(warning, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
            }
        }
    }
    private void Build_Pillar()
    {
        for (int i = 0; i < warning_list.Count; i++)
        {
            pillar_list.Add(Instantiate(pillar, warning_list[i].transform.position, warning_list[i].transform.rotation));
            Destroy(warning_list[i]);
        }
        warning_list.Clear();
    }
    private void Warning_Destroy_Pillar()
    {
        for (int i = 0; i < pillar_list.Count; i++)
        {
            warning_list.Add(Instantiate(warning, pillar_list[i].transform.position, pillar_list[i].transform.rotation));
            if (!pillar_list[i].activeSelf)
            {
                warning_list[i].SetActive(false);
            }
        }
    }
    private void Destroy_Pillar()
    {
        for (int i = 0; i < pillar_list.Count; i++)
        {
            Destroy(pillar_list[i]);
            Destroy(warning_list[i]);
        }
        pillar_list.Clear();
        warning_list.Clear();
    }
    private void Setup_Pillar()
    {
        for (float x = -30; x <= 20; x += 6f)
        {
            for (float z = -10; z <= 10; z += 10)
            {
                float randomX = Random.Range(-3.0f, 3.0f);
                float randomZ = Random.Range(-3.0f, 3.0f);
                GameObject go = null;
                if (x % 12 == 0 && z != 0)
                {
                    pillar_list.Add(go = Instantiate(pillar, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 12 == 6f && z == 0)
                {
                    pillar_list.Add(go = Instantiate(pillar, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                if (go != null) SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneAt(1));
            }
        }
    }

    // 메테오 패턴
    private IEnumerator Meteor()
    {
        isAttack = true;
        animator.SetTrigger("FireStart");
        Spawn_Meteor();
        yield return new WaitForSecondsRealtime(2.0f);
        StartCoroutine(Drop_Meteor());
        animator.SetTrigger("FireEnd");
    }
    private void Spawn_Meteor()
    {
        meteor_list.Clear();

        for (int i = 0; i <= 40; i += 10)
        {
            float randomX = Random.Range(-3.0f, 3.0f) - 20;
            float randomY = Random.Range(25f, 30f);
            GameObject go = null;
            meteor_list.Add(go = Instantiate(meteor, gameObject.transform.position + new Vector3(randomX + i, randomY, 0), gameObject.transform.rotation));
            go.GetComponent<Meteor>().Set(gameObject, 0, 100, 100);
        }
    }
    private IEnumerator Drop_Meteor()
    {
        for (int i = 0; i < meteor_list.Count; i++)
        {
            Vector3 shotDirection = (target.transform.position - meteor_list[i].transform.position).normalized;
            float shotSpeed = 20f;
            meteor_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

    // 로켓펀치 패턴
    private IEnumerator Rocket_Punch()
    {
        isAttack = true;
        animator.SetTrigger("ArmFireStart");
        Ready_Punch();
        yield return new WaitForSecondsRealtime(2.0f);
        StartCoroutine(Shot_Punch());
        yield return new WaitForSecondsRealtime(2.0f);
        animator.SetTrigger("ArmFireEnd");
    }
    private void Ready_Punch()
    {
        Vector3 bossPos = transform.position;
        Vector3 playerPos = target.transform.position;
        punch_list.Clear();
        GameObject go = null;
        punch_list.Add(go = Instantiate(punch[0], new Vector3(bossPos.x + 20, 10, bossPos.z - 5), transform.rotation));
        go.GetComponent<Rocket_Punch>().Set(gameObject, 0, 100, 100);
        punch_list.Add(go = Instantiate(punch[1], new Vector3(bossPos.x - 20, 10, bossPos.z - 5), transform.rotation));
        go.GetComponent<Rocket_Punch>().Set(gameObject, 0, 100, 100);
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x + 3, 0, playerPos.z), target.transform.rotation));
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x - 3, 0, playerPos.z), target.transform.rotation));
        for (int i = 0; i < warning_list.Count; i++)
        {
            warning_list[i].transform.localScale = new Vector3(2, warning_list[i].transform.localScale.y, 2);
        }
    }
    private IEnumerator Shot_Punch()
    {
        for (int i = 0; i < punch_list.Count; i++)
        {
            Vector3 shotPos = warning_list[i].transform.position;
            Vector3 shotDirection = (new Vector3(shotPos.x, 0, shotPos.z) - punch_list[i].transform.position).normalized;
            float shotSpeed = 50f;
            if (i == 0) animator.SetTrigger("ArmFireL");
            else animator.SetTrigger("ArmFireR");
            punch_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        warning_list.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        weapon.OnWeapon(this);
        stateModifier.AddHandler(weapon.GetModifier());
        animator = GetComponent<Animator>();
        target = GameManager.Instance.Player.gameObject;
        maxHealthPoint.ResetState(30000);
        // maxProtectionPoint.ResetState(8000);
        healthPoint = maxHealthPoint;
        protectionPoint = maxProtectionPoint;
        Setup_Pillar();
        Setup_Razer();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isAlive)
        {
            BossAttack();
            if (isRazer)
            {
                Shot_Razer();
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}