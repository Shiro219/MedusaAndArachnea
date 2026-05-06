using System;
using UnityEngine;
public class MusicPlayer : MonoBehaviour
{

    public static bool isPlaying = false;
    private AudioSource audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
        if (!isPlaying)
        {
            isPlaying = true;
            audioSource.Play();
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
