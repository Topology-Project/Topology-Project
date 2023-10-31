using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscriptions : MonoBehaviour
{
    [SerializeField]
    private Inscription inscription;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Inscription.Data data in inscription.datas)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
