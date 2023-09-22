using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject manager;
    private static InputManager inputMng;
    public InputManager InputMng
    {
        get { if(inputMng == null) inputMng = manager.GetComponent<InputManager>(); return inputMng; }
        private set { inputMng = value; }
    }

    public Player player;
    public PlayerCamera playerCamera;

    private void Awake()
    {
        inputMng =this;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 mouseDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        player.dir = dir.normalized;
        playerCamera.dir = mouseDir;
        player.mouseDir = mouseDir;
        if(Input.GetButtonDown("Dash")) player.DashOn();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButtonDown("Fire1")) player.Fire(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();
        if(Input.GetKeyDown(KeyCode.F)) {}
    }
}
