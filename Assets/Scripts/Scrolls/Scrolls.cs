using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolls : MonoBehaviour
{
    [SerializeField]
    private Scroll scroll;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Scroll.Data data in scroll.datas)
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
            foreach(State state in data.states) 
            {
                state.operatorHandler = operatorHandler;
            }
        }
    }

    public Scroll.Data[] GetRandomDatas(int amount)
    {
        Scroll.Data[] datas = new Scroll.Data[amount];
        for(int i=0; i<amount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, scroll.datas.Length);
            datas[i] = Scroll.Clone(scroll.datas[rnd]);
            Debug.Log(datas[i].info);
        }
        return datas;
    }
}
