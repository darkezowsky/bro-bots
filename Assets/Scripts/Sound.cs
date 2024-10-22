using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
         
    public bool loop;

    [Range(0f, 1f)]
    public float valume;
    [Range(.1f, 3f)]
    public float pitch;

   [HideInInspector] public AudioSource source;
}
