using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private GameObject player;

    public GameObject warning;
    List<GameObject> warning_list = new List<GameObject>();

    public GameObject razer;
    private LineRenderer LR;

    public GameObject pillar;
    List<GameObject> pillar_list = new List<GameObject>();

    public GameObject meteor;
    List<GameObject> meteor_list = new List<GameObject>();

    public GameObject punch;
    List<GameObject> punch_list = new List<GameObject>();

    Animator animator;

    float WaitT = 0f;
    float MaxWaitT = 8f;

    public bool isAtk = true;
    bool isRazer = false;

    // 공격 시작 메소드
    void Attack()
    {
        if (!isAtk && animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            switch (GetRandomPattern())
            {
                case "Razer":
                    StartCoroutine(Razer());
                    break;
                case "Stone_Pillar":
                    // StartCoroutine(Stone_Pillar());
                    break;
                case "Meteor":
                    // StartCoroutine(Meteor());
                    break;
                case "Rocket_Punch":
                    // StartCoroutine(Rocket_Punch());
                    break;
            }
        }
        else
        {
            Waiting();
        }
    }
    // 대기 메소드
    public void Waiting()
    {
        Debug.Log("Wait Boss");


        if (WaitT < MaxWaitT)
        {
            WaitT += Time.deltaTime;
        }
        else
        {
            WaitT = 0f;
            isAtk = false;
        }
    }

    // 랜덤 패턴 값 가져오기
    string GetRandomPattern()
    {
        string[] boss_Patterns = { "Razer", "Stone_Pillar", "Meteor", "Rocket_Punch" };
        int value = Random.Range(0, boss_Patterns.Length);
        return boss_Patterns[value];
    }

    // 레이저 패턴
    IEnumerator Razer()
    {
        isAtk = true;
        Debug.Log("Razer Pattern");
        // 레이저 준비
        yield return new WaitForSecondsRealtime(1f);
        isRazer = true;
        Debug.Log(isRazer);
        yield return new WaitForSecondsRealtime(2.0f);
        isRazer = false;
        Debug.Log(isRazer);
    }
    void Setting_Razer()
    {
        LR = razer.GetComponent<LineRenderer>();
        LR.positionCount = 2;
        LR.startWidth = 1f;
        LR.endWidth = 1f;
    }
    void Shot_Razer()
    {
        LR.enabled = true;

        RaycastHit hit;

        Vector3 startPos = razer.transform.position;
        // Vector3 endPos = player.transform.position + Vector3.down;
        Vector3 endPos {

        };

        LR.SetPosition(0, startPos);
        LR.SetPosition(1, endPos);
    }
    void Destroy_Razer()
    {
        LR.enabled = false;
    }

    // 돌기둥 패턴
    IEnumerator Stone_Pillar()
    {
        // Debug.Log("Stone Pillar Pattern");
        isAtk = true;
        animator.SetBool("isSlam", true);
        Warning_Destroy_Pillar();
        yield return new WaitForSecondsRealtime(1.4f);
        Destroy_Pillar();
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("isSlam", false);
        animator.SetBool("isSummon", true);
        Warning_Build_Pillar();
        yield return new WaitForSecondsRealtime(1.0f);
        Build_Pillar();
        yield return new WaitForSecondsRealtime(2.0f);
        animator.SetBool("isSummon", false);
    }

    void Warning_Build_Pillar()
    {
        // Debug.Log("Warning_Build_Pillar");
        for (float x = 0; x <= 75; x += 7.5f)
        {
            for (float z = 0; z <= 20; z += 10)
            {
                float randomX = Random.Range(-3.0f, 3.0f) - 32.5f;
                float randomZ = Random.Range(-3.0f, 3.0f) - 35;

                if (x % 15 == 0 && z != 10)
                {
                    warning_list.Add(Instantiate(warning, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 15 == 7.5f && z == 10)
                {
                    warning_list.Add(Instantiate(warning, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
            }
        }
    }
    void Build_Pillar()
    {
        // Debug.Log("Build_Pillar");
        for (int i = 0; i < warning_list.Count; i++)
        {
            pillar_list.Add(Instantiate(pillar, warning_list[i].transform.position, warning_list[i].transform.rotation));
            Destroy(warning_list[i]);
        }
        warning_list.Clear();
    }
    void Warning_Destroy_Pillar()
    {
        // Debug.Log("Warning_Destroy_Pillar");
        for (int i = 0; i < pillar_list.Count; i++)
        {
            warning_list.Add(Instantiate(warning, pillar_list[i].transform.position, pillar_list[i].transform.rotation));
            if (!pillar_list[i].activeSelf)
            {
                warning_list[i].SetActive(false);
            }
        }
    }
    void Destroy_Pillar()
    {
        // Debug.Log("Destroy_Pillar");
        for (int i = 0; i < pillar_list.Count; i++)
        {
            Destroy(pillar_list[i]);
            Destroy(warning_list[i]);
        }
        pillar_list.Clear();
        warning_list.Clear();
    }
    void Start_Pillar()
    {
        for (float x = 0; x <= 75; x += 7.5f)
        {
            for (float z = 0; z <= 20; z += 10)
            {
                float randomX = Random.Range(-3.0f, 3.0f) - 32.5f;
                float randomZ = Random.Range(-3.0f, 3.0f) - 35;

                if (x % 15 == 0 && z != 10)
                {
                    pillar_list.Add(Instantiate(pillar, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
                else if (x % 15 == 7.5f && z == 10)
                {
                    pillar_list.Add(Instantiate(pillar, new Vector3(x + randomX, 0, z + randomZ), transform.rotation));
                }
            }
        }
    }

    // 메테오 패턴
    IEnumerator Meteor()
    {
        isAtk = true;
        // Debug.Log("Meteor Pattern");
        animator.SetBool("isMeteor", true);
        Spawn_Meteor();
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(Drop_Meteor());
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("isMeteor", false);
        yield return new WaitForSecondsRealtime(1.5f);
    }
    void Spawn_Meteor()
    {
        meteor_list.Clear();

        for (int i = 0; i <= 40; i += 10)
        {
            float randomX = Random.Range(-3.0f, 3.0f) - 20;
            float randomY = Random.Range(25f, 30f);
            meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(randomX + i, randomY, 0), gameObject.transform.rotation));
        }
    }
    IEnumerator Drop_Meteor()
    {
        for (int i = 0; i < meteor_list.Count; i++)
        {
            Vector3 shotDirection = (player.transform.position - meteor_list[i].transform.position).normalized;
            float shotSpeed = 20f;
            meteor_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }

    }

    // 로켓펀치 패턴
    IEnumerator Rocket_Punch()
    {
        isAtk = true;
        // Debug.Log("Rocket Punch Pattern");
        Ready_Punch();
        yield return new WaitForSecondsRealtime(2.0f);
        StartCoroutine(Shot_Punch());
        yield return new WaitForSecondsRealtime(2.0f);
    }
    void Ready_Punch()
    {
        Vector3 bossPos = transform.position;
        Vector3 playerPos = player.transform.position;
        punch_list.Clear();
        punch_list.Add(Instantiate(punch, new Vector3(bossPos.x + 20, 10, bossPos.z - 5), transform.rotation));
        punch_list.Add(Instantiate(punch, new Vector3(bossPos.x - 20, 10, bossPos.z - 5), transform.rotation));
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x + 3, 0, playerPos.z), player.transform.rotation));
        warning_list.Add(Instantiate(warning, new Vector3(playerPos.x - 3, 0, playerPos.z), player.transform.rotation));
        for (int i = 0; i < warning_list.Count; i++)
        {
            warning_list[i].transform.localScale = new Vector3(2, warning_list[i].transform.localScale.y, 2);
        }
    }
    IEnumerator Shot_Punch()
    {
        for (int i = 0; i < punch_list.Count; i++)
        {
            Vector3 shotPos = warning_list[i].transform.position;
            Vector3 shotDirection = (new Vector3(shotPos.x, 0, shotPos.z) - punch_list[i].transform.position).normalized;
            float shotSpeed = 50f;
            punch_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        warning_list.Clear();
    }


    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.Player.gameObject;
        Start_Pillar();
        Setting_Razer();
    }

    // Update is called once per frame
    public void Update()
    {
        Attack();
        if (isRazer)
        {
            Debug.Log("sssss");
            Shot_Razer();
        }
        else
        {
            Destroy_Razer();
        }
    }
}