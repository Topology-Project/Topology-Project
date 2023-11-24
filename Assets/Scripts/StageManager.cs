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

    // 다음 스테이지 로드 및 맵메니저 탐색
    public void NextStageLoad()
    {
        // if(stageIdx < stageNames.Length) SceneManager.LoadScene(stageNames[stageIdx++]);
        // GameObject go = GameObject.Find("MapManager");
        if (stageIdx < stageNames.Length)
        {
            var op = SceneManager.LoadSceneAsync(stageNames[stageIdx++]);
            op.completed += (x) =>
            {
                mapManager = FindObjectOfType<MapManager>();
            };
        }
    }
}
