using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DashEffect : MonoBehaviour
{
    public GameObject D_effect;
    public GameObject Character;

    void Start()
    {
        D_effect.SetActive(false);
    }
    void Update()
    {
        // isDash가 true인지 확인하고 대시 효과를 업데이트합니다.
        DashImageOnOff();
    }

    private void DashImageOnOff()
    {
        // Character 스크립트에 isDash가 불리언 속성으로 가정합니다.
        bool isDash = Character.GetComponent<Character>().isDash;
        if (isDash == true)
        {
            D_effect.SetActive(true);
            AudioSource D_Sound = GetComponent<AudioSource>();
            D_Sound.Play();

        }
        else D_effect.SetActive(false);
    }
}

