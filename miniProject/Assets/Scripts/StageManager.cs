using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private string[] stageNames;
    private int stageIdx;

    public MapManager mapManager;

    private void Awake()
    {
        stageIdx = 0;
        stageNames = new string[]
        {
            "s1_map", "s1_map", "s1_map"
        };
    }

    public void NextStageLoad()
    {
        if(stageIdx < stageNames.Length) SceneManager.LoadScene(stageNames[stageIdx++]);
        // GameObject go = GameObject.Find("MapManager");
        // mapManager = go.GetComponent<MapManager>();
    }
}
