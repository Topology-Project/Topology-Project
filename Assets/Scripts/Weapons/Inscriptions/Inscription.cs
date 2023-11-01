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
    }
    
    public Data[] datas;
}
