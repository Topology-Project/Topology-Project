using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public State state { get; private set; }
    public StateType stateType;
    public float value;

    void Start()
    {
        state = new(stateType, value);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            player.AddInventory(this);
            //Destroy(this.gameObject);
        }
    }
}
