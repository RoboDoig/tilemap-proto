using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSourceOneShot;
    public AudioSource audioSourceDefault;
    public AudioSource audioSourceMusic;

    public AudioClip breakWall;
    public AudioClip guardKO;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip switchToggle;

    public AudioClip foundWalk;
    public AudioClip lostWalk;
    public AudioClip guardWalk;

    void Awake() {
        if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSourceMusic.Play();
    }

    public void PlayFoundWalk() {
        audioSourceDefault.clip = foundWalk;
        audioSourceDefault.Play();
    }

    public void PlayLostWalk() {
        audioSourceDefault.clip = lostWalk;
        audioSourceDefault.Play();
    }

    public void PlayGuardWalk() {
        audioSourceDefault.clip = guardWalk;
        audioSourceDefault.Play();
    }
    
    public void StopSound() {
        audioSourceDefault.Stop();
    }

    public void AudioWallBreak() {
        audioSourceOneShot.PlayOneShot(breakWall);
    }

    public void AudioGuardKO() {
        audioSourceOneShot.PlayOneShot(guardKO);
    }


    public void AudioWin() {
        audioSourceOneShot.PlayOneShot(win);
    }

    public void AudioSwitch() {
        audioSourceOneShot.PlayOneShot(switchToggle);
    }

    public void AudioLose() {
        audioSourceOneShot.PlayOneShot(lose);
        audioSourceMusic.Stop();
    }
}
