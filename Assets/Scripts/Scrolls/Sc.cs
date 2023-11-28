using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sc
{
    public abstract Sc CreateSc(); // 상속밭은 자식 클래스 반환할 것
    public abstract State[] GetState(); // getter
    public abstract void SetState(State[] states); // setter
    public abstract void Active(); // state 변화에 필요한 트리거 등 설정
    public abstract void Inactive(); // 설정한 트리거 해재
    public abstract void Awake(); // state 등 초기화할 것 들
    public abstract Sc Clone(); // 상속밭은 자식 클래스 반환할 것
}