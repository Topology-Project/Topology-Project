using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
    public static GameManager Instance
    {
        get { if(instance == null) Init(); return instance; }
        private set { instance = value; }
    }

    private Player player;
    public Player Player
    {
        get { if(player == null) Init(); return player; }
        private set { player = value; }
    }

    private PlayerCamera mainCamera;
    public PlayerCamera MainCamera
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
    	if(instance.player == null)
        {
        	GameObject go = GameObject.Find("Player");

            if(go == null)
            {
            	go = new GameObject { name = "Player" };
                instance.player.transform.position = new Vector3(0, 1, 0);
                instance.player.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            if(go.GetComponent<Player>() == null)
            {
            	go.AddComponent<Player>();
            }

            instance.player = go.GetComponent<Player>();
        }
    	if(instance.mainCamera == null)
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

            instance.mainCamera = go.GetComponent<PlayerCamera>();
        }
    }       
}
