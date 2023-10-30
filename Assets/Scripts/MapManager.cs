using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Map[] maps; // 스테이지 내 방 목록

    public int round; // 스테이지 진행 방 갯수(방 갯수 -1 까지만)
    private Map[] activeMap; // 현재 스테이지에서 사용할 방 목록
    private int activeMapIdx = 0; // 현재 진행 중인 방 인덱스

    private GameObject player;
    private GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 스테이지 방 루트 설정
        activeMap = new Map[++round];
        Stack<Map> mapStack = new();
        Dictionary<Map, int?> keyValuePairs = new();
        int random = Random.Range(0, maps.Length);
        mapStack.Push(maps[random]);
        
        while(mapStack.Count < round)
        {
            Map peek = mapStack.Peek();
            if(!keyValuePairs.ContainsKey(peek))
            {
                keyValuePairs.Add(peek, 0);
            }
            if(keyValuePairs[peek] < 4)
            {
                keyValuePairs[peek]++;
                random = Random.Range(0, peek.doors.Length);

                peek = peek.nextMaps[random];
                if(peek != null && !mapStack.Contains(peek)) 
                {
                    mapStack.Push(peek);
                }
            }
            else
            {
                if(mapStack.Count > 0) mapStack.Pop();
                if(mapStack.Count == 0) 
                {
                    random = Random.Range(0, maps.Length);
                    mapStack.Push(maps[random]);
                    keyValuePairs.Clear();
                }
            }
        }

        Map next = null;
        for(int i=activeMap.Length-1; i>=0; i--)
        {
            activeMap[i] = mapStack.Pop();
            activeMap[i].nextMap = next;
            next = activeMap[i];
            if(mapStack.Count > 0) activeMap[i].prevMap = mapStack.Peek();
            if(i == activeMap.Length-2) activeMap[i].WarpSet(); // 마지막 방 문 워프 설정
            if(i == activeMap.Length-2) activeMap[i].ChestSet(); // 마지막 방 클리어 보상 상자 설정
        }

        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.RoomClear, ClearRoom);

        player = GameManager.Instance.Player.gameObject;
        playerCamera = GameManager.Instance.MainCamera.gameObject;

        PlayerSpawn(); // 플레이어 워프
        EnterRoom(); // 방 셋팅
    }


    // Update is called once per frame
    private void Update()
    {
        
    }

    void OnDestroy()
    {
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.RoomClear, ClearRoom);
    }
    private void ClearRoom()
    {
        // 방 클릭어 시 셋팅 (임시)
        activeMap[activeMapIdx++].ChestUnlock();
        activeMap[activeMapIdx].DoorActive(true);
    }

    // 플레이어 워프
    private void PlayerSpawn()
    {
        player.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        player.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
        playerCamera.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        playerCamera.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
    } 

    // 방 셋팅 용 메서드
    public void EnterRoom()
    {
        activeMap[activeMapIdx].RoomSet();
    }
}
