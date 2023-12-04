using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    /* nextMaps와 doors 배열 순서 맞춰야함 */
    public Door[] doors; // 방이 가지고 있는 문 목록
    public Map[] nextMaps; // 연결된 방 목록
    /* ---------------------------------- */
    public Map nextMap; // 다음 방
    public Map prevMap; // 이전 방
    public Transform playerSpawnPoint; // 플레이어 스폰 위치 설정
    public Transform[] enemySpawnPoints; // 적들 스폰 위치 설정

    public int enemyCount = 0; // 현재 방에 존재하는 적들 카운트

    private bool isRoomSet = false;

    // 플레이어 진입 시 방 셋팅 용
    public void RoomSet()
    {
        if (!isRoomSet)
        {
            // 적스포너 당 하나 씩 적 인스턴스 생성 & enemyCount + 1
            // 문 워프 트리거 작동
            isRoomSet = true;
            foreach (Transform sp in enemySpawnPoints)
            {
                GameObject go;
                if(Random.Range(0, 2) == 0) go = Resources.Load("Prefabs/Enemy/Monster_Asset/Monster_Spear_Prefab") as GameObject;
                else go = Resources.Load("prefabs/Enemy/Monster_Asset/Monster_Sword_Prefab") as GameObject;
                Instantiate(go, sp);
                enemyCount++;
            }
            foreach (Door door in doors) door.DTSet();
            GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.EnemyDie, EnemyDecount); // 적이 죽을 때마다 작동되도록 트리거 설정
            GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.EnemyDie, RoomClear); // 적이 죽을 때마다 작동되도록 트리거 설정
            DoorActive(false);
        }
    }

    // 적 숫자 디카운트 용
    private void EnemyDecount() => enemyCount--;

    // 방 클리어 시 작동
    public void RoomClear()
    {
        if (isRoomSet && enemyCount <= 0)
        {
            // Debug.Log("roomclear / " + enemyCount);
            DoorActive(true);
            GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.EnemyDie, EnemyDecount); // 트리거 설정해제
            GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.EnemyDie, RoomClear); // 트리거 설정해제
            GameManager.Instance.TriggerManager.OnTrigger(PlayTriggerType.RoomClear);
        }
    }

    // 문 On, Off 메서드
    public void DoorActive(bool b)
    {
        int i = 0;
        if(GetIdx(prevMap, out i)) doors[i].IsOpen = b;
        if(GetIdx(nextMap, out i)) doors[i].IsOpen = b;
    }

    // 상자 언락 메서드
    public void ChestUnlock()
    {
        int i = 0;
        if (GetIdx(nextMap, out i)) doors[i].ChestUnlock();
    }

    // 상자 셋팅 메서드
    // 방문 워프 셋팅 용 메서드
    public void WarpSet(int size)
    {
        if(size <= 1)
        {
            doors[0].WarpSet();
            doors[0].ChestSet();
            return;
        } 
        int i = 0;
        if(GetIdx(nextMap, out i))
        {
            doors[i].WarpSet();
            doors[i].ChestSet();
        }
        else 
        {
            while(prevMap == nextMaps[i]) i++;
            doors[i].WarpSet();
            doors[i].ChestSet();
            nextMap = nextMaps[i];
        } 
    }

    // 방 문 찾기용 메서드 (임시)
    public bool GetIdx(Map map, out int idx)
    {
        for (int i = 0; i < nextMaps.Length; i++)
        {
            if (nextMaps[i] == map)
            {
                idx = i;
                return true;
            }
        }
        idx = 0;
        return false;
    }
}
