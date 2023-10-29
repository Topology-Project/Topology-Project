using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Scroll : MonoBehaviour
{
    public State state { get; private set; }
    public StateType stateType;
    public float value;
    public int stack;
    public float rate;
    public OperatorHandler operatorHandler = State.BaseOper;

    void Start()
    {
        state = new(stateType, value, operatorHandler, stack, rate);
    }

    public void GetState(Player player)
    {
        player.AddInventory(this);
        Destroy(gameObject);
    }
}
