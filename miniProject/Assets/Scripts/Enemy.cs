using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 기능 쓰려면 넣어야 함
using UnityEngine.AI;

public class Enemy : Character
{
    // ---------- 변수 정의 ---------- //

    [SerializeField] public Transform target;
    NavMeshAgent nma;

    // AI 랜덤위치 이동 관련 변수값
    public float UpdateTime = 3f;
    private float LastUpdate;
    public float UpdateRange = 10f;

    // 플레이어 감지 여부
    private bool isFind = false;

    // 근거리몹 플레이어 접근거리 (임시)
    private float MeleeSD = 2f;
    // 원거리몹 플레이어 접근거리 (임시)
    private float RangedSD = 10f;

    // ---------- 메소드 ---------- //

    // Enemy 이동 코드 ( 플레이어 및 랜덤위치 )
    void MoveEnemy()
    {
        if (isFind == true)
        {
            // 플레이어의 좌표값으로 이동
            // Debug.Log("플레이어에게 이동");
            nma.SetDestination(target.position);
            // 랜덤좌표 이동의 원활한 진행을 위한 시간 초기화
            LastUpdate = 0;
        }
        else
        {
            // 랜덤 위치 선정 후 해당 좌표값으로 이동
            // Debug.Log("랜덤 위치로 이동");

            LastUpdate += Time.deltaTime;
            if (LastUpdate >= UpdateTime)
            {
                // 우선 제자리에 멈춘 다음, (해당 오브젝트의 위치값)
                nma.SetDestination(transform.position);
                // 아래에 있는 랜덤좌표값 얻어오기 메소드를 활용하여 RandomPos 값 획득
                Vector3 RandomPos = GetRandomPosOnNM();
                // RandomPos 로 이동
                nma.SetDestination(RandomPos);
                // 시간 초기화
                LastUpdate = 0;
            }
        }
    }

    // Get Random Position On Nav Mesh
    // Nav Mesh 위에 있는 랜덤 좌표값을 얻는 메소드
    Vector3 GetRandomPosOnNM()
    {
        // Random.insideUnitSphere = 반경 1을 가진 구 안의 임시 지점 반환, 변수값을 곱하여 반경 확대
        // 원하는 범위 내의 랜덤한 방향 벡터 생성
        Vector3 RandomDirection = Random.insideUnitSphere * UpdateRange;
        // 랜덤 방향 벡터에 현재 위치를 더함.
        RandomDirection += transform.position;

        NavMeshHit hit;
        // 랜덤 위치가 Nav Mesh 위에 있는지 확인
        if (NavMesh.SamplePosition(RandomDirection, out hit, UpdateRange, NavMesh.AllAreas))
        {
            // 있을 경우, Nav Mesh 위 랜덤 위치값 반환
            return hit.position;
        }
        else
        {
            // Nav Mesh 위 랜덤 위치를 찾지 못한 경우 현재 위치값 반환
            // Navigation Bake 후 Nav Obstacle를 통해 막거나 Area 설정으로 AI가 못가는 영역 생성 후, 그 위치로 랜덤 위치가 찍힐 경우의 상황
            return transform.position;
        }
    }

    // 플레이어 찾기
    void FindTarget()
    {
        // Enemy의 위치, 정면
        Vector3 rayStart = transform.position;
        Vector3 rayDir = transform.forward;
        // Enemy가 인식할 범위
        float sphereRadius = 10f;
        // 시야각 구현을 위한 값
        float findRange = 45f;

        // rayDir (중심) 기준 왼쪽 최대 시야각 (수치상 최소값)
        Quaternion leftRot = Quaternion.Euler(0, -findRange * 0.5f, 0);
        Vector3 leftDir = leftRot * rayDir;
        float leftRad = Mathf.Acos(Vector3.Dot(rayDir, leftDir));
        float leftDeg = -(Mathf.Rad2Deg * leftRad);

        // rayDir (중심) 기준 오른쪽 최대 시야각 (수치상 최대값)
        Quaternion rightRot = Quaternion.Euler(0, findRange * 0.5f, 0);
        Vector3 rightDir = rightRot * rayDir;
        float rightRad = Mathf.Acos(Vector3.Dot(rayDir, rightDir));
        float rightDeg = Mathf.Rad2Deg * rightRad;

        // 시야각 확인을 위한 레이저 쏴보기 (씬에서 확인, 추후 주석처리 or 삭제)
        Debug.DrawRay(rayStart, rayDir * sphereRadius, Color.red);
        Debug.DrawRay(rayStart, leftDir * sphereRadius, Color.green);
        Debug.DrawRay(rayStart, rightDir * sphereRadius, Color.blue);

        // Enemy 중심 원형 범위 전개, 해당 범위 내의 모든 오브젝트를 배열에 저장
        RaycastHit[] hits = Physics.SphereCastAll(rayStart, sphereRadius, rayDir, 0f);

        // 각 오브젝트마다 확인 진행
        foreach (RaycastHit hit in hits)
        {
            // 플레이어를 발견했을 경우
            if (hit.transform.CompareTag("Player"))
            {
                // hit의 방향벡터값 계산
                Vector3 hitDir = (hit.transform.position - rayStart).normalized;
                float hitRad = Mathf.Acos(Vector3.Dot(rayDir, hitDir));
                float hitDeg = Mathf.Rad2Deg * hitRad;

                // hit이 시야각 안에 들어왔는지 확인 (콘솔창에서 확인, 추후 주석처리 or 삭제)
                // Debug.Log(hitDeg + ", " + leftDeg + ", " + rightDeg);

                // 시야각 계산, 왼쪽 한계보다 크고 오른쪽 한계보다 작을때 if문 실행
                if (hitDeg >= leftDeg && hitDeg <= rightDeg)
                {
                    // 플레이어 감지 여부 true값 변경
                    // Debug.Log("플레이어 발견");
                    isFind = true;

                    // Enemy가 Player를 바라볼 때, 오브젝트에 의해 가려진 상태인지 확인
                    RaycastHit hitObj;
                    if (Physics.Raycast(rayStart, hitDir, out hitObj, 10f))
                    {
                        // 플레이어가 아닐때 (다른 오브젝트로 플레이어가 가린 상황)
                        if (!hitObj.transform.CompareTag("Player"))
                        {
                            // 장애물을 AI를 통해 피하고 플레이어를 바라볼 수 있게끔 Stopping Distance값을 0으로 설정 (임시)
                            // Debug.Log("오브젝트에 가려짐, 플레이어에게 근접 접근 시도");
                            nma.stoppingDistance = 0f;
                            // 접근 중 Player를 지속적으로 바라볼 수 있게끔하여 코드가 계속 작동하도록 함
                            transform.Rotate(transform.rotation.x, hitDeg, transform.rotation.z);
                        }
                        else
                        {
                            // 오브젝트에 가려지지 않았을 때 Stopping Distance 값을 정상으로 되돌림
                            // 근거리몹 수치 : MeleeSD = 2f, 원거리몹 수치 RangedSD = 10f (스크립트 상단 변수정의 참고)
                            // Debug.Log("플레이어가 바로 보임");
                            nma.stoppingDistance = MeleeSD;
                        }
                    }
                }
                else
                {
                    // Player가 Enemy 주변 범위에는 감지되었으나, 시야각 내부가 아닐 떄
                    // Debug.Log("플레이어가 범위 안엔 있으나 눈 앞에 없음");
                    // isFind = false;
                }
            }
            else
            {
                // Player가 Enemy 감지 범위 밖일 때
                // Debug.Log("플레이어를 찾을 수 없음.");
                // isFind = false;
            }
        }
    }

    // ---------- Unity 이벤트 메소드 호출 ---------- //

    protected override void Start()
    {
        nma = GetComponent<NavMeshAgent>();

        // 해당 Enemy의 Type에 따라 변경 (현재 근거리몹 기준, 상단 변수 참고)
        nma.stoppingDistance = MeleeSD;

        // 랜덤 좌표를 시작과 동시에 찾도록 설정
        LastUpdate += UpdateTime;
    }

    void Update()
    {
        MoveEnemy();
        FindTarget();
    }
}
