using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Sc
{
    public State[] GetState();
    public abstract void Active();
    public abstract void Inactive();
}
