using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스텟 연산용 델리게이트
public delegate void StateHandler<State>(ref float baseVar, ref float sum, ref float mul);
// 스텟 연산 종류 설정용 델리게이트
public delegate StateHandler<State> OperatorHandler(State state);

public class State
{
    public OperatorHandler operatorHandler { get; private set; }

    public StateType stateType { private set; get; } // 스텟 종류
    private float stateValue; // 스텟 값
    public float Value 
    { 
        private set
        {
            stateValue = value;
        }
        get
        {
            // 스택당 스탯값 계산 후 반환
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

    // 수치 합연산
    private void BaseState(ref float baseVar, ref float sum, ref float mul)
    {
        baseVar += Value;
    }
    // 배율 합연산
    private void AddState(ref float baseVar, ref float sum, ref float mul)
    {
        sum += Value;
    }
    // 배율 곱연산
    private void MulState(ref float baseVar, ref float sum, ref float mul)
    {
        mul *= Value;
    }

    /* 연산자 설정 용 메서드 (델리게이트) */
    // 수치 합연산 설정
    // operhandler 호출 시 BaseState메서드 반환
    public static StateHandler<State> BaseOper(State state)
    {
        return state.BaseState;
    }
    // 배율 합연산 설정
    // operhandler 호출 시 AddState메서드 반환
    public static StateHandler<State> AddOper(State state)
    {
        return state.AddState;
    }
    // 배율 곱연산 설정
    // operhandler 호출 시 MulState메서드 반환
    public static StateHandler<State> MulOper(State state)
    {
        return state.MulState;
    }
    /* ---------------------------------------- */

    // public static State Clone(State state)
    // {
    //     return new State(state.stateType, state.Value);
    // }
    // public static bool isTypeEquals(State s1, State s2)
    // {
    //     if(s1.stateType == s2.stateType) return true;
    //     else return false;
    // }

    /* operator override */
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
    /* ------------------------------------ */
}