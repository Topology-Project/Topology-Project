using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject manager;
    private static GameManager gameMng;
    public GameManager GameMng
    {
        get { if(gameMng == null) gameMng = manager.GetComponent<GameManager>(); return gameMng; }
        private set { gameMng = value; }
    }

    void Awake()
    {
        gameMng = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
