using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip jump0;
    public AudioClip jump1;

    bool isJump;
    // Update is called once per frame
    void Update()
    {
        if (!isJump && Input.GetButtonDown("Jump"))
        {
            audioSource.clip = jump0;
            audioSource.Play();
            isJump = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(isJump)
        {
            audioSource.clip = jump0;
            audioSource.Play();
            isJump = false;
        }
    }
}
