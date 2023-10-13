using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Player player;
    private Camera camera;
    private PlayerCamera playerCamera;
    private Ray interactionRay = new();
    private LayerMask layerMask = (1<<6)|(1<<7)|(1<<8);

    public Vector3 moveDir { get; private set; }
    public Vector3 lookDir { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.Instance.Player;
        playerCamera = GameManager.Instance.MainCamera;
        camera = playerCamera.GetComponent<Camera>();
    }
    // Update is called once per frame
    private void Update()
    {
        Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 rayDir = camera.transform.forward;
        RaycastHit raycastHit;

        Debug.DrawRay(interactionRay.origin, interactionRay.direction * 100f, Color.green);
        Physics.Raycast(rayOrigin, rayDir, out raycastHit, 100f, layerMask);

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        lookDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        player.Angle(playerCamera.transform.localEulerAngles);
        playerCamera.SetDir(lookDir);

        if(Input.GetButtonDown("Dash")) player.Dash();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButton("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonUp("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();
        if(Input.GetButtonDown("Interaction"))
        {
            if(raycastHit.collider.transform.tag.Equals("Scroll"))
            {
                raycastHit.collider.GetComponent<Scroll>().GetState(player);
            }
            else if(raycastHit.collider.transform.tag.Equals("Weapon"))
            {
                raycastHit.collider.GetComponent<Weapon>();
            }
            else if(raycastHit.collider.transform.tag.Equals("Warp"))
            {
                raycastHit.collider.GetComponent<WarpPoint>().Warp();
            }
        }
    }
    private void FixedUpdate()
    {
        player.Move(moveDir.normalized);
    }
}
