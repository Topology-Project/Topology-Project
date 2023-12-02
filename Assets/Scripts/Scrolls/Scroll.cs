using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립터블 오브젝트를 생성하기 위한 메뉴를 에디터 상에서 추가
[CreateAssetMenu(fileName = "Scroll", menuName = "ScriptableObject/Scroll")]
public class Scroll : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public ScrollType ScrollType;

        // 스프라이트, 이름, 정보 등의 속성
        public Sprite sprite;
        public string name;
        public string info;

        public State[] states;
        public OperType operType;

       // Sc 클래스의 인스턴스
        public Sc sc;

        // Data 클래스의 생성자
        public Data(Data data)
        {
            ScrollType = data.ScrollType;
            sprite = data.sprite;
            name = data.name;
            info = data.info;
            states = data.states;
            operType = data.operType;
        }
    }

    // 데이터 복제 메서드
    public static Scroll.Data Clone(Scroll.Data data)
    {
        Scroll.Data temp = new Scroll.Data(data);
        temp.sc = data.sc.Clone();
        return temp;
    }

    // 데이터 배열
    public Data[] datas;
}
