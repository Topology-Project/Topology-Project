using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Punch : MonoBehaviour
{
    private void OnCollisionEnter(Collision obj)
    {
        // if (obj.gameObject.CompareTag("Player") || obj.gameObject.CompareTag("Warning"))
        // {
        //     Destroy(gameObject);
        // }
        if (obj.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Warning"))
        {
            Destroy(obj.gameObject);
            Destroy(gameObject);
        }
    }
}
