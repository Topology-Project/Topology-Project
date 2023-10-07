using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public Map[] maps;

    public int round;
    private Map[] activeMap;

    // Start is called before the first frame update
    void Start()
    {
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
            if(keyValuePairs[peek] < peek.doors.Length)
            {
                keyValuePairs[peek]++;
                random = Random.Range(0, peek.doors.Length);
                peek = peek.nextMaps[random];
                if(peek != null && !mapStack.Contains(peek)) mapStack.Push(peek);
            }
            else
            {
                if(mapStack.Count > 1) mapStack.Pop();
                else if(mapStack.Count == 1) keyValuePairs.Clear();
            }
        }
        for(int i=0; i<activeMap.Length; i++)
        {
            activeMap[i] = mapStack.Pop();
            activeMap[i].gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
