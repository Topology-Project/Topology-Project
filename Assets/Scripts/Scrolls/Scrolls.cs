using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Scrolls : MonoBehaviour
{
    [SerializeField]
    private Scroll scroll;

    void Awake()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach(Scroll.Data sd in scroll.datas)
        {
            System.Type t = assembly.GetType(sd.name);
            Sc obj = (System.Activator.CreateInstance(t) as Sc).CreateSc();
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
            foreach(State state in sd.states) 
            {
                state.operatorHandler = operatorHandler;
            }
            obj.SetState(sd.states); 
            obj.Awake();
            sd.sc = obj;
        }
    }

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
