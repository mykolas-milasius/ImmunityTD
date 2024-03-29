using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

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
}
