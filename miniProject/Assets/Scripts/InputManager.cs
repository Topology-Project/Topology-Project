using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Player player;
    private PlayerCamera playerCamera;

    public Vector3 moveDir { get; private set; }
    public Vector3 lookDir { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.Instance.Player;
        playerCamera = GameManager.Instance.MainCamera;
    }
    // Update is called once per frame
    private void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        lookDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        player.Angle(playerCamera.transform.localEulerAngles);
        playerCamera.SetDir(lookDir);

        if(Input.GetButtonDown("Dash")) player.Dash();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButton("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonUp("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();
        if(Input.GetButtonDown("Interaction")) {}
    }
    private void FixedUpdate()
    {
        player.Move(moveDir.normalized);
    }
}
