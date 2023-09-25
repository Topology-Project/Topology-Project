using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State
{
    public const int WEAPON_LENGTH = 11;

    public delegate StateHandler<State> OperatorHandler(State state, StateHandler<State> del=null);
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

    private void AddState(State modifier)
    {
        modifier.value += value;
    }
    private void MulState(State modifier)
    {
        modifier.value *= value;
    }

    public static StateHandler<State> AddOper(State state, StateHandler<State> del=null)
    {
        StateHandler<State> temp = state.AddState;
        return (StateHandler<State>)Delegate.Combine(del, temp);
    }
    public static StateHandler<State> MulOper(State state, StateHandler<State> del=null)
    {
        StateHandler<State> temp = state.MulState;
        return (StateHandler<State>)Delegate.Combine(temp, del);
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