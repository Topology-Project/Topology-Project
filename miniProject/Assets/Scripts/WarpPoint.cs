using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public void Warp()
    {
        GameManager.StageManager.NextStageLoad();
    }
}
