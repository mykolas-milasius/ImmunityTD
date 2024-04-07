using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManagerTests
{
    [UnityTest]
    public IEnumerator AudioManager_PlaysMainMenuMusicOnStart()
    {
        SceneManager.LoadScene("StartScreen");
        yield return null;

        AudioManager audioManager = Object.FindObjectOfType<AudioManager>();

        Assert.IsNotNull(audioManager, "AudioManager component not found in StartScreen scene.");

        Assert.AreEqual(audioManager.musicSource.clip, audioManager.mainmenu, "Music source clip is not set to mainmenu clip on StartScreen.");

        Assert.IsTrue(audioManager.musicSource.isPlaying, "Music source is not playing on StartScreen.");
    }

    [UnityTest]
    public IEnumerator AudioManager_PlaysBackgroundMusicInGameScene()
    {
        SceneManager.LoadScene("Game");

        yield return null;

        AudioManager audioManager = Object.FindObjectOfType<AudioManager>();


        Assert.IsNotNull(audioManager, "AudioManager component not found in Game scene.");

        Assert.AreEqual(audioManager.musicSource.clip, audioManager.background, "Music source clip is not set to background clip in Game scene.");

        Assert.IsTrue(audioManager.musicSource.isPlaying, "Music source is not playing in Game scene.");
    }

    [UnityTest]
    public IEnumerator AudioManager_PlaysButtonClickSound()
    {
        SceneManager.LoadScene("StartScreen");

        yield return null;

        AudioManager audioManager = Object.FindObjectOfType<AudioManager>();

        Assert.IsNotNull(audioManager, "AudioManager component not found.");

        audioManager.PlayButtonSound();

        Assert.AreEqual(audioManager.SFXSource.clip, audioManager.buttonClick, "SFX source clip is not set to buttonClick clip.");

        Assert.IsTrue(audioManager.SFXSource.isPlaying, "SFX source is not playing.");
    }
}
