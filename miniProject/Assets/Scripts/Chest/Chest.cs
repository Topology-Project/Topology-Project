using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject contents;
    public bool isOpen;

    
    // Start is called before the first frame update
    void Start()
    {
        contents.SetActive(false);
    }

    public void BoxOpen()
    {
        if(isOpen)contents.SetActive(true);
    }
}
