using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public AudioSource GutynSound;
    [SerializeField] public AudioSource BackgroundSound;
    [SerializeField] public AudioSource MenuMusic;
    [SerializeField] public Detection_controller detectionArea;

    private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        HandleSceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneMusic(scene.name);

        // Procura o slider de música mesmo que desativado
        Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (Slider slider in allSliders)
        {
            if (slider.CompareTag("MusicSlider"))
            {
                musicSlider = slider;
                musicSlider.onValueChanged.RemoveAllListeners();
                musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(musicSlider.value); });

                float currentVolume = MenuMusic.isPlaying ? MenuMusic.volume : BackgroundSound.volume;
                musicSlider.value = currentVolume;
                break;
            }
        }
    }

    private void HandleSceneMusic(string scene)
    {
        if (scene == "MainMenu")
        {
            MenuMusic?.Play();
            BackgroundSound?.Stop();
            GutynSound?.Stop();
        }
        else
        {
            MenuMusic?.Stop();
            BackgroundSound?.Play();
            GutynSound?.Pause();
        }
    }

    private void Update()
    {
        if (detectionArea != null)
        {
            if (detectionArea.detectedObjs.Count > 0)
            {
                BackgroundSound?.Pause();
                GutynSound?.UnPause();
            }
            else
            {
                BackgroundSound?.UnPause();
                GutynSound?.Pause();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (BackgroundSound != null) BackgroundSound.volume = volume;
        if (GutynSound != null) GutynSound.volume = volume;
        if (MenuMusic != null) MenuMusic.volume = volume;
    }
}
