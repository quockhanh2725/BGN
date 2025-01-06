using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControler_UI : MonoBehaviour
{
    [SerializeField] private string mixerParametr;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private float sliderMultiplier;


    public void SetUpVolumeSlider()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = .0001f;
        slider.value = PlayerPrefs.GetFloat(mixerParametr, slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(mixerParametr, slider.value);
    }
    private void SliderValue(float value)
    {
        audioMixer.SetFloat(mixerParametr, Mathf.Log10(value) * sliderMultiplier);
    }

}
