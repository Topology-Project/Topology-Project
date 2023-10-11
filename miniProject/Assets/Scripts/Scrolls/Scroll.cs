using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Scroll : MonoBehaviour
{
    public State state { get; private set; }
    public StateType stateType;
    public OperatorHandler operatorHandler;
    public float value;

    private int stack;
    public int Stack
    {
        get { return stack; }
        set
        {
            stack = value;
            state.SetState(stateType, stack*value);
        }
    }

    void Start()
    {
        state = new(stateType, value);
    }

    public void GetState(Player player)
    {
            player.AddInventory(this);
            Debug.Log(stateType.ToString() + " +" + value);
    }
}
