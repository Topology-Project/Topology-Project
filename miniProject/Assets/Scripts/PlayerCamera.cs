using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 dir;
    private Player player;

    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += dir;
        float x = transform.localEulerAngles.x>180 ? transform.localEulerAngles.x- 360 : transform.localEulerAngles.x;
        transform.eulerAngles = new Vector3(Math.Clamp(x, -85, 85), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
