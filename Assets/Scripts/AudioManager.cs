using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSourceOneShot;
    public AudioSource audioSourceDefault;

    public AudioClip breakWall;

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
}
