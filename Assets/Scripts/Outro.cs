using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Outro : MonoBehaviour
{
    private bool isResult;

    public Text T_result;
    private string result = "도전 성공";

    public Text T_stage;
    private string stage = "스테이지 1-X";

    public Text T_playTime;
    private string playTime = "12분 34초";

    public Text T_dmg;

    public Text T_dmg_value;
    private string dmg_value = "1234567890";

    public Text T_weapon;

    public Text T_weapon_value; // 무기 이미지 들어갈 예정, 텍스트가 아님 (임시)
    private string weapon_value = "파운드리"; // 무기 이미지 들어갈 예정, 텍스트가 아님 (임시)

    public Text T_scroll;

    public Text T_scroll_cnt;
    private string scroll_cnt = "123";

    public Text T_elite;

    public Text T_elite_cnt;
    private string elite_cnt = "456";

    public Text T_credit;

    private void Result_Btn()
    {
        isResult = true;

        T_result.text = result;

        T_stage.text = stage;
        T_playTime.text = playTime;

        T_dmg.text = "DMG";
        T_dmg_value.text = dmg_value;

        T_weapon.text = "Weapon";
        T_weapon_value.text = weapon_value; // 무기 이미지 들어갈 예정, 텍스트가 아님 (임시)

        T_scroll.text = "스크롤 획득량";
        T_scroll_cnt.text = scroll_cnt;

        T_elite.text = "엘리트 몬스터 처치 수";
        T_elite_cnt.text = elite_cnt;

        T_credit.text = "C r e d i t";
    }

    private void Credit_Btn()
    {
        isResult = false;

        T_result.text = "Team2 - TOPOLOGY";

        T_stage.text = "PM";
        T_playTime.text = "백민철";

        T_dmg.text = "DEV";
        T_dmg_value.text = "손세준";

        T_weapon.text = "DEV";
        T_weapon_value.text = "남정운";

        T_scroll.text = "AD";
        T_scroll_cnt.text = "오희재";

        T_elite.text = "ART";
        T_elite_cnt.text = "송창민";

        T_credit.text = "결 과 보 기";
    }

    public void Switch_Btn()
    {
        if (isResult)
        {
            Credit_Btn();
        }
        else
        {
            Result_Btn();
        }
    }

    public void Restart_Btn()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        Result_Btn();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
