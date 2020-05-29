using UnityEngine.Audio;
using UnityEngine;
using System;

/*
 *  SoundManager for Unity
 *      developed by James Kerber (2019)
 *      Department of Game Design, Uppsala University, Campus Gotland
 */

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 4f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager i {
        get { return _instance; }
        private set { _instance = value; }
    }
    public Sound[] sounds;

    void Awake()
    {
        // Singleton initialization
        if(i == null) {
            i = this;
        } else {
            Debug.LogWarning("More than one SoundManager present!");
        }

        // Initialize AudioSources
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        LoopSound("BgMusic");
    }

    public void PlayOnce(string name, bool avoidDoubles = false)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // avoid playing twice if requested
        if(!(avoidDoubles && s.source.isPlaying)) {
            s.source.PlayOneShot(s.source.clip, s.volume);
        }
        
    }

    public void LoopSound(string name)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.source.isPlaying)
        {
            return;
        }
        s.source.Play();
    }

    public void StopPlaying(string name)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            return;
        }
        s.source.Stop();
    }

    public void SetPitch(string name, float pitch)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = pitch;
    }

    public Sound GetSound(string name) {
        return Array.Find(sounds, sound => sound.name == name);
    }
}