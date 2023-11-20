using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private GameObject player;
    private NavMeshAgent nma;

    public float UpdateTime = 3f;
    private float LastUpdate;
    public float UpdateRange = 10f;

    private bool isFind = false;

    private void MoveEnemy()
    {
        if (isFind == true)
        {
            Debug.Log("플레이어에게 이동");
            nma.SetDestination(player.transform.position);
        }
        else
        {
            Debug.Log("랜덤 위치로 이동");
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

    private Vector3 GetRandomPosOnNM()
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
    private void FindPlayer()
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
            Debug.Log("foreach");
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("first if");
                // hit의 방향벡터값 계산
                Vector3 hitDir = (hit.transform.position - rayStart).normalized;
                float hitRad = Mathf.Acos(Vector3.Dot(rayDir, hitDir));
                float hitDeg = Mathf.Rad2Deg * hitRad;

                // Debug.Log(hitDeg + ", " + leftDeg + ", " + rightDeg);

                // 시야각 계산
                if (hitDeg >= leftDeg && hitDeg <= rightDeg)
                {
                    Debug.Log("플레이어 발견");
                    isFind = true;

                    RaycastHit hitObj;
                    if (Physics.Raycast(rayStart, hitDir, out hitObj, 10f))
                    {
                        if (!hitObj.transform.CompareTag("Player"))
                        {
                            Debug.Log("오브젝트에 가려짐, 플레이어에게 근접 접근 시도");
                            nma.stoppingDistance = 2f;
                        }
                        else
                        {
                            Debug.Log("플레이어가 바로 보임, 원거리 몹일 경우 값 증가시킬 것");
                            nma.stoppingDistance = 2f;
                        }
                        break;
                    }
                }
                else
                {
                    Debug.Log("플레이어가 범위 안엔 있으나 눈 앞에 없음");
                    isFind = false;
                }
            }
            else
            {
                Debug.Log("플레이어를 찾을 수 없음.");
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        player = GameManager.Instance.Player.gameObject;
        nma = GetComponent<NavMeshAgent>();
        nma.stoppingDistance = 2f;
        LastUpdate += UpdateTime;
    }


    // Update is called once per frame
    protected override void Update()
    {
        MoveEnemy();
        FindPlayer();

        if (healthPoint <= 0)
        {
            // GameManager.Instance.StageManager.mapManager.EnemyDeath();
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider obj)
    {
        base.OnTriggerEnter(obj);
        if (obj.gameObject.CompareTag("Bullet"))
        {
            GameManager.Instance.TriggerManager.onTrigger(PlayTriggerType.EnemyHit);
        }
    }
}
