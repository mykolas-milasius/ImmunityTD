using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource MusicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip Background;
    public AudioClip Mainmenu;
    public AudioClip ButtonClick;
    public AudioClip VirusDeath;
    public AudioClip TowerShoot;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "StartScreen")
        {
            MusicSource.clip = Mainmenu;
        }
        else
        {
            if (sceneName == "Game")
            {
                MusicSource.clip = Background;
            }
        }
        MusicSource.volume = 0.1f;
        MusicSource.Play();
    }

    public void PlayButtonSound()
    {
        SFXSource.clip = ButtonClick;
        SFXSource.volume = 0.1f;
        SFXSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
