using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(parent.ToString());
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어 대미지 주는 코드
        }
        Destroy(gameObject);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        // Debug.Log(parent.ToString());
    }
}
