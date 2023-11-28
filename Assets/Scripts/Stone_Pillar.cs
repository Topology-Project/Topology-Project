using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Pillar : MonoBehaviour
{
    private void OnCollisionEnter(Collision obj)
    {
        if (!obj.gameObject.CompareTag("Player") && !obj.gameObject.CompareTag("Warning"))
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }

    }
    public void DestroyPillar() => gameObject.SetActive(false);
}
