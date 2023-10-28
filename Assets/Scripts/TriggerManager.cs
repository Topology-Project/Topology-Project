using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayTrigger();

public class TriggerManager : MonoBehaviour
{
    Dictionary<PlayTriggerType, PlayTrigger> keyValuePairs;
    private event PlayTrigger EnemyHit;
    private event PlayTrigger EnemyDie;

    private void Awake()
    {
        keyValuePairs = new Dictionary<PlayTriggerType, PlayTrigger>();

        keyValuePairs.Add(PlayTriggerType.EnemyHit, EnemyHit);
        keyValuePairs.Add(PlayTriggerType.EnemyDie, EnemyDie);
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
    public void onTrigger(PlayTriggerType playTriggerType)
    {
        if(keyValuePairs[playTriggerType] != null) keyValuePairs[playTriggerType]();
    }
    // 트리거 추가 메서드
    public void AddTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        if(keyValuePairs[playTriggerType] != null) keyValuePairs[playTriggerType] += playTrigger;
    }
    // 트리가 제거 메서드
    public void DelTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        if(keyValuePairs[playTriggerType] != null) keyValuePairs[playTriggerType] -= playTrigger;
    }
}
