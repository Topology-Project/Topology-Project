using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Image BGImg; // 배경 이미지를 나타내는 Image 컴포넌트
    public Image loadingImg; // 로딩 이미지를 나타내는 Image 컴포넌트
    public Image stageImg; // 스테이지 진행 상황을 나타내는 Image 컴포넌트

    private int stageIdx; // 현재 스테이지 인덱스를 저장하는 변수
    public int StageIdx
    {
        get
        {
            return stageIdx;
        }
        set
        {
            stageIdx = value;

            // 스테이지 진행 상황 이미지 업데이트
            stageImg.fillAmount = UnityEngine.Mathf.Floor(((float)stageIdx + 1) / 6 * 100) / 100;
        }
    }

    private void Start()
    {
        // 현재 스테이지 인덱스 초기화
        StageIdx = GameManager.Instance.StageManager.StageIdx;
    }
}
