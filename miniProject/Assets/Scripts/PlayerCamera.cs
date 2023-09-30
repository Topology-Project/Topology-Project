using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Player player;
    private Vector3 dir;
    private Vector3 stability;

    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }
    public void SetStability(float stability)
    {
        this.stability += new Vector3(stability, 0, 0);
        this.tempVector += new Vector3(stability, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Player;
    }

    private Vector3 tempVector;
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += dir;
        tempVector += dir;
        tempVector = Rotation(tempVector);

        if(stability.x >= 0.01f || stability.y >= 0.01f)
        {
            stability -= dir;
            Vector3 temp = Vector3.Slerp(Vector3.zero, stability, 30f*Time.deltaTime);
            transform.localEulerAngles -= temp;
            stability -= temp;
        }
        // else
        // {
        //     Vector3 temp = Vector3.Lerp(transform.localEulerAngles-tempVector, Vector3.zero, 30f*Time.deltaTime);

        //     transform.localEulerAngles -= temp;
        // }
        
        float x = transform.localEulerAngles.x>180 ? transform.localEulerAngles.x- 360 : transform.localEulerAngles.x;
        transform.localEulerAngles = new Vector3(Math.Clamp(x, -85, 85), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }

    private Vector3 Rotation(Vector3 vector)
    {
        float x = vector.x > 360 ? vector.x - 360 : vector.x < 0 ? vector.x + 360 : vector.x;
        float y = vector.y > 360 ? vector.y - 360 : vector.y < 0 ? vector.y + 360 : vector.y;
        float z = vector.z > 360 ? vector.z - 360 : vector.z < 0 ? vector.z + 360 : vector.z;

        return new Vector3(x, y, z);
    }
}
