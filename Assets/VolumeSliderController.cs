using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliderController : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;
    public string volumeParameter = "MasterVolume";

    [Header("UI")]
    public Slider volumeSlider;

    [SerializeField] private string PlayerPrefsKey = "MasterVolumeValue";


    void Start()
    {
        // Load saved volume or default to 0.75 if none exists
        float savedVolume = PlayerPrefs.GetFloat(PlayerPrefsKey, 1f);

        // Set slider value without triggering the onValueChanged event
        //volumeSlider.SetValueWithoutNotify(savedVolume);

        Debug.Log("Loaded volume: " + savedVolume);

        // Apply to mixer
        SetVolume(savedVolume);

        // Update slider UI
        volumeSlider.value = savedVolume;

        // Listen for slider changes
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);


    }

    private void OnSliderValueChanged(float value)
    {
        SetVolume(value);
        PlayerPrefs.SetFloat(PlayerPrefsKey, value);
    }

    private void SetVolume(float value)
    {
        // Protect against log(0)
        if (value <= 0.0001f) value = 0.0001f;

        float dB = Mathf.Log10(value) * 20f;
        Debug.Log("Setting volume to: " + dB + " dB for slider value: " + value);
        mixer.SetFloat(volumeParameter, dB);

        mixer.GetFloat(volumeParameter, out float checkValue);
        Debug.Log("Mixer stored this: " + checkValue);

    }
}
