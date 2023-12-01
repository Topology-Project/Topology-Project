using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 게임매니저 인스턴스
    public static GameManager Instance // 게임매니저의 모든 객체 접근 시
                                       // GameManager.Instance로 접근 권장
    {
        get { if (instance == null) Init(); return instance; }
        private set { instance = value; }
    }
    private static InputManager inputManager;
    public InputManager InpuManager
    {
        get { if (inputManager == null) Init(); return inputManager; }
        private set { inputManager = value; }
    }
    private static StageManager stageManager;
    public StageManager StageManager
    {
        get { if (stageManager == null) Init(); return stageManager; }
        private set { stageManager = value; }
    }
    private static TriggerManager triggerManager;
    public TriggerManager TriggerManager
    {
        get { if (triggerManager == null) Init(); return triggerManager; }
        private set { triggerManager = value; }
    }

    private Player player; // GameManager.Instance.Player로 플레이어 객체 접근 가능
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

    private Inscriptions inscriptions;
    public Inscriptions Inscriptions
    {
        get { if (inscriptions == null) Init(); return inscriptions; }
        private set { inscriptions = value; }
    }
    private Scrolls scrolls;
    public Scrolls Scrolls
    {
        get { if (scrolls == null) Init(); return scrolls; }
        private set { scrolls = value; }
    }
    private static Inventory inventory;
    public static Inventory Inventory
    {
        get { if (inventory == null) Init(); return inventory; }
        private set { inventory = value; }
    }
    private static EventSystem eventSystem;
    public static EventSystem EventSystem
    {
        get { if (eventSystem == null) Init(); return eventSystem; }
        private set { eventSystem = value; }
    }
    private static StandaloneInputModule standaloneInputModule;
    public static StandaloneInputModule StandaloneInputModule
    {
        get { if (standaloneInputModule == null) Init(); return standaloneInputModule; }
        private set { standaloneInputModule = value; }
    }


    private bool isPlay = false;
    public bool IsPlay { 
        get { return isPlay; }
        set
        {
            isPlay = value;
            if(isPlay)
            {  
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        }

    void Awake()
    {
        Init();
    }
    void Start()
    {
        // if(gameObject != GameManager.Instance) Destroy(gameObject);
    }
    static void Init() // 매니저 객체 초기화 메서드
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
            if (go.GetComponent<TriggerManager>() == null)
            {
                go.AddComponent<TriggerManager>();
            }
            if (go.GetComponent<Inventory>() == null)
            {
                go.AddComponent<Inventory>();
            }
            if (go.GetComponent<EventSystem>() == null)
            {
                go.AddComponent<EventSystem>();
            }
            if (go.GetComponent<StandaloneInputModule>() == null)
            {
                go.AddComponent<StandaloneInputModule>();
            }

            instance = go.GetComponent<GameManager>();
            DontDestroyOnLoad(go.gameObject);
            inputManager = go.GetComponent<InputManager>();
            stageManager = go.GetComponent<StageManager>();
            triggerManager = go.GetComponent<TriggerManager>();
            inventory = go.GetComponent<Inventory>();
            eventSystem = go.GetComponent<EventSystem>();
            standaloneInputModule = go.GetComponent<StandaloneInputModule>();
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
        if (instance.inscriptions == null)
        {
            GameObject go = GameObject.Find("Inscriptions");

            if (go == null)
            {
                go = new GameObject { name = "Inscriptions" };
            }
            if (go.GetComponent<Inscriptions>() == null)
            {
                go.AddComponent<Inscriptions>();
            }

            instance.inscriptions = go.GetComponent<Inscriptions>();
            DontDestroyOnLoad(go.gameObject);
        }
        if (instance.scrolls == null)
        {
            GameObject go = GameObject.Find("Scrolls");

            if (go == null)
            {
                go = new GameObject { name = "Scrolls" };
            }
            if (go.GetComponent<Scrolls>() == null)
            {
                go.AddComponent<Scrolls>();
            }

            instance.scrolls = go.GetComponent<Scrolls>();
            DontDestroyOnLoad(go.gameObject);
        }
    }

    public GameObject[] gameObjects;

    public void MoveGameObjectToScene()
    {
        foreach(GameObject go in gameObjects) SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneAt(0));
    }

    // void OnDestroy()
    // {
    //     Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
    //     foreach(GameObject go in gameObjects) if(go != null) Destroy(go);
    // }
}
