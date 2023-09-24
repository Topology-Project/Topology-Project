using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
    public static  GameManager Instance
    {
        get { if(instance == null) Init(); return instance; }
        private set { instance = value; }
    }

    private static Player player;
    public static Player Player
    {
        get { if(player == null) Init(); return player; }
        private set { player = value; }
    }

    private static PlayerCamera mainCamera;
    public static PlayerCamera MainCamera
    {
        get { if(mainCamera == null) Init(); return mainCamera; }
        private set { mainCamera = value; }
    }

	void Start()
    {
        Init();
    }
    static void Init()
    {
    	if(instance == null)
        {
        	GameObject go = GameObject.Find("Manager");

            if(go == null)
            {
            	go = new GameObject { name = "Manager" };
            }
            if(go.GetComponent<GameManager>() == null)
            {
            	go.AddComponent<GameManager>();
            }

            instance = go.GetComponent<GameManager>();
        }
    	if(player == null)
        {
        	GameObject go = GameObject.Find("Player");

            if(go == null)
            {
            	go = new GameObject { name = "Player" };
            }
            if(go.GetComponent<Player>() == null)
            {
            	go.AddComponent<Player>();
            }

            player = go.GetComponent<Player>();
        }
    	if(mainCamera == null)
        {
        	GameObject go = GameObject.Find("Main Camera");
            
            if(go == null)
            {
            	go = new GameObject { name = "Main Camera" };
            }
            if(go.GetComponent<PlayerCamera>() == null)
            {
                go.AddComponent<PlayerCamera>();
            }

            mainCamera = go.GetComponent<PlayerCamera>();
        }
    }       
}
