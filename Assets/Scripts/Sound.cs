using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void EventSound(Object go)
    {
        audioSource.clip = (AudioClip) go;
        audioSource.PlayOneShot((AudioClip) go);
    }
}
