using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Rocket_Punch"))
        {
            Destroy(gameObject);
        }
    }
}
