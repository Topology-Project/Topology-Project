using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public State state { get; private set; }
    public StateType stateType;
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

    public void SetStack(int stack)  { Stack = stack; }
    public void AddStack(int addnum) { SetStack(Stack+addnum); }
    public void SubStack(int subnum) { SetStack(Stack+subnum); }

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
            Debug.Log(stateType.ToString() + " +" + value);
            //Destroy(this.gameObject);
        }
    }
}
