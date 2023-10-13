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
        y = doors[0].transform.position.y;
    }

    private bool temp = true;
    public void OnCollisionEnter(Collision other)
    {
        if(temp && other.transform.tag.Equals("Player"))
        {
            GameManager.StageManager.mapManager.EnterRoom(this);
            temp = false;
        }
    }

    private bool isRoomSet = false;
    public void RoomSet()
    {
        if(!isRoomSet)
        {
            isRoomSet = true;
            foreach(Transform sp in enemySpawnPoints)
            {
                Instantiate(GameManager.StageManager.mapManager.enemy, sp);
            }
        }
    }

    private int GetIdx(MapType mapType)
    {
        int i=0;
        if(mapType == MapType.Prev) while(nextMaps[i] != prevMap) i++;
        else if(mapType == MapType.Next) while(nextMaps[i] != nextMap) i++;
        // Debug.Log(i);
        return i;
    }

    float y = 0;

    public void DoorOpen(MapType mapType)
    {
        GameObject door = doors[GetIdx(mapType)];
        door.transform.position = new Vector3(door.transform.position.x, y+5, door.transform.position.z);
        // door.transform.Translate(door.transform.position + (Vector3.up * 3f), Space.Self);
    }
    public void DoorClose(MapType mapType)
    {
        GameObject door = doors[GetIdx(mapType)];
        door.transform.position = new Vector3(door.transform.position.x, y, door.transform.position.z);
        // door.transform.Translate(door.transform.position + (Vector3.down * 3f), Space.Self);
    }
}
