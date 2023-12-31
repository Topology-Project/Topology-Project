using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    private Animator animator;

    private float WaitT = 0f;
    private float MaxWaitT = 5f;

    private bool isAttack = true;

    public GameObject warning;
    private List<GameObject> warning_list = new List<GameObject>();

    public GameObject razer;
    private LineRenderer LR;
    private RaycastHit hit;
    private bool isRazer = false;

    public GameObject pillar;
    private List<GameObject> pillar_list = new List<GameObject>();

    public GameObject meteor;
    private List<GameObject> meteor_list = new List<GameObject>();

    public GameObject[] hand;
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
                    case "Razer":
                        StartCoroutine(Razer());
                        break;
                    case "Stone_Pillar":
                        StartCoroutine(Stone_Pillar());
                        break;
                    case "Meteor":
                        StartCoroutine(Meteor());
                        break;
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
    private void Shot_Razer()
    {
        LR.enabled = true;

        Vector3 razerPoint = razer.transform.position + Vector3.down;
        Vector3 endPoint = player.transform.position + (Vector3.down * 0.5f);

        if (Physics.Linecast(razerPoint, endPoint, out hit, layerMask))
        {
            // if (hit.transform.CompareTag("Pillar"))
            // {
            endPoint = hit.transform.position + Vector3.down;
            // }
        }

        LR.SetPosition(0, razerPoint);
        LR.SetPosition(1, endPoint);
    }
    private void Destroy_Razer()
    {
        Debug.Log("123");
        if (hit.transform.tag.Equals("Player"))
        {
            Debug.Log("1231321");
            hit.transform.GetComponent<Player>().DamageCalc(stateModifier);
        }
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("3909_Attack01 (1)") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
            {
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
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
        for (float x = 0; x <= 40; x += 5f)
        {
            for (float z = 0; z <= 25; z += 12.5f)
            {
                float randomX = Random.Range(-3.0f, 3.0f) - 30f;
                float randomZ = Random.Range(-2.0f, 2.0f) - 12.5f;

                if (x % 10 == 0 && z != 12.5f)
                {
                    warning_list.Add(Instantiate(warning, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 10 != 0 && z == 12.5f)
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
        for (float x = 0; x <= 40; x += 5f)
        {
            for (float z = 0; z <= 25; z += 12.5f)
            {
                float randomX = Random.Range(-3.0f, 3.0f) - 30f;
                float randomZ = Random.Range(-2.0f, 2.0f) - 12.5f;
                GameObject go = null;
                if (x % 10 == 0 && z != 12.5f)
                {
                    pillar_list.Add(go = Instantiate(pillar, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 10 != 0 && z == 12.5f)
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
            Vector3 shotDirection = (player.transform.position - meteor_list[i].transform.position).normalized;
            float shotSpeed = 40f;
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
        yield return new WaitForSecondsRealtime(1.5f);
        animator.SetTrigger("ArmFireEnd");
    }
    private void Ready_Punch()
    {
        Transform leftInfo = hand[0].transform;
        Transform rightInfo = hand[1].transform;
        Vector3 playerPos = player.transform.position;
        punch_list.Clear();
        GameObject go = null;
        punch_list.Add(go = Instantiate(punch[0], leftInfo.position, leftInfo.rotation));
        go.gameObject.SetActive(false);
        go.GetComponent<Rocket_Punch>().Set(gameObject, 0, 100, 100);
        punch_list.Add(go = Instantiate(punch[1], rightInfo.position, rightInfo.rotation));
        go.gameObject.SetActive(false);
        go.GetComponent<Rocket_Punch>().Set(gameObject, 0, 100, 100);
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x + 7, 0, playerPos.z), player.transform.rotation));
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x - 7, 0, playerPos.z), player.transform.rotation));
        for (int i = 0; i < warning_list.Count; i++)
        {
            warning_list[i].transform.localScale = new Vector3(3, warning_list[i].transform.localScale.y, 3);
        }
    }
    private IEnumerator Shot_Punch()
    {
        for (int i = 0; i < punch_list.Count; i++)
        {
            Vector3 shotPos = warning_list[i].transform.position;
            Vector3 shotDirection = (new Vector3(shotPos.x, 0, shotPos.z) - punch_list[i].transform.position).normalized;
            float shotSpeed = 75f;
            if (i == 0) animator.SetTrigger("ArmFireL");
            else animator.SetTrigger("ArmFireR");
            punch_list[i].gameObject.SetActive(true);
            punch_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        warning_list.Clear();
    }

    LayerMask layerMask;
    protected override void Awake()
    {
        base.Awake();
        layerMask = (1 << LayerMask.NameToLayer("BossObj")) |
                    (1 << LayerMask.NameToLayer("Player"));
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        weapon.OnWeapon(this);
        stateModifier.AddHandler(weapon.GetModifier());
        animator = GetComponent<Animator>();
        player = GameManager.Instance.Player.gameObject;
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