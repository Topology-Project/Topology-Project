using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscriptions : MonoBehaviour
{
    [SerializeField]
    private Inscription inscription;
    // Start is called before the first frame update
    void Awake()
    {
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
            foreach(State state in data.states) 
            {
                state.operatorHandler = operatorHandler;
            }
        }
    }

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
