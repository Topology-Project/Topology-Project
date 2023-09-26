using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StateHandler<State>(ref float baseVar, ref float sum, ref float mul);


public class State
{
    public delegate StateHandler<State> OperatorHandler(State state);
    public OperatorHandler operatorHandler { get; private set; }

    public StateType stateType { private set; get; }
    public float value { private set; get; }

    public State(StateType stateType=StateType.BaseDamage, float value=0f, OperatorHandler operatorHandler=null)
    {
        if(operatorHandler == null) operatorHandler = AddOper;
        this.operatorHandler = operatorHandler;
        SetState(stateType, value);
    }

    public void SetState(StateType stateType, float value)
    {
        this.stateType = stateType;
        this.value = value;
    }

    public void SetState(State state)
    {
        SetState(state.stateType, state.value);
    }

    private void BaseState(ref float baseVar, ref float sum, ref float mul)
    {
        baseVar = value;
    }
    private void AddState(ref float baseVar, ref float sum, ref float mul)
    {
        sum += value;
    }
    private void MulState(ref float baseVar, ref float sum, ref float mul)
    {
        mul *= value;
    }

    public static StateHandler<State> BaseOper(State state)
    {
        return state.BaseState;
    }
    public static StateHandler<State> AddOper(State state)
    {
        return state.AddState;
    }
    public static StateHandler<State> MulOper(State state)
    {
        return state.MulState;
    }

    // public static State Clone(State state)
    // {
    //     return new State(state.stateType, state.value);
    // }
    // public static bool isTypeEquals(State s1, State s2)
    // {
    //     if(s1.stateType == s2.stateType) return true;
    //     else return false;
    // }
    public static float operator +(State s1, State s2)
    {
        return s1.value + s2.value;
    }
    public static float operator *(State s1, State s2)
    {
        return s1.value * s2.value;
    }
    public static float operator +(int i, State s2)
    {
        return i + s2.value;
    }
    public static float operator *(State s1, float f)
    {
        return s1.value * f;
    }
    public static string operator +(string s, State s1)
    {
        return s + s1.value;
    }
    public static implicit operator float(State s1)
    {
        return s1.value;
    }
}