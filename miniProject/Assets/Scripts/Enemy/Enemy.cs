using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform target;
    NavMeshAgent nma;

    public float UpdateTime = 3f;
    private float LastUpdate;
    public float UpdateRange = 10f;

    private bool isFind = false;

    void Move()
    {
        if (isFind == true)
        {
            Debug.Log("나 거기로 간다 플레이어야");
            nma.SetDestination(target.position);
        }
        else
        {
            Debug.Log("나 딴데로 간다");
            LastUpdate += Time.deltaTime;
            if (LastUpdate >= UpdateTime)
            {
                nma.SetDestination(transform.position);
                Vector3 RandomPos = GetRandomPosOnNM();
                nma.SetDestination(RandomPos);
                LastUpdate = 0;
            }
        }
    }

    Vector3 GetRandomPosOnNM()
    {
        Vector3 RandomDirection = Random.insideUnitSphere * UpdateRange;
        RandomDirection += transform.position;

        NavMeshHit NMHit;
        if (NavMesh.SamplePosition(RandomDirection, out NMHit, UpdateRange, NavMesh.AllAreas))
        {
            return NMHit.position;
        }
        else
        {
            return transform.position;
        }
    }

    // 플레이어 찾기
    void FindTarget()
    {
        // 레이저 쏴보기(씬에서만 보임)
        Debug.DrawRay(transform.position + (Vector3.up / 2), transform.forward * 30f, Color.red);

        // 레이저 맞추고 맞춘 오브젝트 뭔지말하기
        RaycastHit RCHit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out RCHit, 30f))
        {
            if (RCHit.transform.CompareTag("Player"))
            {
                Debug.Log("플레이어 찾았다 ^^");
                isFind = true;
            }
            else
            {
                Debug.Log("플레이어 어딨노");
                isFind = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        LastUpdate += UpdateTime;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        FindTarget();
    }
}