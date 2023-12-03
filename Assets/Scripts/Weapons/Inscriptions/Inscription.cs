using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립터블 오브젝트를 생성하기 위한 메뉴를 에디터 상에서 추가
[CreateAssetMenu(fileName = "Inscription", menuName = "ScriptableObject/Inscription")]
public class Inscription : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public InscriptionType inscriptionType; // 인장 등급

        public string info; // 인장에 대한 정보

        public PlayTriggerType stackTrigger;
        public PlayTriggerType rateTrigger;

        public OperatorHandler operatorHandler; // 연산자 핸들러

        public List<State> states;
        public OperType operType;

        // public Data(InscriptionType inscriptionType, string info,
        //  PlayTriggerType stackTrigger, PlayTriggerType rateTrigger, 
        //  OperatorHandler operatorHandler, List<State> states, OperType operType)
        // {
        //     this.inscriptionType = inscriptionType;
        //     this.info = info;
        //     this.stackTrigger = stackTrigger;
        //     this.rateTrigger = rateTrigger;
        //     this.operatorHandler = operatorHandler;
        //     this.states = states;
        //     this.operType = operType;
        // }

        // Clone 생성자
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

    // 데이터 복제 메서드
    public static Inscription.Data Clone(Inscription.Data data)
    {
        return new Inscription.Data(data);
    }
    
    // 데이터 배열
    public Data[] datas;
}
