using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { if (instance == null) Init(); return instance; }
        private set { instance = value; }
    }
    private static InputManager inputManager;
    public static InputManager InpuManager
    {
        get { if (inputManager == null) Init(); return inputManager; }
        private set { inputManager = value; }
    }
    private static StageManager stageManager;
    public static StageManager StageManager
    {
        get { if (stageManager == null) Init(); return stageManager; }
        private set { stageManager = value; }
    }

    private Player player;
    public Player Player
    {
        get { if (player == null) Init(); return player; }
        private set { player = value; }
    }

    private PlayerCamera mainCamera;
    public PlayerCamera MainCamera
    {
        get { if (mainCamera == null) Init(); return mainCamera; }
        private set { mainCamera = value; }
    }

    void Start()
    {
        Init();
    }
    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Manager");

            if (go == null)
            {
                go = new GameObject { name = "Manager" };
            }
            if (go.GetComponent<GameManager>() == null)
            {
                go.AddComponent<GameManager>();
            }
            if (go.GetComponent<InputManager>() == null)
            {
                go.AddComponent<InputManager>();
            }
            if (go.GetComponent<StageManager>() == null)
            {
                go.AddComponent<StageManager>();
            }

            instance = go.GetComponent<GameManager>();
            DontDestroyOnLoad(go.gameObject);
            inputManager = go.GetComponent<InputManager>();
            stageManager = go.GetComponent<StageManager>();
        }
        if (instance.player == null)
        {
            GameObject go = GameObject.Find("Player");

            if (go == null)
            {
                go = (GameObject)Resources.Load("Player/Player");
            }
            if (go.GetComponent<Player>() == null)
            {
                go.AddComponent<Player>();
            }

            instance.player = go.GetComponent<Player>();
            DontDestroyOnLoad(go.gameObject);
        }
        if (instance.mainCamera == null)
        {
            GameObject go = GameObject.Find("Main Camera");

            if (go == null)
            {
                go = new GameObject { name = "Main Camera" };
            }
            if (go.GetComponent<PlayerCamera>() == null)
            {
                go.AddComponent<PlayerCamera>();
            }

            instance.mainCamera = go.GetComponent<PlayerCamera>();
            DontDestroyOnLoad(go.gameObject);
        }
    }
}
