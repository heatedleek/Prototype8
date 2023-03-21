using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The audio manager
/// </summary>
public static class AudioManager
{
    static AudioSource audioSource;
    static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();
    static Dictionary<AudioClipName, List<AudioClip>> audioRange =
    new Dictionary<AudioClipName, List<AudioClip>>();
    static bool initialized = false;

    /// <summary>
    /// Initializes the audio manager
    /// </summary>
    /// <param name="source">audio source</param>
    public static void Initialize(AudioSource source)
    {
        audioSource = source;
        if(!initialized)
        {

            audioClips.Add(AudioClipName.Turn,
                Resources.Load<AudioClip>("Turn"));
            audioClips.Add(AudioClipName.Jump,
                Resources.Load<AudioClip>("Jump"));
            audioClips.Add(AudioClipName.Land,
                Resources.Load<AudioClip>("Land"));
            audioClips.Add(AudioClipName.TickTock,
                Resources.Load<AudioClip>("TickTock"));
            audioClips.Add(AudioClipName.Death,
                Resources.Load<AudioClip>("Death"));
            audioClips.Add(AudioClipName.Goal,
                Resources.Load<AudioClip>("Goal"));
            initialized = true;
        }
    }

    /// <summary>
    /// Plays the audio clip with the given name
    /// </summary>
    /// <param name="name">name of the audio clip to play</param>
    public static void Play(AudioClipName name)
    {
        audioSource.PlayOneShot(audioClips[name]);
    }
    public static void PlayRandom(AudioClipName name)
    {
        audioSource.PlayOneShot(audioRange[name][Random.Range(0, audioRange[name].Count)]);
    }
}
