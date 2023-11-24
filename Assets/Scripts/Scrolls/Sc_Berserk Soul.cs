using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_BerserkSoul : Sc
{
    State[] states;
    Player player;

    public override Sc CreateSc()
    {
        return new Sc_BerserkSoul();
    }
    public override State[] GetState()
    {
        return states;
    }
    public override void SetState(State[] states) => this.states = states;
    
    public override void Active()
    {
        GameManager.Instance.TriggerManager.AddTrigger(PlayTriggerType.PlayerShot, HPCheck);
        player = GameManager.Instance.Player;
    }
    public override void Inactive()
    {
        GameManager.Instance.TriggerManager.DelTrigger(PlayTriggerType.PlayerShot, HPCheck);
    }
    public override void Awake()
    {
        // states = new State[1];
        // states[0] = new State(StateType.CriticalX, 0.25f, State.MulOper, 0);
    }
    public override Sc Clone()
    {
        Sc temp = new Sc_BerserkSoul();
        temp.SetState(states);
        return temp;
    }

    void HPCheck() => states[0].stack = 100 - (int)(player.MaxHealthPoint / player.HealthPoint);
}
