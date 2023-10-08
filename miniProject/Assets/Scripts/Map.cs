using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MapType
{
    Next, Prev
}
public class Map : MonoBehaviour
{
    public GameObject[] doors;
    public Map[] nextMaps;
    public Map nextMap;
    public Map prevMap;
    public string[] nextRoomName;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    public int enemyCount = 0;
    private void Start()
    {
        enemyCount = enemySpawnPoints.Length;
    }

    private bool temp = true;
    public void OnCollisionEnter(Collision other)
    {
        if(temp && other.transform.tag.Equals("Player"))
        {
            MapManager.Instance.EnterRoom(this);
            temp = false;
        }
    }

    public void RoomSet()
    {
        foreach(Transform sp in enemySpawnPoints)
        {
            Instantiate(MapManager.Instance.enemy, sp);
        }
    }

    private int GetIdx(Map map)
    {
        int i=0;
        for( ; i<nextMaps.Length; i++)
        {
            if(nextMaps[i] == map) break;
        }
        return i;
    }

    public void DoorOpen(MapType mapType)
    {
        GameObject door = doors[GetIdx(nextMap)];
        if(mapType == MapType.Next) door = doors[GetIdx(prevMap)];
        door.transform.position += Vector3.up * 5f;
        // door.transform.Translate(door.transform.position + (Vector3.up * 3f), Space.Self);
    }
    public void DoorClose(MapType mapType)
    {
        GameObject door = doors[GetIdx(nextMap)];
        if(mapType == MapType.Next) door = doors[GetIdx(prevMap)];
        door.transform.position += Vector3.down * 5f;
        // door.transform.Translate(door.transform.position + (Vector3.down * 3f), Space.Self);
    }
}
