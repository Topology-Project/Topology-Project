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

        float temp = 0;
        transform.localEulerAngles += Vector3.left * Mathf.SmoothDamp(stability.x-dir.x, 0, ref temp, 0.1f);

        float stabilityX = stability.x>=0.01f ? stability.x-(1f*Time.deltaTime) : 0;
        stability = new Vector3(stabilityX, 0, 0);

        float x = transform.localEulerAngles.x>180 ? transform.localEulerAngles.x- 360 : transform.localEulerAngles.x;
        transform.eulerAngles = new Vector3(Math.Clamp(x, -85, 85), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
