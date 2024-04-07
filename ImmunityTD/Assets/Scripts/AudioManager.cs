using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip mainmenu;
    public AudioClip buttonClick;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "StartScreen")
        {
            musicSource.clip = mainmenu;
        }
        else
        {
            if (sceneName == "Game")
            {
                musicSource.clip = background;
            }
        }
        musicSource.volume = 0.1f;
        musicSource.Play();
    }

    public void PlayButtonSound()
    {
        SFXSource.clip = buttonClick;
        SFXSource.volume = 0.1f;
        SFXSource.Play();
    }
}
