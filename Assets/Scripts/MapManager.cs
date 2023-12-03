using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum RootType { Fixed, Random }
    public RootType rootType;
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
        activeMap = new Map[round];
        Stack<Map> mapStack = new();

        // 루트 타입에 따라 방을 생성하는 로직 선택
        if(rootType == RootType.Random) RandomRoot(mapStack);
        else if(rootType == RootType.Fixed) FixedRoot(mapStack);

        Map next = null;
        for(int i=activeMap.Length-1; i>=0; i--)
        {
            activeMap[i] = mapStack.Pop();
            activeMap[i].nextMap = next;
            next = activeMap[i];
            if(mapStack.Count > 0) activeMap[i].prevMap = mapStack.Peek();
            if(i == activeMap.Length-1) 
            {
                activeMap[i].WarpSet(round); // 마지막 방 문 워프 설정
                // Debug.Log(activeMap[i].name);
            }
        }
        
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.RoomClear, ClearRoom);

        player = GameManager.Instance.Player.gameObject;
        playerCamera = GameManager.Instance.MainCamera.gameObject;

        PlayerSpawn(); // 플레이어 워프
        // EnterRoom(); // 방 셋팅
    }
    // 고정된 루트를 사용하여 방을 스택에 추가
    private void FixedRoot(Stack<Map> mapStack)
    {
        foreach(Map map in maps) mapStack.Push(map);
    }
    // 무작위 루트를 사용하여 방을 스택에 추가
    private void RandomRoot(Stack<Map> mapStack)
    {
        // 각 맵과 해당 맵이 스택에 추가되었는지 여부를 저장하는 딕셔너리
        Dictionary<Map, bool> keyValuePairs = new();
        int random = 0;
        
        // 원하는 방의 개수에 도달할 때까지 반복
        while(mapStack.Count < round)
        {
            // 스택이 비어있을 때 초기화
            if(mapStack.Count <= 0)
            {
                mapStack.Clear();
                // maps 배열에서 무작위로 한 개의 맵을 스택에 추가
                mapStack.Push(maps[Random.Range(0, maps.Length)]);
            }

            // 현재 스택의 맨 위 맵을 가져옴
            Map peek = mapStack.Peek();

            // 맵이 딕셔너리에 존재하지 않으면 추가하고 값을 false로 초기화
            if(!keyValuePairs.ContainsKey(peek)) keyValuePairs.Add(peek, false);

            // 값이 false일 때 (맵을 추가할 수 있는 상태)
            if(!keyValuePairs[peek])
            {
                keyValuePairs[peek] = true; // 값을 true로 변경하여 현재 맵이 두 번 연속으로 추가되지 않도록 함
                random = Random.Range(0, peek.doors.Length); // 현재 맵의 문 중 하나를 무작위로 선택

                // 선택된 문이 다음 맵을 가지고 있고, 그 다음 맵이 딕셔너리에 존재하면 스택에 추가
                if(!peek.nextMaps[random].IsUnityNull() && keyValuePairs.ContainsKey(peek.nextMaps[random])) mapStack.Push(peek.nextMaps[random]);
            }
            // 값이 true일 때 (중복된 맵이 있는 상태)
            else 
            {
                keyValuePairs[peek] = false; // 값을 false로 변경하여 다음에도 다시 추가할 수 있도록 함
                mapStack.Pop(); // 스택에서 현재 맵을 제거
            }
        }
    }
            // Map peek = mapStack.Peek();
            // if(!keyValuePairs.ContainsKey(peek))
            // {
            //     keyValuePairs.Add(peek, 0);
            // }
            // if(keyValuePairs[peek] < 4)
            // {
            //     keyValuePairs[peek]++;
            //     random = Random.Range(0, peek.doors.Length);

            //     peek = peek.nextMaps[random];
            //     if(peek != null && !mapStack.Contains(peek)) 
            //     {
            //         mapStack.Push(peek);
            //     }
            // }
            // else
            // {
            //     if(mapStack.Count > 0) mapStack.Pop();
            //     if(mapStack.Count == 0) 
            //     {
            //         random = Random.Range(0, maps.Length);
            //         mapStack.Push(maps[random]);
            //         keyValuePairs.Clear();
            //     }
            // }

    // Update is called once per frame
    private void Update()
    {
        
    }

    void OnDestroy()
    {
        Debug.LogWarning(gameObject.scene.name);
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.RoomClear, ClearRoom);
    }
    private void ClearRoom()
    {
        // 방 클릭어 시 셋팅 (임시)
        activeMap[activeMapIdx].ChestUnlock();
        activeMap[activeMapIdx].DoorActive(true);
        activeMapIdx++;
        if(activeMapIdx < activeMap.Length) 
        {
            activeMap[activeMapIdx].DoorActive(true);
        }
        // Debug.Log("room clear");
    }

    // 플레이어 워프
    private void PlayerSpawn()
    {
        player.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        player.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
        playerCamera.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        playerCamera.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
        playerCamera.GetComponent<PlayerCamera>().SetDirForce(activeMap[activeMapIdx].playerSpawnPoint.rotation);
    } 

    // 방 셋팅 용 메서드
    public void EnterRoom()
    {
        activeMap[activeMapIdx].RoomSet();
        // Debug.Log("room set");
         activeMap[activeMapIdx].RoomClear();
    }
}
