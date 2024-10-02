using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;

    private static float playRate = 0.5f;     //can be able to play every 1 sec
    private static float nextPlayTime = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name, float volumeScale)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            if(Time.time > nextPlayTime)
            {
                print("Play in Audio");
                //used to not let the sound play if input value (all horizontal and vertical) is not 0
                //used to solve if input value is not 0 and the sound (walk, climp, landing) still play while dying
                nextPlayTime = Time.time + playRate;
                sfxSource.PlayOneShot(s.clip, volumeScale);
            }
            
        }
    }
    public AudioClip SearchSFX(String name)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);
        AudioClip _sfx = null;
        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            _sfx = s.clip;
        }
        return _sfx;
    }

    public void PlaySFXInObject(AudioSource sfx, String name)
    {
        sfx.clip = SearchSFX(name);
        sfx.Play();

        //Setting volume
        sfx.volume = sfxSource.volume;
    }
    public void StopSFXInObject(AudioSource sfx)
    {
        sfx.Stop();
    }
}