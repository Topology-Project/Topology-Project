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

    public float playTime;

    private void Awake()
    {
        stageIdx = 0;
        stageNames = new string[]
        {
            "s1_start_1130",
            "s1_1106map", "s1_1106map", "s1_1106map", "s1_1106map",
            "Boss_map"
        };
        // SceneLoad("SampleScene");
    }

    private void Start()
    {
        NextStageLoad();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPlay) playTime += Time.fixedDeltaTime;
    }

    AsyncOperation op;
    public void SceneLoad(string sceneName, System.Action<AsyncOperation> action = null)
    {
        SceneManager.LoadScene("Loading");
        op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        GameManager.Instance.IsPlay = false;
        op.allowSceneActivation = false;
        if (action != null) op.completed += action;
        StartCoroutine(UnloadingScene());
    }
    IEnumerator UnloadingScene()
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
        if (mapManager != null) mapManager.EnterRoom();
    }

    // 다음 스테이지 로드 및 맵메니저 탐색
    public void NextStageLoad()
    {
        if (stageIdx < stageNames.Length)
        {
            mapManager = null;
            SceneLoad(stageNames[stageIdx++], (x) =>
            {
                mapManager = FindObjectOfType<MapManager>();
            });
        }
        else
        {
            stageIdx = 0;
            SceneLoad("Outro");
        }
    }
}
