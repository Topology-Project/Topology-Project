using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBullet : Bullet
{
    public GameObject parentobj;

    protected override void Start()
    {
        this.parent = parentobj;
        this.speed = 0;
        startPos = parentobj.transform.position;
        this.range = 9999;
        this.time = 9999;
    }
    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.tag.Equals(parent.tag))
        {
            Debug.Log("damage");
        }

    }
}
