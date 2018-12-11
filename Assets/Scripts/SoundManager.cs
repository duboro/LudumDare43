using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    public AudioSource efxSource;
    public AudioSource musicSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clip;
        efxSource.Play();
    }
}
