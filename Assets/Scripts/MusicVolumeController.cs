using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (AudioManager.Instance != null && musicSlider != null)
        {
            musicSlider.value = AudioManager.Instance.BackgroundSound.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
    }
}
