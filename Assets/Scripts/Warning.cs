using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
}
