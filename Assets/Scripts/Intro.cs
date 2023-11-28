using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Image popup;

    public void Start_Btn()
    {
        SceneManager.LoadScene("Start");
    }
    public void Quit_Btn()
    {
        popup.gameObject.SetActive(true);
    }
    public void Popup_Btn_Y()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Popup_Btn_N()
    {
        popup.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        popup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
