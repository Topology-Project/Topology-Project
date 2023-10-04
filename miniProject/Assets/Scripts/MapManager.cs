using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    WarpPoint[] enterSecretWarpPoints;
    WarpPoint[] exitSecretWarpPoints;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("WarpPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
