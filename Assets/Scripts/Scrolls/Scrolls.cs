using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Scrolls : MonoBehaviour
{
    [SerializeField]
    // 인스펙터에서 지정할 스크롤 객체
    private Scroll scroll;

    void Awake()
    {
        // 현재 실행 중인 어셈블리 가져오기
        Assembly assembly = Assembly.GetExecutingAssembly();

        // 각 스크롤 데이터에 대해 처리
        foreach(Scroll.Data sd in scroll.datas)
        {
            // 데이터의 이름을 사용하여 타입 가져오기
            System.Type t = assembly.GetType(sd.name);

            // Sc 타입의 객체 생성 및 초기화
            Sc obj = (System.Activator.CreateInstance(t) as Sc).CreateSc();

            // 연산 핸들러 설정
            OperatorHandler operatorHandler = null;
            switch(sd.operType)
            {
                case OperType.BaseOper:
                operatorHandler = State.BaseOper;
                break;
                case OperType.AddOper:
                operatorHandler = State.AddOper;
                break;
                case OperType.MulOper:
                operatorHandler = State.MulOper;
                break;
            }

            // 각 상태에 연산 핸들러 설정
            foreach(State state in sd.states) 
            {
                state.operatorHandler = operatorHandler;
            }

            // Sc 객체에 state 설정 및 Awake 호출
            obj.SetState(sd.states); 
            obj.Awake();

            // 원래의 sd.sc에 obj 대입
            sd.sc = obj;
        }
    }

    // 지정된 개수만큼 무작위 스크롤 데이터를 반환하는 메서드
    public Scroll.Data[] GetRandomDatas(int amount)
    {
        Scroll.Data[] datas = new Scroll.Data[amount];
        for(int i=0; i<amount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, scroll.datas.Length);
            datas[i] = Scroll.Clone(scroll.datas[rnd]);
        }
        return datas;
    }
}
