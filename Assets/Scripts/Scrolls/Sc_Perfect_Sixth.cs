using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sc_Perfect_Sixth : Sc
{
    State[] states;
    int count;

    public override Sc CreateSc()
    {
        return new Sc_Perfect_Sixth();
    }
    public override State[] GetState()
    {
        return states;
    }
    public override void SetState(State[] states) => this.states = states;
    
    public override void Active()
    {
        count = 0;
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.PlayerShot, CountUp);
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.Reload, CountReset);
    }
    public override void Inactive()
    {
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.PlayerShot, CountUp);
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.Reload, CountReset);
    }
    public override void Awake()
    {
        // states = new State[1];
        // states[0] = new State(StateType.CriticalX, 0.8f, State.BaseOper, 0);
    }
    public override Sc Clone()
    {
        Sc temp = new Sc_Perfect_Sixth();
        temp.SetState(states);
        return temp;
    }

    void CountUp()
    {
        count++;
        if(count == 6) states[0].stack = 1;
        else states[0].stack = 0;
    }
    void CountReset() => count = 0;

}
