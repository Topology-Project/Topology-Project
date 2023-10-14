using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private string[] stageNames;
    private int stageIdx;

    public MapManager mapManager { get; private set;}

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
        // if(stageIdx < stageNames.Length) SceneManager.LoadScene(stageNames[stageIdx++]);
        // GameObject go = GameObject.Find("MapManager");
        if(stageIdx < stageNames.Length) 
        {
            var op = SceneManager.LoadSceneAsync(stageNames[stageIdx++]);
            op.completed += (x) => {
                mapManager = FindObjectOfType<MapManager>();
            };
        }
    }
}
