using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Player player;
    public Weapon weapon;
    public Camera mainCamera;
    private PlayerCamera playerCamera;

    // Start is called before the first frame update
    private void Start()
    {
        playerCamera = mainCamera.GetComponent<PlayerCamera>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 dir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 mouseDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        player.dir = dir.normalized;
        playerCamera.dir = mouseDir;
        player.mouseDir = mouseDir;
        if(Input.GetButtonDown("Dash")) player.DashOn();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButtonDown("Fire1")) weapon.Fire(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) weapon.Reload();
        if(Input.GetKeyDown(KeyCode.F)) {}
    }
}
