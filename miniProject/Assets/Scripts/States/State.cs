using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StateHandler<State>(ref float baseVar, ref float sum, ref float mul);
public delegate StateHandler<State> OperatorHandler(State state);

public class State
{
    public OperatorHandler operatorHandler { get; private set; }

    public StateType stateType { private set; get; }
    private float stateValue;
    public float Value 
    { 
        private set
        {
            stateValue = value;
        }
        get
        {
            return stateValue * stack;
        } 
    }
    public int stack { private set; get; }

    public State(StateType stateType=StateType.BaseDamage, float Value=0f, int stack=1, OperatorHandler operatorHandler=null)
    {
        this.stateType = stateType;
        this.Value = Value;
        this.stack = stack;
        if(operatorHandler == null) operatorHandler = State.BaseOper;
        this.operatorHandler = operatorHandler;
    }

    public void SetState(StateType stateType=StateType.BaseDamage, float Value=0f, int stack=1, OperatorHandler operatorHandler=null)
    {
        this.stateType = stateType;
        this.Value = Value;
        this.stack = stack;
        if(operatorHandler == null) operatorHandler = State.BaseOper;
        this.operatorHandler = operatorHandler;
    }

    public void SetState(State state)
    {
        SetState(state.stateType, state.Value, state.stack, state.operatorHandler);
    }

    private void BaseState(ref float baseVar, ref float sum, ref float mul)
    {
        baseVar += Value;
    }
    private void AddState(ref float baseVar, ref float sum, ref float mul)
    {
        sum += Value;
    }
    private void MulState(ref float baseVar, ref float sum, ref float mul)
    {
        mul *= Value;
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
    //     return new State(state.stateType, state.Value);
    // }
    // public static bool isTypeEquals(State s1, State s2)
    // {
    //     if(s1.stateType == s2.stateType) return true;
    //     else return false;
    // }
    public static float operator +(State s1, State s2)
    {
        return s1.Value + s2.Value;
    }
    public static float operator *(State s1, State s2)
    {
        return s1.Value * s2.Value;
    }
    public static float operator +(int i, State s2)
    {
        return i + s2.Value;
    }
    public static float operator *(State s1, float f)
    {
        return s1.Value * f;
    }
    public static string operator +(string s, State s1)
    {
        return s + s1.Value;
    }
    public static implicit operator float(State s1)
    {
        return s1.Value;
    }
}