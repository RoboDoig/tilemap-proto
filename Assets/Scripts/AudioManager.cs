using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSourceOneShot;
    public AudioSource audioSourceDefault;

    public AudioClip breakWall;
    public AudioClip guardKO;
    public AudioClip lose;
    public AudioClip lostWin;
    public AudioClip foundWin;
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


    public void AudioLostWin() {
        audioSourceOneShot.PlayOneShot(lostWin);
    }

    public void AudioFoundWin() {
        audioSourceOneShot.PlayOneShot(foundWin);
    }

    public void AudioSwitch() {
        audioSourceOneShot.PlayOneShot(switchToggle);
    }

    public void AudioLose() {
        audioSourceOneShot.PlayOneShot(lose);
        GameObject background = GameObject.FindGameObjectWithTag("BackgroundMusic");
        if (background != null) {
            background.GetComponent<AudioSource>().volume = 0;
        }
    }
}
