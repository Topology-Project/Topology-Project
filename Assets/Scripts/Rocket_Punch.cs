using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Rocket_Punch : Bullet
{
    protected override void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Warning"))
        {
            obj.GetComponent<Warning>().DestroyWarning();
=======
public class Rocket_Punch : MonoBehaviour
{
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Warning"))
        {
>>>>>>> PlayerController
            Destroy(gameObject);
        }
    }
}
