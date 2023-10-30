using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject warning;
    public GameObject pillar;
    List<GameObject> warning_list = new List<GameObject>();
    List<GameObject> pillar_list = new List<GameObject>();

    public GameObject meteor;
    List<GameObject> meteor_list = new List<GameObject>();

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
            Spawn_Meteor();
            StartCoroutine(Stone_Pillar());

            // switch (GetRandomPattern())
            // {
            //     case "Razer":
            //         StartCoroutine(Razer());
            //         break;
            //     case "Stone_Pillar":
            //         StartCoroutine(Stone_Pillar());
            //         break;
            //     case "Meteor":
            //         StartCoroutine(Meteor());
            //         break;
            //     case "Rocket_Punch":
            //         StartCoroutine(Rocket_Punch());
            //         break;
            // }
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
        for (float x = 0; x <= 60; x += 7.5f)
        {
            for (float z = 0; z <= 20; z += 10)
            {
                float randomX = Random.Range(-3.0f, 3.0f) - 30;
                float randomZ = Random.Range(-3.0f, 3.0f) - 25;

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
    }
    void Warning_Destroy_Pillar()
    {
        Debug.Log("Warning_Destroy_Pillar");
        for (int i = 0; i < pillar_list.Count; i++)
        {
            warning_list.Add(Instantiate(warning, pillar_list[i].transform.position, pillar_list[i].transform.rotation));
        }
    }
    void Destroy_Pillar()
    {
        Debug.Log("Destroy_Pillar");
        for (int i = 0; i < pillar_list.Count; i++)
        {
            Destroy(pillar_list[i]);
        }
        for (int i = 0; i < warning_list.Count; i++)
        {
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
        Spawn_Meteor();
        yield return new WaitForSecondsRealtime(3.0f);
        Drop_Meteor();
        StartCoroutine(Waiting());
    }
    void Spawn_Meteor()
    {
        meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(-20, 20, 0), gameObject.transform.rotation));
        meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(-10, 22.5f, 0), gameObject.transform.rotation));
        meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(0, 25, 0), gameObject.transform.rotation));
        meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(10, 22.5f, 0), gameObject.transform.rotation));
        meteor_list.Add(Instantiate(meteor, gameObject.transform.position + new Vector3(20, 20, 0), gameObject.transform.rotation));
    }
    void Drop_Meteor()
    {

    }

    // 로켓펀치 패턴
    IEnumerator Rocket_Punch()
    {
        isAtk = true;
        Debug.Log("Rocket Punch Pattern");
        yield return new WaitForSecondsRealtime(3.0f);
        StartCoroutine(Waiting());
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
