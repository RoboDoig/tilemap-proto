using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip titleMusic;
    public AudioClip gameMusic;
    public AudioSource audioSource;
    void Awake() {
        DontDestroyOnLoad(this.gameObject);

        audioSource.clip = titleMusic;
        audioSource.Play();
    }

    public void GameStart() {
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }
}
