using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    Scroll.Data data;

    public Scroll.Data GetData()
    {
        Debug.Log(data.info);
        Destroy(gameObject);
        return data;
    }

    void Start()
    {
        data = GameManager.Instance.Scrolls.GetRandomDatas(1)[0];
    }
}
