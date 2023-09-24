using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager;

    private Player player;
    private PlayerCamera playerCamera;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.Instance;

        player = GameManager.Player;
        playerCamera = GameManager.MainCamera;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 dir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 mouseDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        player.Move(dir.normalized);
        player.Angle(mouseDir);
        playerCamera.SetDir(mouseDir);
        if(Input.GetButtonDown("Dash")) player.Dash();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButtonDown("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();
        if(Input.GetKeyDown(KeyCode.F)) {}
    }
}
