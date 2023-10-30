using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayTrigger();

public class TriggerManager : MonoBehaviour
{
    Dictionary<PlayTriggerType, PlayTrigger> keyValuePairs;
    private event PlayTrigger enemyHit;
    private event PlayTrigger enemyDie;
    private event PlayTrigger roomClear;

    private void Awake()
    {
        keyValuePairs = new Dictionary<PlayTriggerType, PlayTrigger>();

        keyValuePairs.Add(PlayTriggerType.EnemyHit, enemyHit);
        keyValuePairs.Add(PlayTriggerType.EnemyDie, enemyDie);
        keyValuePairs.Add(PlayTriggerType.RoomClear, roomClear);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 트리거 작동 메서드
    public void OnTrigger(PlayTriggerType playTriggerType)
    {
        if(keyValuePairs[playTriggerType] != null) keyValuePairs[playTriggerType]();
    }
    // 트리거 추가 메서드
    public void AddTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        keyValuePairs[playTriggerType] += playTrigger;
    }
    // 트리가 제거 메서드
    public void DelTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        keyValuePairs[playTriggerType] -= playTrigger;
    }
}
