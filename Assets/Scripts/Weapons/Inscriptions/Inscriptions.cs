using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscriptions : MonoBehaviour
{
    [SerializeField]
    private Inscription inscription;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Inscription.Data data in inscription.datas)
        {
            Debug.Log(data);
        }
    }

    public Inscription.Data[] GetRandomDatas(int amount)
    {
        Inscription.Data[] datas = new Inscription.Data[amount];
        for(int i=0; i<amount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, inscription.datas.Length);
            datas[i] = inscription.datas[rnd];
        }
        return datas;
    }
}
