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

    public void Warp() => warp();

    private void WarpStage()
    {
        GameManager.StageManager.NextStageLoad();
    }
    private void WarpTransform()
    {
        GameObject player = GameManager.Instance.Player.gameObject;
        player.transform.position = warpTransform.position;
    }
}
