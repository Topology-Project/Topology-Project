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
    public OperatorHandler operatorHandler = State.BaseOper;

    void Start()
    {
        state = new(stateType, value, stack, operatorHandler);
    }

    public void GetState(Player player)
    {
        player.AddInventory(this);
        Destroy(gameObject);
    }
}
