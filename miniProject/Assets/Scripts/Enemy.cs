using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] public Transform target;
    NavMeshAgent nma;

    public float UpdateTime = 3f;
    private float LastUpdate;
    public float UpdateRange = 10f;

    private bool isFind = false;

    void MoveEnemy()
    {
        if (isFind == true)
        {
            // Debug.Log("나 거기로 간다 플레이어야");
            nma.SetDestination(target.position);
        }
        else
        {
            // Debug.Log("나 딴데로 간다");
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

        NavMeshHit hit;
        if (NavMesh.SamplePosition(RandomDirection, out hit, UpdateRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position;
        }
    }

    // 플레이어 찾기
    void FindTarget()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDir = transform.forward;
        float sphereRadius = 10f;
        float findRange = 45f;

        Quaternion leftRot = Quaternion.Euler(0, -findRange * 0.5f, 0);
        Vector3 leftDir = leftRot * rayDir;
        float leftRad = Mathf.Acos(Vector3.Dot(rayDir, leftDir));
        float leftDeg = -(Mathf.Rad2Deg * leftRad);

        Quaternion rightRot = Quaternion.Euler(0, findRange * 0.5f, 0);
        Vector3 rightDir = rightRot * rayDir;
        float rightRad = Mathf.Acos(Vector3.Dot(rayDir, rightDir));
        float rightDeg = Mathf.Rad2Deg * rightRad;



        // 레이저 쏴보기(씬에서만 보임)
        Debug.DrawRay(rayStart, rayDir * sphereRadius, Color.red);
        Debug.DrawRay(rayStart, leftDir * sphereRadius, Color.green);
        Debug.DrawRay(rayStart, rightDir * sphereRadius, Color.blue);

        // 에너미 중심 원형 범위 전개
        RaycastHit[] hits = Physics.SphereCastAll(rayStart, sphereRadius, rayDir, 0f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {

                // hit의 방향벡터값 계산
                Vector3 hitDir = (hit.transform.position - rayStart).normalized;
                float hitRad = Mathf.Acos(Vector3.Dot(rayDir, hitDir));
                float hitDeg = Mathf.Rad2Deg * hitRad;

                // Debug.Log(hitDeg + ", " + leftDeg + ", " + rightDeg);

                // 시야각 계산
                if (hitDeg >= leftDeg && hitDeg <= rightDeg)
                {
                    //Debug.Log("플레이어 찾았다 ^^");
                    isFind = true;
                    break;
                }
                else
                {
                    //Debug.Log("플레이어 범위 안엔 있는데 눈앞이 아님");
                    break;
                }
            }
            else
            {
                //Debug.Log("플레이어 어딨노");
                // isFind = false;
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        target = GameManager.Instance.Player.transform;
        nma = GetComponent<NavMeshAgent>();
        LastUpdate += UpdateTime;
    }


    // Update is called once per frame
    protected override void Update()
    {
        MoveEnemy();
        FindTarget();

        if(healthPoint <= 0)
        {
            GameManager.StageManager.mapManager.EnemyDeath();
            Destroy(gameObject);
        }
    }
}
