using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Audio;

public class OnOff : MonoBehaviour
{
    public GameObject targetUI;
    public AudioClip soundWhenFalse;
    public AudioClip soundWhenTrue;
    AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryOnOff();
        }
    }
    private void InventoryOnOff()
    {
        if (targetUI.activeSelf)
        {
            targetUI.SetActive(false);
            PlaySound(soundWhenFalse);
        }
        else
        {
            targetUI.SetActive(true);
            PlaySound(soundWhenTrue);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
