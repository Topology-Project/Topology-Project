using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWarp : MonoBehaviour
{
    public WarpPoint warpPoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player")) warpPoint.Warp();
    }
}
