using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class Outro : MonoBehaviour
{
    private bool isResult = GameManager.Instance.StageManager.gameClear;

    public Canvas[] result;

    public TMP_Text T_credit;

    public TMP_Text T_stage;
    public AudioClip successClip;
    public AudioClip failureClip;

    private AudioSource audioSource;
    private string stage = "Play Time"; // 시간 상 스테이지 이름 출력에서 걸린 시간으로 변경
    // private int stageIdx = GameManager.Instance.StageManager.StageIdx;
    // private void StageName()
    // {
    //     int temp = stageIdx - 1;
    //     switch (temp)
    //     {
    //         case 0:
    //             stage = "스테이지 1-1";
    //             break;
    //         case 1:
    //             stage = "스테이지 1-2";
    //             break;
    //         case 2:
    //             stage = "스테이지 1-3";
    //             break;
    //         case 3:
    //             stage = "스테이지 1-4";
    //             break;
    //         case 4:
    //             stage = "보스 스테이지";
    //             break;
    //     }
    // }

    public TMP_Text T_playTime;
    private float playTime = GameManager.Instance.StageManager.playTime; // 걸린 시간

    public TMP_Text T_dmg;

    public TMP_Text T_dmg_value;
    private float dmg_value = GameManager.Instance.StageManager.MaxDamage; // 최대 대미지

    public TMP_Text T_weapon;

    public Image T_weapon_image;
    public TMP_Text T_devMember;

    public TMP_Text T_scroll;

    public TMP_Text T_scroll_cnt;
    private int scroll_cnt = GameManager.Instance.Player.scroll_cnt;

    public TMP_Text T_btn;
    private bool isCredit;

    private void Result_Btn()
    {
        // StageName();

        result[0].gameObject.SetActive(false);
        result[1].gameObject.SetActive(false);
        if (isResult)
        {
            result[0].gameObject.SetActive(true);
            PlayAudio(successClip); // 성공 사운드 재생
        }
        else
        {
            result[1].gameObject.SetActive(true);
            PlayAudio(failureClip); // 실패 사운드 재생
        }
        result[2].gameObject.SetActive(false);

        T_stage.text = stage;
        T_playTime.text = playTime.ToString() + "초";

        T_dmg.text = "DMG";
        T_dmg_value.text = dmg_value.ToString();

        T_weapon.text = "Weapon";
        T_weapon_image.gameObject.SetActive(true);
        T_devMember.gameObject.SetActive(false);

        T_scroll.text = "Scroll Count";
        T_scroll_cnt.text = scroll_cnt.ToString();

        T_btn.text = "C r e d i t";
        isCredit = false;
    }

    private void Credit_Btn()
    {

        // T_result.text = "C r e d i t";

        result[0].gameObject.SetActive(false);
        result[1].gameObject.SetActive(false);
        result[2].gameObject.SetActive(true);

        T_stage.text = "TEAM 2";
        T_playTime.text = "TOPOLOGY";

        T_dmg.text = "Reference";
        T_dmg_value.text = "Gunfire Reborn";

        T_weapon.text = "DEV";
        T_devMember.text = "백민철, 손세준, 남정운";
        T_weapon_image.gameObject.SetActive(false);
        T_devMember.gameObject.SetActive(true);

        T_scroll.text = "ART";
        T_scroll_cnt.text = "오희재, 송창민";

        T_btn.text = "결 과 보 기";
        isCredit = true;
    }

    public void Switch_Btn()
    {
        if (!isCredit)
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
        GameManager.Instance.MoveGameObjectToScene();
        SceneManager.LoadScene("Loading");
        var op = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Additive);
        op.completed += (x) =>
        {
            SceneManager.UnloadSceneAsync("Loading");
            SceneManager.UnloadSceneAsync("Outro");
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        playTime = GameManager.Instance.StageManager.playTime;
        dmg_value = GameManager.Instance.StageManager.MaxDamage;
        scroll_cnt = GameManager.Instance.Player.scroll_cnt;
        // stageIdx = GameManager.Instance.StageManager.StageIdx;
        isResult = GameManager.Instance.StageManager.gameClear;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Result_Btn();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }
}
