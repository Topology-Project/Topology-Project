using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateModifier
{
    // 스탯 연산용 델리게이트 딕셔너리
    private Dictionary<StateType, StateHandler<State>> modifier = new();

    // 다양한 스탯에 대한 이벤트
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
        // 딕셔너리에 스탯 연산 이벤트 추가
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

    // 스텟 연산 결과값 반환 메서드
    public State GetState(StateType stateType)
    {
        float baseVar = 0;
        float sum = 0;
        float mul = 0;

        // 딕셔너리에서 해당 스탯의 연산 이벤트를 찾아 호출
        modifier[stateType](ref baseVar, ref sum, ref mul);
        // if(stateType == StateType.CriticalX) Debug.Log(stateType.ToString() + " : " + baseVar+","+sum+","+mul+","+(baseVar + ((baseVar==0 ? 1 : baseVar) * sum)) * mul);

        // 스텟 객체 생성 및 반환
        return new State(stateType, (baseVar + (baseVar*sum)) * (1+mul));
    }

    // 스탯 핸들러 추가 메서드
    public void AddHandler(State state)
    {
        // 해당 스탯의 연산 이벤트에 스탯의 연산 메서드 추가
        // StateHandler<State> += void (ref float baseVar, ref float sum, ref float mul)
        modifier[state.stateType] += state.operatorHandler(state);
    }
    // 다른 StateModifier의 스탯 핸들러를 추가하는 메서드
    public void AddHandler(StateModifier stateModifier)
    {
        // 이미 등록된 핸들러 제거 후 새로운 핸들러 추가
        DelHandler(stateModifier);
        foreach(var handler in stateModifier.modifier)
        {
            modifier[handler.Key] += handler.Value;
        }
    }

    // 스탯 핸들러 제거 메서드
    public void DelHandler(State state)
    {
        // 해당 스탯의 연산 이벤트에서 스탯의 연산 메서드 제거
        modifier[state.stateType] -= state.operatorHandler(state);
    }
    // 다른 StateModifier의 스탯 핸들러를 제거하는 메서드
    public void DelHandler(StateModifier stateModifier)
    {
        // 다른 StateModifier의 모든 연산 이벤트에서 연산 메서드 제거
        foreach(var handler in stateModifier.modifier)
        {
            modifier[handler.Key] -= handler.Value;
        }
    }

    // private static bool CheckDelegateHasMethod(StateHandler<State> @delegate)
    // {
    //     return @delegate?.GetInvocationList()
    //             .Where(d => d.Method == method.Method)
    //             .Count() > 0;
    // }
}