using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// public class Scrolls : MonoBehaviour
// {
//     [SerializeField]
//     private Scroll scroll;

//     void Awake()
//     {
//         Assembly assembly = Assembly.GetExecutingAssembly();

//         foreach (Scroll.Data sc in scroll.datas)
//         {
//             System.Type t = assembly.GetType(sc.name);
//             object obj = System.Activator.CreateInstance(t);

//             if (obj == null)
//             {
//                 Debug.LogError("Scroll Load Erorr");
//                 return;
//             }

//             sc.sc = (Sc)(obj as Scroll);
//             Debug.Log(sc.sc);
//         }
//     }

//     public Scroll.Data[] GetRandomDatas(int amount)
//     {
//         Scroll.Data[] datas = new Scroll.Data[amount];
//         for (int i = 0; i < amount; i++)
//         {
//             int rnd = UnityEngine.Random.Range(0, scroll.datas.Length);
//             datas[i] = Scroll.Clone(scroll.datas[rnd]);
//             Debug.Log(datas[i].info);
//         }
//         return datas;
//     }
// }
