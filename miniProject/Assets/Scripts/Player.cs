using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private Camera playerCamera;
    private bool isJump;
    private ArrayList inventory = new();
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerCamera = GameManager.Instance.MainCamera.GetComponent<Camera>();
    }

    protected override void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        isJump = false;

    }

    public override void Move(Vector3 dir)
    {
        if(isDash) dir *= stateModifier.GetState(StateType.DashSpeed);
        Vector3 temp = transform.position + transform.TransformDirection(dir) * stateModifier.GetState(StateType.MovementSpeed) * Time.deltaTime;
        temp = new Vector3(temp.x, transform.position.y, temp.z);
        rig.MovePosition(temp);
    }

    public override void Angle(Vector3 dir)
    {
        transform.localEulerAngles = new Vector3(0, dir.y, 0);
    }

    public void JumpOn()
    {
        if(!isJump) Jump();
    }

    private void Jump()
    {
        isJump = true;
        rig.AddForce(Vector3.up * 4, ForceMode.VelocityChange);
    }

    public void AddInventory(Scroll scroll)
    {
        inventory.Add(scroll);
        stateModifier.AddHandler(scroll.state);
    }
}
