using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayTrigger();

public class TriggerManager : MonoBehaviour
{
    // 트리거 타입에 따른 델리게이트 이벤트들을 저장하는 딕셔너리
    Dictionary<PlayTriggerType, PlayTrigger> keyValuePairs;

    // 각 트리거 이벤트에 대한 델리게이트 이벤트들
    private event PlayTrigger enemyHit;
    private event PlayTrigger enemyDie;
    private event PlayTrigger roomClear;
    private event PlayTrigger playerShot;
    private event PlayTrigger playerHit;
    private event PlayTrigger playerDie;
    private event PlayTrigger reload;

    private void Awake()
    {
        // 딕셔너리 및 각 트리거 이벤트 초기화
        keyValuePairs = new Dictionary<PlayTriggerType, PlayTrigger>();
    
        // 각 트리거 이벤트와 해당 키를 딕셔너리에 등록
        keyValuePairs.Add(PlayTriggerType.EnemyHit, enemyHit);
        keyValuePairs.Add(PlayTriggerType.EnemyDie, enemyDie);
        keyValuePairs.Add(PlayTriggerType.RoomClear, roomClear);
        keyValuePairs.Add(PlayTriggerType.PlayerShot, playerShot);
        keyValuePairs.Add(PlayTriggerType.PlayerHit, playerHit);
        keyValuePairs.Add(PlayTriggerType.PlayerDie, playerDie);
        keyValuePairs.Add(PlayTriggerType.Reload, reload);
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
        // 트리거 타입이 None이면 작동하지 않음
        if(playTriggerType == PlayTriggerType.None) return;

        // 해당 트리거 타입에 등록된 이벤트가 있다면 실행
        if(keyValuePairs[playTriggerType] != null) keyValuePairs[playTriggerType]();
    }
    // 트리거 추가 메서드
    public void AddTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        // 트리거 타입이 None이면 추가하지 않음
        if(playTriggerType == PlayTriggerType.None) return;

        // 해당 트리거 타입에 델리게이트 이벤트 추가
        keyValuePairs[playTriggerType] += playTrigger;
    }
    // 트리가 제거 메서드
    public void DelTrigger(PlayTriggerType playTriggerType, PlayTrigger playTrigger)
    {
        // 트리거 타입이 None이면 제거하지 않음
        if(playTriggerType == PlayTriggerType.None) return;

        // 해당 트리거 타입에서 델리게이트 이벤트 제거
        keyValuePairs[playTriggerType] -= playTrigger;
    }

    // 트리거를 초기화하는 메서드
    public void ClearTrigger(PlayTriggerType playTriggerType) => keyValuePairs[playTriggerType] = null;
}
