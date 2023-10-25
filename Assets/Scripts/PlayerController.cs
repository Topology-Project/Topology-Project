using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController players; // 캐릭터 컨트롤러를 받아올 변수
    public float speed = 2.0f; // 속도
    public float gravity = 30.0f; // 중력을 추가
    Vector3 moveDirection = Vector3.zero; // 벡터 변수를 새로 만든후 초기화 해줍니다.

    // Start is called before the first frame update
    void Start()
    {
        players.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }
    void PlayerMove()
    {
        // 캐릭터가 지면에 닿았는가 리턴하는 함수를 적용
        if (players.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, 0, z) * speed;
        }
        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
            moveDirection.y = 10.0f;

        moveDirection.y -= gravity * Time.deltaTime;
        players.Move(moveDirection * Time.deltaTime);
    }
}
