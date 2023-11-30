using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashEffect : MonoBehaviour
{
    public Image D_effect;
    public GameObject Character;

    // Start is called before the first frame update
    void Start()
    {
        // 대시 효과의 초기 상태가 올바르게 설정되도록 확인합니다.
        DashImageOnOff();
    }

    // Update is called once per frame
    void Update()
    {
        // isDash가 true인지 확인하고 대시 효과를 업데이트합니다.
        DashImageOnOff();
    }

    private void DashImageOnOff()
    {
        // Character 스크립트에 isDash가 불리언 속성으로 가정합니다.
        bool isDash = Character.GetComponent<Character>().isDash;

        // isDash 값에 따라 Dash 효과 이미지의 활성화 또는 비활성화를 설정합니다.
        D_effect.gameObject.SetActive(isDash);
    }
}

