using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void StateHandler<State>(State modifier);

public class StateModifier
{
    private Dictionary<StateType, StateHandler<State>> modifier = new();

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
    private event StateHandler<State> range;

    private event StateHandler<State> maxHealthPoint;
    private event StateHandler<State> maxProtectionPoint;

    private event StateHandler<State> movementSpeed;
    private event StateHandler<State> dashSpeed;
    private event StateHandler<State> dashDuration;
    private event StateHandler<State> DashCooltime;

    public StateModifier()
    {
        modifier.Add(StateType.BaseDamage, baseDamage);
        modifier.Add(StateType.CriticalX, criticalX);
        modifier.Add(StateType.LuckyShot, luckyShot);
        modifier.Add(StateType.Magazine, magazine);
        modifier.Add(StateType.Projectiles, projectiles);
        modifier.Add(StateType.ProjectileSpeed, projectileSpeed);
        modifier.Add(StateType.RateOfFire, rateOfFire);
        modifier.Add(StateType.ReloadTime, reloadTime);
        modifier.Add(StateType.WPNUpgrade, upgrade);
        modifier.Add(StateType.Accuracy, accuracy);
        modifier.Add(StateType.Stability, stability);
        modifier.Add(StateType.baseDMGIncrease, baseDMGIncrease);
        modifier.Add(StateType.ExplosionDMGIncrease, ExplosionDMGIncrease);
        modifier.Add(StateType.ElementalDMGIncrease, ElementalDMGIncrease);
        modifier.Add(StateType.Range, range);

        modifier.Add(StateType.MaxHealthPoint, maxHealthPoint);
        modifier.Add(StateType.MaxProtectionPoint, maxProtectionPoint);

        modifier.Add(StateType.MovementSpeed, movementSpeed);
        modifier.Add(StateType.DashSpeed, dashSpeed);
        modifier.Add(StateType.DashDuration, dashDuration);
        modifier.Add(StateType.DashCooltime, DashCooltime);
    }


    public State GetState(StateType stateType)
    {
        State temp = new State(stateType, 0);
        modifier[stateType](temp);
        
        return temp;
    }
    public void AddHandler(State state)
    {
        modifier[state.stateType] += state.AddState;
    }
    public void DelHandler(State state)
    {
        modifier[state.stateType] += state.AddState;
    }
}