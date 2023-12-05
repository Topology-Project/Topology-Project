using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private string[] stageNames; // 각 스테이지의 씬 이름을 저장하는 배열
    private int stageIdx; // 현재 진행 중인 스테이지의 인덱스
    public int StageIdx { get { return stageIdx; } } // 현재 스테이지의 인덱스를 외부에서 읽을 수 있도록 하는 속성

    public MapManager mapManager { get; private set; } // 현재 스테이지의 맵 매니저

    public bool gameClear; // 게임 클리어 여부
    public float playTime; // 플레이 타임
    private float maxDamage; // 최대 데미지
    public float MaxDamage
    {
        set
        {
            if (maxDamage < value) maxDamage = value;
        }
        get { return maxDamage; }
    }

    private void Awake()
    {
        stageIdx = 0;
        // 각 스테이지의 씬 이름을 초기화
        stageNames = new string[]
        {
            "s1_start_1130",
            //"Stage_1_map", "Stage_1_map", "Stage_1_map", 
            "Boss_Golem 1"
        };
    }

    private void Start()
    {
        gameClear = false;
        playTime = 0;
        maxDamage = 0;
        // 플레이어가 사망했을 때 Outro 씬으로 이동하는 트리거 등록
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.PlayerDie, LoadOutro);
        // 다음 스테이지 로드
        NextStageLoad();
    }
    private void FixedUpdate()
    {
        // 게임 플레이 중일 때 플레이 타임 증가
        if (GameManager.Instance.IsPlay) playTime += Time.fixedDeltaTime;
    }

    // 씬을 로드하는 메서드
    public void SceneLoad(string sceneName, System.Action<AsyncOperation> action = null)
    {
        // Loading 씬을 로드하고, 추가적인 로드 작업을 수행하는 비동기 작업을 반환
        SceneManager.LoadScene("Loading");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        GameManager.Instance.IsPlay = false; // 게임 플레이 중이 아님을 표시
        op.allowSceneActivation = false; // 씬을 활성화하지 않음

        // 완료된 경우 추가적인 액션 수행
        if (action != null) op.completed += action;

        // 로딩이 끝나면 씬 언로드 및 게임 플레이 상태로 전환
        StartCoroutine(UnloadingScene(op));
    }
    IEnumerator UnloadingScene(AsyncOperation op)
    {
        yield return null;
        // while(!op.isDone)
        // {
        //     // 로드 중...
        // }

        // 로딩 완료후 추가 대기
        yield return new WaitForSecondsRealtime(0);

        // 게임 플레이 중으로 전환하고 Loading 씬을 언로드
        GameManager.Instance.IsPlay = true;
        op.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync("Loading");

        // 현재 맵 매니저가 존재하면 방 진입 및 스테이지 인덱스 증가
        if (mapManager != null)
        {
            mapManager.EnterRoom();
            stageIdx++;
        }
    }

    // 다음 스테이지 로드 및 맵 메니저 찾기
    public void NextStageLoad()
    {
        // 클리어할 스테이지가 남은 경우
        if (stageIdx < stageNames.Length)
        {
            mapManager = null;

            // 다음 스테이지 씬을 로드하고, 맵 매니저를 찾는 콜백 등록
            SceneLoad(stageNames[stageIdx], (x) =>
            {
                mapManager = FindObjectOfType<MapManager>();
            });
        }
        // 모든 스테이지를 클리어한 경우, 게임 클리어 플래그를 설정하고 Outro 씬 로드
        else
        {
            stageIdx = 0;
            gameClear = true;
            LoadOutro();
        }
    }

    // Outro 씬 로드
    private void LoadOutro() => SceneLoad("Outro");
}
