using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private event StateHandler<State> explosionRange;
    private event StateHandler<State> explosionDMGIncrease;
    private event StateHandler<State> elementalRate;
    private event StateHandler<State> elementalDMGIncrease;
    private event StateHandler<State> range;
    private event StateHandler<State> ammoRate;

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
        modifier.Add(StateType.BaseDMGIncrease, baseDMGIncrease);
        modifier.Add(StateType.ExplosionRange, explosionRange);
        modifier.Add(StateType.ExplosionDMGIncrease, explosionDMGIncrease);
        modifier.Add(StateType.ElementalRate, elementalRate);
        modifier.Add(StateType.ElementalDMGIncrease, elementalDMGIncrease);
        modifier.Add(StateType.Range, range);
        modifier.Add(StateType.AmmoRate, ammoRate);

        modifier.Add(StateType.MaxHealthPoint, maxHealthPoint);
        modifier.Add(StateType.MaxProtectionPoint, maxProtectionPoint);

        modifier.Add(StateType.MovementSpeed, movementSpeed);
        modifier.Add(StateType.DashSpeed, dashSpeed);
        modifier.Add(StateType.DashDuration, dashDuration);
        modifier.Add(StateType.DashCooltime, DashCooltime);
    }

    public State GetState(StateType stateType)
    {
        float baseVar = 0;
        float sum = 0;
        float mul = 1;
        modifier[stateType](ref baseVar, ref sum, ref mul);
        // Debug.Log(baseVar+","+sum+","+mul+","+(baseVar + ((baseVar==0 ? 1 : baseVar) * sum)) * mul);
        return new State(stateType, (baseVar + ((baseVar==0 ? 1 : baseVar) * sum)) * mul);
    }
    public void AddHandler(State state)
    {
        modifier[state.stateType] += state.operatorHandler(state);
    }
    public void AddHandler(StateModifier stateModifier)
    {
        foreach(var handler in stateModifier.modifier)
        {
            modifier[handler.Key] += handler.Value;
        }
    }
    public void DelHandler(State state)
    {
        modifier[state.stateType] -= state.operatorHandler(state);
    }
    public void DelHandler(StateModifier stateModifier)
    {
        foreach(var handler in stateModifier.modifier)
        {
            modifier[handler.Key] -= handler.Value;
        }
    }
}