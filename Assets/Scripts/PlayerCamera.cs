using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerArm;
    private Player player;
    private Vector3 dir;
    private Vector3 stability;

    // 카메라 rotation 설정 메서드
    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }
    public void SetDirForce(Quaternion dir)
    {
        transform.rotation = dir;
        tempVector = dir.eulerAngles;
    }
    // 카메라 반동값 설정 메서드
    public void SetStability(float stability)
    {
        float random = UnityEngine.Random.Range(-stability, stability);
        // float random = 0;
        this.stability += new Vector3(stability, random, 0);
        this.tempVector += new Vector3(stability, random, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }

    private Vector3 tempVector;
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += dir;

        Vector3 temp;
        if(stability.x >= 0.01f || stability.y >= 0.01f)
        {
            // 반동값 반영
            stability -= dir;
            temp = Vector3.Slerp(Vector3.zero, stability, 30f*Time.deltaTime);
            // 반동값 회복
            transform.localEulerAngles -= temp;
            tempVector -= temp;
            stability -= temp;
        }
        tempVector += dir;
        tempVector = Rotation(tempVector);
        
        // 카메라 rotation 보정
        temp = Vector3.Slerp(Vector3.zero, Over180(Rotation(transform.localEulerAngles-tempVector)), 3f*Time.deltaTime);
        transform.localEulerAngles -= temp;
        tempVector += temp * 0.2f;

        /// 플레이어 암
        playerArm.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, playerArm.transform.localEulerAngles.y, playerArm.transform.localEulerAngles.z);
        /// 플레이어 암

        float x = transform.localEulerAngles.x>180 ? transform.localEulerAngles.x- 360 : transform.localEulerAngles.x;
        transform.localEulerAngles = new Vector3(Math.Clamp(x, -85, 85), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        tempVector = new Vector3(tempVector.x, tempVector.y, 0);
    }

    // rotation 값 보정 용 메서드
    // 범위 밖의 값을 보정해줌
    private Vector3 Over180(Vector3 vector)
    {
        float x = vector.x > 180 ? vector.x - 360 : vector.x;
        float y = vector.y > 180 ? vector.y - 360 : vector.y;
        float z = vector.z > 180 ? vector.z - 360 : vector.z;

        // float x = vector.x > 180 ? vector.x - 360 : vector.x < -180 ? vector.x + 360 : vector.x;
        // float y = vector.y > 180 ? vector.y - 360 : vector.x < -180 ? vector.y + 360 : vector.y;
        // float z = vector.z > 180 ? vector.z - 360 : vector.x < -180 ? vector.z + 360 : vector.z;

        return new Vector3(x, y, z);
    }
    private Vector3 Rotation(Vector3 vector)
    {
        float x = vector.x > 360 ? vector.x - 360 : vector.x < 0 ? vector.x + 360 : vector.x;
        float y = vector.y > 360 ? vector.y - 360 : vector.y < 0 ? vector.y + 360 : vector.y;
        float z = vector.z > 360 ? vector.z - 360 : vector.z < 0 ? vector.z + 360 : vector.z;

        return new Vector3(x, y, z);
    }
}
