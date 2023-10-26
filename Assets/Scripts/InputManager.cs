using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Player player;
    private Camera m_camera;
    private PlayerCamera playerCamera;
    private LayerMask layerMask;

    public Vector3 moveDir { get; private set; }
    public Vector3 lookDir { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.Instance.Player;
        playerCamera = GameManager.Instance.MainCamera;
        m_camera = playerCamera.GetComponent<Camera>();

        layerMask = (1<<LayerMask.NameToLayer("Wall"))|
                    (1<<LayerMask.NameToLayer("WarpPoint"))|
                    (1<<LayerMask.NameToLayer("Enemy"))|
                    (1<<LayerMask.NameToLayer("Item"))|
                    (1<<LayerMask.NameToLayer("Chest"));
    }
    // Update is called once per frame
    private void Update()
    {
        Vector3 rayOrigin = m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 rayDir = m_camera.transform.forward;

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        lookDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        player.Angle(playerCamera.transform.localEulerAngles);
        playerCamera.SetDir(lookDir);

        if(Input.GetButtonDown("Dash")) player.Dash();
        if(Input.GetButtonDown("Jump")) player.JumpOn();
        if(Input.GetButton("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonUp("Fire1")) player.Fire1(playerCamera.transform);
        if(Input.GetButtonDown("Reload")) player.Reload();

        RaycastHit raycastHit;
        if(Input.GetButtonDown("Interaction") && Physics.Raycast(rayOrigin, rayDir, out raycastHit, 5f, layerMask))
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
            else if(raycastHit.collider.transform.tag.Equals("Chest"))
            {
                raycastHit.collider.GetComponent<Chest>().BoxOpen();
            }
        }
    }
    private void FixedUpdate()
    {
        player.Move(moveDir.normalized);
    }
}
