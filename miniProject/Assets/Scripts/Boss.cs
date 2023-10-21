using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 임시 작성 스크립트
// Coroutine 학습 후 코드 전반적 교체 예정
public class Boss : MonoBehaviour
{
    string[] boss_Patterns = { "Razer", "Stone_pillar", "Meteor", "Rocket_Punch" };

    float T = 0f;
    float T_Max = 3f;

    float patternT = 0f;
    float patternT_Max = 1f;

    bool isWait = true;
    bool isAtk = false;

    void Waiting()
    {
        Debug.Log("Start Wait");
        T += Time.deltaTime;

        // 대기모션

        if (T >= T_Max)
        {
            T = 0f;
            isWait = false;
            Debug.Log("Stop Wait");
        }
    }

    void P_start()
    {
        for (int i = 0; i < boss_Patterns.Length; i++)
        {
            int randomP_num = Random.Range(0, boss_Patterns.Length);
            if (boss_Patterns[randomP_num] == "Razer")
            {
                Pattern();
                isAtk = true;
                break;
            }
            else if (boss_Patterns[randomP_num] == "Stone_pillar")
            {
                Pattern();
                isAtk = true;
                break;
            }
            else if (boss_Patterns[randomP_num] == "Meteor")
            {
                Pattern();
                isAtk = true;
                break;
            }
            else if (boss_Patterns[randomP_num] == "Rocket_Punch")
            {
                Pattern();
                isAtk = true;
                break;
            }
        }
    }

    void Pattern()
    {
        Debug.Log("Start Atk");
        patternT += Time.deltaTime;

        // 대기모션

        if (patternT >= patternT_Max)
        {
            patternT = 0f;
            isAtk = false;
            isWait = true;
            Debug.Log("Stop Atk");
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAtk == false)
        {
            if (isWait == true)
            {
                Waiting();
            }
            else
            {
                P_start();
            }
        }

    }

}
