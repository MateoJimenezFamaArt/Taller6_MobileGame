using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Load saved volume or set default
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        SetVolume(volumeSlider.value);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // 🔊 Controls the global volume
        PlayerPrefs.SetFloat("Volume", volume); // Saves volume setting
    }
}
