using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] audioClips;
    public AudioSource musicSource, sfxSource;
    // Start is called before the first frame update
    
    void Start()
    {
        musicSource.clip = audioClips[0];
        musicSource.Play();
    }

    // Update is called once per frame
    public void PlaySFX(int index)
    {
        sfxSource.PlayOneShot(audioClips[index]);
    }
}
