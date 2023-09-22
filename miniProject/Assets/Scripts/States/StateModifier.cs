using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void StateHandler<State>(State modifire);

public class StateModifier
{
    private Dictionary<StateType, StateHandler<State>> modifire;

    private event StateHandler<State> baseDamage;
    private event StateHandler<State> criticalX;
    private event StateHandler<State> luckyShot;
    private event StateHandler<State> magazine;
    private event StateHandler<State> projectiles;
    private event StateHandler<State> projectileSpeed;
    private event StateHandler<State> rateOfFire;
    private event StateHandler<State> reloadTime;
    private event StateHandler<State> upgrade;
    private event StateHandler<State> accuracy;
    private event StateHandler<State> stability;
    private event StateHandler<State> baseDMGIncrease;
    private event StateHandler<State> ExplosionDMGIncrease;
    private event StateHandler<State> ElementalDMGIncrease;
    private event StateHandler<State> movementSpeed;

    public StateModifier()
    {
        modifire = new Dictionary<StateType, StateHandler<State>>();

        modifire.Add(StateType.BaseDamage, baseDamage);
        modifire.Add(StateType.CriticalX, criticalX);
        modifire.Add(StateType.LuckyShot, luckyShot);
        modifire.Add(StateType.Magazine, magazine);
        modifire.Add(StateType.Projectiles, projectiles);
        modifire.Add(StateType.ProjectileSpeed, projectileSpeed);
        modifire.Add(StateType.RateOfFire, rateOfFire);
        modifire.Add(StateType.ReloadTime, reloadTime);
        modifire.Add(StateType.WPNUpgrade, upgrade);
        modifire.Add(StateType.Accuracy, accuracy);
        modifire.Add(StateType.Stability, stability);
        modifire.Add(StateType.baseDMGIncrease, baseDMGIncrease);
        modifire.Add(StateType.ExplosionDMGIncrease, ExplosionDMGIncrease);
        modifire.Add(StateType.ElementalDMGIncrease, ElementalDMGIncrease);
        modifire.Add(StateType.MovementSpeed, movementSpeed);
    }


    public State GetState(StateType stateType)
    {
        State temp = new State(stateType, 0);
        modifire[stateType](temp);
        
        return temp;
    }
    public void AddHandler(State state)
    {
        modifire[state.stateType] += state.AddState;
    }
    public void DelHandler(State state)
    {
        modifire[state.stateType] += state.AddState;
    }
}