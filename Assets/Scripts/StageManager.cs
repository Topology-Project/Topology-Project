using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private string[] stageNames;
    private int stageIdx;
    public int StageIdx { get { return stageIdx; } }

    public MapManager mapManager { get; private set; }

    public bool gameClear;
    public float playTime;
    private float maxDamage;
    public float MaxDamage
    {
        set
        {
            if (maxDamage < value) maxDamage = value;
        }
        get { return MaxDamage; }
    }

    private void Awake()
    {
        stageIdx = 0;
        stageNames = new string[]
        {
            "s1_start_1130",
            // "s1_1106map", "s1_1106map", "s1_1106map", "s1_1106map",
            "Boss_Golem 1"

        };
    }

    private void Start()
    {
        gameClear = false;
        playTime = 0;
        maxDamage = 0;
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.PlayerDie, LoadOutro);
        NextStageLoad();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPlay) playTime += Time.fixedDeltaTime;
    }

    public void SceneLoad(string sceneName, System.Action<AsyncOperation> action = null)
    {
        SceneManager.LoadScene("Loading");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        GameManager.Instance.IsPlay = false;
        op.allowSceneActivation = false;
        if (action != null) op.completed += action;
        StartCoroutine(UnloadingScene(op));
    }
    IEnumerator UnloadingScene(AsyncOperation op)
    {
        yield return null;
        // while(!op.isDone)
        // {
        //     // 로드 중...
        // }
        yield return new WaitForSecondsRealtime(2f);
        GameManager.Instance.IsPlay = true;
        op.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync("Loading");
        if (mapManager != null)
        {
            mapManager.EnterRoom();
            stageIdx++;
        }
    }

    // 다음 스테이지 로드 및 맵메니저 탐색
    public void NextStageLoad()
    {
        if (stageIdx < stageNames.Length)
        {
            mapManager = null;
            
            SceneLoad(stageNames[stageIdx], (x) => {
                mapManager = FindObjectOfType<MapManager>();
            });
        }
        else
        {
            stageIdx = 0;
            gameClear = true;
            LoadOutro();
        }
    }

    private void LoadOutro() => SceneLoad("Outro");
}
