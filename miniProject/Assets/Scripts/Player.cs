using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool isDash;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooltime;
    [SerializeField] private bool isJump;
    public Vector3 dir;
    public Vector3 mouseDir;
    Rigidbody rig;
    ArrayList inventory = new();
    private void Awake()
    {
        // 임시
        maxHealthPoint = 100;
        armorType = ProtectionType.Shield;
        maxArmorPoint = 100;
        defaultSpeed = 10;
        dashSpeed = 4f;
        dashDuration = 0.5f;
        dashCooltime = 3;
    }
    // Start is called before the first frame update
    private void Start()
    {
        healthPoint = maxHealthPoint;
        armorPoint = maxArmorPoint;
        dir = new Vector3(0, 0, 0);
        dashCooltimer = 3;
        rig = GetComponent<Rigidbody>();
        stateModifire = new();
        weapon.OnWeapon(this);
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Angle(mouseDir);
    }

    private float dashTimer = 0;
    private float dashCooltimer = 0;
    private void Movement()
    {
        if(isDash)
        {
            Dash(dir);

            dashTimer += Time.deltaTime;
            if(dashTimer >= dashDuration)
            {
                dashCooltimer = 0;
                isDash = false;
            }
        }
        else
        {
            Move(dir);
            
            if(dashCooltimer <= dashCooltime)
            {
                dashTimer = 0;
                dashCooltimer += Time.deltaTime;
            }
        }
    }

    public void DashOn()
    {
        if(dashCooltimer <= dashCooltime) isDash = true;
    }

    private void Dash(Vector3 dir)
    {
        dir.z *= dashSpeed;
        dir.x *= dashSpeed;
        Move(dir);
    }

    public void JumpOn()
    {
        if(!isJump) Jump();
    }

    private void Jump()
    {
        isJump = true;
        rig.AddForce(Vector3.up * 4, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        isJump = false;

    }

    public void AddInventory(Scroll scroll)
    {
        inventory.Add(scroll);
        stateModifire.AddHandler(scroll.state);
    }
}
