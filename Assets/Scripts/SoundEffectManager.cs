using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    private static Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        if (soundEffectLibrary == null || audioSource == null)
            return;

        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
            audioSource.PlayOneShot(audioClip);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tenta achar o slider mesmo que esteja desativado
        Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (Slider slider in allSliders)
        {
            if (slider.CompareTag("SFXSlider"))
            {
                sfxSlider = slider;
                sfxSlider.onValueChanged.RemoveAllListeners();
                sfxSlider.onValueChanged.AddListener(delegate { SetVolume(sfxSlider.value); });
                sfxSlider.value = audioSource.volume;
                break;
            }
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
