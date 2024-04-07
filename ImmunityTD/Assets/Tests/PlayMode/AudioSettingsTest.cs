using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsTest
{
    private GameObject audioSettingsObject;
    private AudioSettings audioSettings;
    private AudioMixer audioMixer;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the AudioSettings component to it
        audioSettingsObject = new GameObject();
        audioSettings = audioSettingsObject.AddComponent<AudioSettings>();

        // Load a test audio mixer from the Resources folder and assign it to the myMixer field
        audioMixer = Resources.Load<AudioMixer>("Audio/NewAudioMixer");
        audioSettings.myMixer = audioMixer;

        // Create separate GameObjects for each Slider and add them as children of audioSettingsObject
        GameObject masterSliderObject = new GameObject("MasterSlider");
        masterSliderObject.transform.parent = audioSettingsObject.transform;
        audioSettings.masterSlider = masterSliderObject.AddComponent<Slider>();

        GameObject musicSliderObject = new GameObject("MusicSlider");
        musicSliderObject.transform.parent = audioSettingsObject.transform;
        audioSettings.musicSlider = musicSliderObject.AddComponent<Slider>();

        GameObject SFXSliderObject = new GameObject("SFXSlider");
        SFXSliderObject.transform.parent = audioSettingsObject.transform;
        audioSettings.SFXSlider = SFXSliderObject.AddComponent<Slider>();

        GameObject UISliderObject = new GameObject("UISlider");
        UISliderObject.transform.parent = audioSettingsObject.transform;
        audioSettings.UISlider = UISliderObject.AddComponent<Slider>();
    }




    [TearDown]
    public void Teardown()
    {
        // Clean up created objects after each test
        Object.Destroy(audioSettingsObject);
    }

    [Test]
    public void SetMasterVolume_SetsMasterVolumeCorrectly()
    {
        // Arrange
        Slider masterSlider = audioSettingsObject.AddComponent<Slider>();
        audioSettings.masterSlider = masterSlider;

        // Act
        audioSettings.masterSlider.value = 0.5f;
        audioSettings.SetMasterVolume();

        // Assert
        float masterVolume;
        audioSettings.myMixer.GetFloat("MasterVolume", out masterVolume);
        Assert.AreEqual(Mathf.Log10(0.5f) * 20, masterVolume);
    }

    [Test]
    public void SetMusicVolume_SetsMusicVolumeCorrectly()
    {
        // Arrange
        Slider musicSlider = audioSettingsObject.AddComponent<Slider>();
        audioSettings.musicSlider = musicSlider;

        // Act
        audioSettings.musicSlider.value = 0.7f;
        audioSettings.SetMusicVolume();

        // Assert
        float musicVolume;
        audioSettings.myMixer.GetFloat("MusicVolume", out musicVolume);
        Assert.AreEqual(Mathf.Log10(0.7f) * 20, musicVolume);
    }

    [Test]
    public void SetSFXVolume_SetsSFXVolumeCorrectly()
    {
        // Arrange
        Slider SFXSlider = audioSettingsObject.AddComponent<Slider>();
        audioSettings.SFXSlider = SFXSlider;

        // Act
        audioSettings.SFXSlider.value = 0.3f;
        audioSettings.SetSFXVolume();

        // Assert
        float SFXVolume;
        audioSettings.myMixer.GetFloat("SFXVolume", out SFXVolume);
        Assert.AreEqual(Mathf.Log10(0.3f) * 20, SFXVolume);
    }

    [Test]
    public void SetUIVolume_SetsUIVolumeCorrectly()
    {
        // Arrange
        Slider UISlider = audioSettingsObject.AddComponent<Slider>();
        audioSettings.UISlider = UISlider;

        // Act
        audioSettings.UISlider.value = 0.8f;
        audioSettings.SetUIVolume();

        // Assert
        float UIVolume;
        audioSettings.myMixer.GetFloat("UIVolume", out UIVolume);
        Assert.AreEqual(Mathf.Log10(0.8f) * 20, UIVolume);
    }
}
