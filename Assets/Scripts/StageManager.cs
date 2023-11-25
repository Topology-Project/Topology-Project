using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private string[] stageNames;
    private int stageIdx;

    public MapManager mapManager { get; private set; }

    private void Awake()
    {
        stageIdx = 0;
        stageNames = new string[]
        {
            // "s1_1105map", "s1_1105map", "s1_1105map",
            "Boss_map"
        };
    }

    private void Start()
    {
        SceneLoad("SampleScene");
    }

    AsyncOperation op;
    public void SceneLoad(string sceneName, System.Action<AsyncOperation> action = null)
    {
        SceneManager.LoadScene("Loading");
        op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        if(action != null) op.completed += action;
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
        op.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync("Loading");
        if(mapManager != null) mapManager.EnterRoom();
    }
    
    // 다음 스테이지 로드 및 맵메니저 탐색
    public void NextStageLoad()
    {
        if (stageIdx < stageNames.Length)
        {
            mapManager = null;
            SceneLoad(stageNames[stageIdx++], (x) => {
                mapManager = FindObjectOfType<MapManager>();
            });
        }
        else stageIdx = 0;
    }
}
