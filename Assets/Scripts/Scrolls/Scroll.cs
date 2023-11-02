using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "ScriptableObject/Scroll")]
public class Scroll : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public Scroll ScrollType;

        public Sprite sprite;
        public string name;
        public string info;

        public PlayTriggerType stackTrigger;
        public PlayTriggerType rateTrigger;

        public OperatorHandler operatorHandler;

        public List<State> states;
        public OperType operType;
        
        public Data(Data data)
        {
            ScrollType = data.ScrollType;
            sprite = data.sprite;
            name = data.name;
            info = data.info;
            stackTrigger = data.stackTrigger;
            rateTrigger = data.rateTrigger;
            operatorHandler = data.operatorHandler;
            states = data.states;
            operType = data.operType;
        }
    }

    public static Scroll.Data Clone(Scroll.Data data)
    {
        return new Scroll.Data(data);
    }
    
    public Data[] datas;
}
