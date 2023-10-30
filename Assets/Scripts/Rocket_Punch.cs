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
        Destroy(gameObject);
    }
}
