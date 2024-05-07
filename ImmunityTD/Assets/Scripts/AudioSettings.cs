using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] public AudioMixer MyMixer;
        [SerializeField] public Slider MasterSlider;
        [SerializeField] public Slider MusicSlider;
        [SerializeField] public Slider SFXSlider;
        [SerializeField] public Slider UISlider;

        public void SetMasterVolume()
        {
            float volume = MasterSlider.value;
            MyMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume()
        {
            float volume = MusicSlider.value;
            MyMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        public void SetSFXVolume()
        {
            float volume = SFXSlider.value;
            MyMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }

        public void SetUIVolume()
        {
            float volume = UISlider.value;
            MyMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
        }
    }
}