using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public AudioClip titleMusic;
    public AudioClip gameMusic;
    public AudioSource audioSource;
    void Awake() {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

        audioSource.clip = titleMusic;
        audioSource.Play();
    }

    public void GameStart() {
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    public void TitleReturn() {
        audioSource.Stop();
        audioSource.clip = titleMusic;
        audioSource.Play();
    }
}
