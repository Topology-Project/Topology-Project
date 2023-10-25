using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Map[] maps;

    public int round;
    private Map[] activeMap;
    private int activeMapIdx = 0;

    public  GameObject enemy;

    private GameObject player;
    private GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        round += 1;
        activeMap = new Map[round];
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
            if(i == activeMap.Length-1) activeMap[i].WarpSet();
            if(i == activeMap.Length-2) activeMap[i].ChestSet();
        }

        player = GameManager.Instance.Player.gameObject;
        playerCamera = GameManager.Instance.MainCamera.gameObject;

        PlayerSpawn();
        EnterRoom();
    }


    // Update is called once per frame
    private void Update()
    {
        if(activeMap[activeMapIdx].enemyCount <= 0)
        {
            activeMap[activeMapIdx].ChestUnlock();
            activeMap[activeMapIdx++].RoomClear();
            activeMap[activeMapIdx].DoorActive(true);
        }
    }

    private void PlayerSpawn()
    {
        player.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        player.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
        playerCamera.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        playerCamera.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
    } 

    public void EnterRoom()
    {
        activeMap[activeMapIdx].RoomSet();
    }

    public void EnemyDeath()
    {
        activeMap[activeMapIdx].enemyCount--;
    }
}
