using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Image BGImg;
    public Image loadingImg;
    public Image stageImg;

    private int stageIdx;
    public int StageIdx
    { 
        get
        {
            return stageIdx;
        }
        set
        {
            stageIdx = value;
            
            stageImg.fillAmount = UnityEngine.Mathf.Floor(((float)stageIdx+1) / 6 * 100) / 100;
        }
    }

    private void Start()
    {
        StageIdx = GameManager.Instance.StageManager.StageIdx;
    }
}
