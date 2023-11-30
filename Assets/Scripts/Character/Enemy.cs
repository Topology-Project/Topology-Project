using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Character
{
    public GameObject damageText;
    [SerializeField] private GameObject player;
    protected NavMeshAgent nma;

    public float UpdateTime = 3f;
    private float LastUpdate;
    public float UpdateRange = 10f;

    private bool isFind = false;
    private bool isAtk = false;
    protected bool isAlive = true;
    public Image HP;

    private void MoveEnemy()
    {
        if (isFind)
        {
            // Debug.Log("플레이어에게 이동");
            nma.SetDestination(player.transform.position);
        }
        else
        {
            // Debug.Log("랜덤 위치로 이동");
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

        float dist = Vector3.Distance(player.transform.position, transform.position);

        // 레이저 디버그 (씬에서만 보임)
        Debug.DrawRay(rayStart, rayDir * sphereRadius, Color.red);
        Debug.DrawRay(rayStart, leftDir * sphereRadius, Color.green);
        Debug.DrawRay(rayStart, rightDir * sphereRadius, Color.blue);

        // Enemy 중심 원형 범위 전개
        RaycastHit[] hits = Physics.SphereCastAll(rayStart, sphereRadius, rayDir, 0f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                // hit의 방향벡터값 계산
                Vector3 hitDir = (hit.transform.position - rayStart).normalized;
                float hitRad = Mathf.Acos(Vector3.Dot(rayDir, hitDir));
                float hitDeg = Mathf.Rad2Deg * hitRad;

                // 범위 안에 플레이어가 있고, 공격 중이 아닐 때 플레이어를 바라보도록 설정
                if (!isAtk)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hitDir), Time.deltaTime * 2.5f);
                }

                // 플레이어가 적의 시야각 내부일 때
                if (hitDeg >= leftDeg && hitDeg <= rightDeg)
                {
                    // Debug.Log("플레이어 발견");
                    isFind = true;

                    RaycastHit hitObj;
                    if (Physics.Linecast(rayStart, hit.transform.position, out hitObj))
                    {
                        if (!hitObj.transform.CompareTag("Player"))
                        {
                            // 오브젝트
                            // Debug.Log("오브젝트에 가려짐, 플레이어에게 근접 접근 시도");
                            nma.stoppingDistance = 0;
                        }
                        else
                        {
                            // Debug.Log("플레이어가 바로 보임, 원거리 몹일 경우 값 증가시킬 것");
                            nma.stoppingDistance = 20f;

                            // 공격 코드
                            if (dist <= 3.5f && !isAtk)
                            {
                                StartCoroutine(Attack());
                            }
                        }
                        break;
                    }
                }
                else
                {
                    // Debug.Log("플레이어가 범위 안엔 있으나 눈 앞에 없음");
                }

            }
            else
            {
                // Debug.Log("플레이어를 찾을 수 없음.");
                if (dist >= sphereRadius) isFind = false;
            }
        }
    }

    // Enemy 공격 패턴
    IEnumerator Attack()
    {
        isAtk = true;
        Debug.Log("atk start");
        yield return new WaitForSecondsRealtime(3f);
        Fire1(transform);
        yield return new WaitForSecondsRealtime(0.2f);
        Fire1(transform);
        yield return new WaitForSecondsRealtime(0.2f);
        Fire1(transform);
        Debug.Log("atk end");
        isAtk = false;
    }

    public override float DamageCalc(StateModifier stateModifier)
    {
        if (isAlive)
        {
            float temp = base.DamageCalc(stateModifier);
            Transform cv = GetComponentInChildren<Canvas>().transform;
            GameObject go = Instantiate(damageText, transform.position + Vector3.up + transform.TransformDirection(Vector3.forward), Quaternion.identity, cv);
            go.GetComponent<DamageText>().SetDamageText((int)temp);
            return temp;
        }
        return 0;
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
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
        if (isAlive)
        {
            MoveEnemy();
            FindPlayer();
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag.Equals("Bullet"))
        {
            // 부모의 모디파이어 객체를 가져옴
            GameObject parent = other.GetComponent<Bullet>().parent;
            if (!parent.tag.Equals(gameObject.tag))
            {
                GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyHit);
                // HP.fillAmount = (float)healthPoint / (float)maxHealthPoint;
                // Debug.Log("fillamount : " + HP.fillAmount + "(" + healthPoint + " / " + maxHealthPoint + ")");
                Debug.Log("나 체력 준다");
                if (healthPoint <= 0 && isAlive)
                {
                    isAlive = false;
                    // Debug.Log("enemy die / " + isAlive);
                    GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.EnemyDie);
                    Destroy(gameObject, 1.2f);
                }
            }
        }
    }
}