using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "ScriptableObject/Scroll")]
public class Scroll : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public ScrollType ScrollType;

        public Sprite sprite;
        public string name;
        public string info;

        public Sc sc;
        
        public Data(Data data)
        {
            ScrollType = data.ScrollType;
            sprite = data.sprite;
            name = data.name;
            info = data.info;
        }
    }

    public static Scroll.Data Clone(Scroll.Data data)
    {
        Scroll.Data temp = new Scroll.Data(data);
        temp.sc = data.sc.Clone();
        return temp;
    }
    
    public Data[] datas;
}
