using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject[] scenes;

    public void Start_Btn()
    {
        SceneManager.LoadScene("Loading");
        var op = SceneManager.LoadSceneAsync("Start", LoadSceneMode.Additive);
        op.completed += (x) => SceneManager.UnloadSceneAsync("Loading");
    }
    public void Quit_Btn(GameObject image)
    {
        image.gameObject.SetActive(true);
    }
    public void Popup_Btn_Y()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Popup_Btn_N(GameObject image)
    {
        image.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        int rnd = Random.Range(0, scenes.Length);
        scenes[rnd].SetActive(true);
        scenes[rnd].GetComponentInChildren<Camera>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
