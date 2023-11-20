using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sc
{
    public abstract Sc CreateSc();
    public abstract State[] GetState();
    public abstract void SetState(State[] states);
    public abstract void Active();
    public abstract void Inactive();
    public abstract void Awake();
    public abstract Sc Clone();
}