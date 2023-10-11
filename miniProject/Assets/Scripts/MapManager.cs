using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance
    {
        get { if(instance == null) Init(); return instance; }
        private set { instance = value; }
    }

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
        Init();
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
                if(mapStack.Count > 1) mapStack.Pop();
                else if(mapStack.Count == 1) keyValuePairs.Clear();
            }
        }

        Map next = null;
        for(int i=activeMap.Length-1; i>=0; i--)
        {
            activeMap[i] = mapStack.Pop();
            activeMap[i].nextMap = next;
            next = activeMap[i];
            if(mapStack.Count > 0) activeMap[i].prevMap = mapStack.Peek();
            activeMap[i].gameObject.SetActive(true);
        }

        player = GameManager.Instance.Player.gameObject;
        playerCamera = GameManager.Instance.MainCamera.gameObject;

    }


    // Update is called once per frame
    private void Update()
    {
        if(activeMap[activeMapIdx].enemyCount <= 0)
        {
            activeMap[activeMapIdx].DoorOpen(MapType.Next);
            activeMap[++activeMapIdx].DoorOpen(MapType.Prev);
        }
    }

    public void PlayerSpawn()
    {
        player.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        player.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
        playerCamera.transform.position = activeMap[activeMapIdx].playerSpawnPoint.position;
        playerCamera.transform.rotation = activeMap[activeMapIdx].playerSpawnPoint.rotation;
    } 
    private bool isSet = false;
    public void EnterRoom(Map map)
    {
        if(!isSet && activeMap[activeMapIdx] == map) 
        {
            activeMap[activeMapIdx].RoomSet();
            activeMap[activeMapIdx-1].DoorClose(MapType.Next);
            activeMap[activeMapIdx].DoorClose(MapType.Prev);
            isSet = true;
        }
    }

    public void EnemyDeath()
    {
        activeMap[activeMapIdx].enemyCount--;
    }

    private static void Init()
    {
        if(instance == null)
        {
        	GameObject go = GameObject.Find("Manager");

            if(go == null)
            {
            	go = new GameObject { name = "Manager" };
            }
            if(go.GetComponent<MapManager>() == null)
            {
            	go.AddComponent<MapManager>();
            }

            instance = go.GetComponent<MapManager>();
        }
    }
}
