using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Perfect_Sixth : MonoBehaviour, Sc
{
    State[] states;
    int count;

    public State[] GetState()
    {
        return states;
    }
    public void Active()
    {
        count = 0;
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.playerShot, CountUp);
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.Reload, CountReset);
    }
    public void Inactive()
    {
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.playerShot, CountUp);
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.Reload, CountReset);
    }

    void CountUp()
    {
        count++;
        if(count == 5) states[1].stack = 1;
        if(count == 6) states[1].stack = 0;
    }
    void CountReset() => count = 0;

    void Awake()
    {
        states = new State[1];
        states[1] = new State(StateType.CriticalX, 0.8f, State.MulOper, 0);
    }
}
