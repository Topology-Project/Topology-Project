using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Player player; // 플레이어 객체를 저장하는 변수
    private Camera m_camera; // 메인 카메라를 저장하는 변수
    private PlayerCamera playerCamera; // 플레이어 카메라를 저장하는 변수
    private LayerMask layerMask; // 레이어 마스크를 저장하는 변수

    public Vector3 moveDir { get; private set; } // 이동 방향을 저장하는 변수
    public Vector3 lookDir { get; private set; } // 시선 방향을 저장하는 변수

    // Start is called before the first frame update
    private void Start()
    {
        // 컴포넌트 및 객체들 초기화
        player = GameManager.Instance.Player;
        playerCamera = GameManager.Instance.MainCamera;
        m_camera = playerCamera.GetComponent<Camera>();

        // 레이어 마스크 초기화
        layerMask = (1 << LayerMask.NameToLayer("WarpPoint")) |
                    (1 << LayerMask.NameToLayer("Enemy")) |
                    (1 << LayerMask.NameToLayer("Item")) |
                    (1 << LayerMask.NameToLayer("Chest"));
    }
    // Update is called once per frame
    RaycastHit raycastHit;
    private void Update()
    {
        if (GameManager.Instance.IsPlay)
        {
            // 카메라 뷰포트 중앙에서 화면으로 뻗어나가는 레이의 원점 및 방향 계산
            Vector3 rayOrigin = m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            Vector3 rayDir = m_camera.transform.forward;

            // 이동 및 시선 방향 갱신
            moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            lookDir = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

            // 플레이어 및 플레이어 카메라의 각도 및 방향 설정
            player.Angle(playerCamera.transform.localEulerAngles);
            playerCamera.SetDir(lookDir);

            // 입력에 따른 플레이어의 동작 실행
            if (Input.GetButtonDown("Dash")) player.Dash();
            if (Input.GetButtonDown("Jump")) player.Jump();
            if (Input.GetButton("Fire1")) player.Fire1(playerCamera.transform);
            if (Input.GetButtonUp("Fire1")) player.Fire1(playerCamera.transform);
            if (Input.GetButtonDown("Reload")) player.Reload();

            // 상호작용 버튼 입력 및 레이캐스트를 통해 상호작용 가능한 대상 확인
            if (Input.GetButtonDown("Interaction") && Physics.Raycast(rayOrigin, rayDir, out raycastHit, 5f, layerMask))
            {
                if (raycastHit.collider.transform.tag.Equals("Scroll"))
                {
                    player.AddInventory(raycastHit.collider.GetComponent<ScrollObject>().GetData());
                }
                else if (raycastHit.collider.transform.tag.Equals("Weapon"))
                {
                    raycastHit.collider.GetComponent<Weapon>();
                }
                else if (raycastHit.collider.transform.tag.Equals("Warp"))
                {
                    raycastHit.collider.GetComponent<WarpPoint>().Warp();
                }
                else if (raycastHit.collider.transform.tag.Equals("Chest"))
                {
                    raycastHit.collider.GetComponent<Chest>().BoxOpen();
                }
            }
        }
        // 게임이 플레이 중이 아닐 때 플레이어 카메라의 방향 제한
        else playerCamera.SetDir(Vector3.zero);
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPlay)
        {
            // 이동 방향에 따라 플레이어 이동
            player.Move(moveDir.normalized);
        }
    }
}
