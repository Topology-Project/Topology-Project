using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Door[] doors;
    public Map[] nextMaps;
    public Map nextMap;
    public Map prevMap;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    public int enemyCount = 0;
    private void Start()
    {
        enemyCount = enemySpawnPoints.Length;
    }

    private bool isRoomSet = false;
    public void RoomSet()
    {
        if(!isRoomSet)
        {
            isRoomSet = true;
            foreach(Transform sp in enemySpawnPoints)
            {
                GameObject go = Resources.Load("prefabs/Enemy/Enemy") as GameObject;
                Instantiate(go, sp);
            }
            foreach(Door door in doors) door.DTSet();
            DoorActive(false);
        }
    }
    public void RoomClear()
    {
        if(isRoomSet) DoorActive(true);
    }

    public void DoorActive(bool b)
    {
        int i = 0;
        if(GetIdx(prevMap, out i)) doors[i].IsOpen = b;
        if(GetIdx(nextMap, out i)) doors[i].IsOpen = b;
    }

    public void ChestUnlock()
    {
        int i = 0;
        if(GetIdx(nextMap, out i)) doors[i].ChestUnlock();
    }

    public void ChestSet()
    {
        int i = 0;
        if(GetIdx(nextMap, out i)) doors[i].ChestSet();
    }

    public void WarpSet()
    {
        int i = 0;
        if(GetIdx(prevMap, out i)) doors[i].WarpSet();
    }

    public bool GetIdx(Map map, out int idx) 
    {
        int i;
        for(i=0; i<nextMaps.Length; i++)
        {
            if(nextMaps[i] == map) 
            {
                idx = i;
                return true;
            }
        }
        idx = 0;
        return false;
    }
}
