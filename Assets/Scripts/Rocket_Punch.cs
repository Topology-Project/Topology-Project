using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Punch : Bullet
{
    protected override void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Warning"))
        {
            obj.GetComponent<Warning>().DestroyWarning();
            Destroy(gameObject);
        }
    }
}
