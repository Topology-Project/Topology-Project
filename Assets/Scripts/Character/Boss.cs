using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject player;

    public GameObject warning;
    public GameObject pillar;
    List<GameObject> warning_list = new List<GameObject>();
    List<GameObject> pillar_list = new List<GameObject>();

    public GameObject meteor;
    List<GameObject> meteor_list = new List<GameObject>();

    public GameObject rocket_punch;
    List<GameObject> punch_list = new List<GameObject>();

    Animator animator;

    public bool isAtk = false;

    // 대기 모션 코루틴
    IEnumerator Waiting()
    {
        Debug.Log("Wait Boss");
        yield return new WaitForSeconds(0.5f);
        if (isAtk == true) isAtk = false;
    }

    // 공격 시작 메소드
    void Attack()
    {
        if (!isAtk)
        {
            switch (GetRandomPattern())
            {
                case "Razer":
                    // StartCoroutine(Razer());
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
        yield return new WaitForSecondsRealtime(3.0f);
        StartCoroutine(Waiting());
    }

    // 돌기둥 패턴
    IEnumerator Stone_Pillar()
    {
        Debug.Log("Stone Pillar Pattern");
        isAtk = true;
        animator.SetBool("isSlam", true);
        Warning_Destroy_Pillar();
        yield return new WaitForSeconds(1.4f);
        Destroy_Pillar();
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isSlam", false);
        animator.SetBool("isSummon", true);
        Warning_Build_Pillar();
        yield return new WaitForSeconds(1.0f);
        Build_Pillar();
        yield return new WaitForSeconds(2.0f);
        animator.SetBool("isSummon", false);
        StartCoroutine(Waiting());
    }

    void Warning_Build_Pillar()
    {
        Debug.Log("Warning_Build_Pillar");
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
        Debug.Log("Build_Pillar");
        for (int i = 0; i < warning_list.Count; i++)
        {
            pillar_list.Add(Instantiate(pillar, warning_list[i].transform.position, warning_list[i].transform.rotation));
            Destroy(warning_list[i]);
        }
        warning_list.Clear();
    }
    void Warning_Destroy_Pillar()
    {
        Debug.Log("Warning_Destroy_Pillar");
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
        Debug.Log("Destroy_Pillar");
        for (int i = 0; i < pillar_list.Count; i++)
        {
            Destroy(pillar_list[i]);
            Destroy(warning_list[i]);
        }
        pillar_list.Clear();
        warning_list.Clear();
    }

    // 메테오 패턴
    IEnumerator Meteor()
    {
        isAtk = true;
        Debug.Log("Meteor Pattern");
        animator.SetBool("isMeteor", true);
        Spawn_Meteor();
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(Drop_Meteor());
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("isMeteor", false);
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(Waiting());
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
        Debug.Log("Rocket Punch Pattern");
        Ready_Punch();
        yield return new WaitForSecondsRealtime(2.0f);
        StartCoroutine(Shot_Punch());
        yield return new WaitForSecondsRealtime(2.0f);
        StartCoroutine(Waiting());
    }
    void Ready_Punch()
    {
        Vector3 bossPos = transform.position;
        Vector3 playerPos = player.transform.position;
        punch_list.Clear();
        punch_list.Add(Instantiate(rocket_punch, new Vector3(bossPos.x + 20, 10, bossPos.z - 5), transform.rotation));
        punch_list.Add(Instantiate(rocket_punch, new Vector3(bossPos.x - 20, 10, bossPos.z - 5), transform.rotation));
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
            Vector3 shotDirection = (player.transform.position - punch_list[i].transform.position).normalized;
            float shotSpeed = 50f;
            punch_list[i].GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < warning_list.Count; i++)
        {
            Destroy(warning_list[i]);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        warning_list.Clear();
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtk == false)
        {
            Attack();
        }
    }
}
