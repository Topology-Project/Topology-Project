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
    private Vector3 dir;
    // Update is called once per frame
    private void Update()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 mouseDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        player.Angle(mouseDir);
        playerCamera.SetDir(mouseDir);

        if(Input.GetButtonDown("Dash")) player.Dash();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButton("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonUp("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();
        if(Input.GetKeyDown(KeyCode.F)) {}
    }
    private void FixedUpdate()
    {
        player.Move(dir.normalized);
    }
}
