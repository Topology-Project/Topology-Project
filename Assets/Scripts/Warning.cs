using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public bool isDestroy = false;

    public void OnDestroy()
    {
        isDestroy = true;
        Debug.Log(isDestroy);
        // if ()
    }

    // private void OnCollisionEnter(Collision obj)
    // {
    //     if (obj.gameObject.CompareTag("Rocket_Punch"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
