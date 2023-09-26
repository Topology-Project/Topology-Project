using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool isJump;
    ArrayList inventory = new();
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        healthPoint = maxHealthPoint;
        armorPoint = maxProtectionPoint;
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

    void OnCollisionEnter(Collision collision)
    {
        isJump = false;

    }

    public void AddInventory(Scroll scroll)
    {
        inventory.Add(scroll);
        stateModifier.AddHandler(scroll.state);
    }
}
