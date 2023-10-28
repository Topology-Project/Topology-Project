using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WarpType
{
    Room, Stage
}

public class WarpPoint : MonoBehaviour
{
    private delegate void WarpDel();
    private WarpDel warp;

    public WarpType warpType;
    public Transform warpTransform;

    private void Start()
    {
        if(warpType == WarpType.Room) warp = WarpTransform;
        else if(warpType == WarpType.Stage) warp = WarpStage;
    }

    // 매서드 실행 시
    // warpType이 WarpType.Room 일 때 WarpTransform 메서드 실행
    // warpType이 WarpType.Stage 일 떄 WarpStage 메서드 실행
    public void Warp() => warp();

    // 다음 스테이지 전환 용
    private void WarpStage()
    {
        GameManager.Instance.StageManager.NextStageLoad();
    }

    // 스테이지 내 이동 용
    private void WarpTransform()
    {
        GameObject player = GameManager.Instance.Player.gameObject;
        player.transform.position = warpTransform.position;
    }
}
