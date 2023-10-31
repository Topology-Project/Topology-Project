using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayTriggerType
{
    None,
    
    PlayerHit, EnemyHit,
    PlayerDie, EnemyDie,

    RoomEnter, RoomClear,
    StageEnter, StageClear
}
