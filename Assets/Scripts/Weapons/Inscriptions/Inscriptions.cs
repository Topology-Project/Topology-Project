using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscriptions : MonoBehaviour
{
    [SerializeField]
    // 인스펙터에서 지정할 인장 객체
    private Inscription inscription;
    // Start is called before the first frame update
    void Awake()
    {
        // 각 데이터의 연산자 핸들러를 설정
        foreach(Inscription.Data data in inscription.datas)
        {
            OperatorHandler operatorHandler = null;
            switch(data.operType)
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

            // 각 상태의 연산자 핸들러를 설정
            foreach(State state in data.states) 
            {
                state.operatorHandler = operatorHandler;
            }
        }
    }

    // 지정된 개수만큼 랜덤한 데이터를 반환하는 메서드
    public Inscription.Data[] GetRandomDatas(int amount)
    {
        Inscription.Data[] datas = new Inscription.Data[amount];
        for(int i=0; i<amount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, inscription.datas.Length);
            datas[i] = Inscription.Clone(inscription.datas[rnd]);
            // Debug.Log(datas[i].info);
        }
        return datas;
    }
}
