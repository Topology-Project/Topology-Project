using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public const int WEAPON_LENGTH = 11;
    public StateType stateType;
    public float value { private set; get; }

    public State(StateType stateType, float value)
    {
        SetState(stateType, value);
    }

    public void SetState(StateType stateType=StateType.BaseDamage, float value=0f)
    {
        this.stateType = stateType;
        this.value = value;
    }

    public void SetState(State state)
    {
        SetState(state.stateType, state.value);
    }

    public void AddState(State modifier)
    {
        modifier.value += value;
    }

    public static State Clone(State state)
    {
        return new State(state.stateType, state.value);
    }
    public static bool isTypeEquals(State s1, State s2)
    {
        if(s1.stateType == s2.stateType) return true;
        else return false;
    }
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