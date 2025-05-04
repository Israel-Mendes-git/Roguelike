using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;
    public Dictionary<string, AudioClip[]> soundClips;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            List<AudioClip> clips = soundDictionary[soundName];
            if (clips != null && clips.Count > 0)
            {
                return clips[UnityEngine.Random.Range(0, clips.Count)];
            }
        }

        Debug.LogWarning($"Sound '{soundName}' not found or list is empty.");
        return null;
    }


    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> audioClips;
    }
}
