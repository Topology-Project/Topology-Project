using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Meteor : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(parent.ToString());
        if (other.gameObject.CompareTag("Player"))
=======
public class Meteor : MonoBehaviour
{
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
>>>>>>> PlayerController
        {
            // 플레이어 대미지 주는 코드
        }
        Destroy(gameObject);
    }
<<<<<<< HEAD
    protected override void OnTriggerEnter(Collider other)
    {
        // Debug.Log(parent.ToString());
    }
=======
>>>>>>> PlayerController
}
