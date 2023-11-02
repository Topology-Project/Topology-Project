using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inscription", menuName = "ScriptableObject/Inscription")]
public class Inscription : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public InscriptionType inscriptionType;

        public string info;

        public PlayTriggerType stackTrigger;
        public PlayTriggerType rateTrigger;

        public OperatorHandler operatorHandler;

        public List<State> states;
        public OperType operType;
        
        public Data(Data data)
        {
            inscriptionType = data.inscriptionType;
            info = data.info;
            stackTrigger = data.stackTrigger;
            rateTrigger = data.rateTrigger;
            operatorHandler = data.operatorHandler;
            states = data.states;
            operType = data.operType;
        }
    }

    public static Inscription.Data Clone(Inscription.Data data)
    {
        return new Inscription.Data(data);
    }
    
    public Data[] datas;
}
