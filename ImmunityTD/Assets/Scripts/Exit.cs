using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public IApplicationQuitter applicationQuitter = new ApplicationQuitter();

    [SerializeField] public GameObject quitPanel;

    public void openConfirmationMenu()
    {
        quitPanel.SetActive(true);
    }

    public void closeConfirmationMenu()
    {
        quitPanel.SetActive(false);
    }

    public void QuitGame()
    {
        applicationQuitter.Quit();
    }
}

public interface IApplicationQuitter
{
    void Quit();
}

public class ApplicationQuitter : IApplicationQuitter
{
    public void Quit()
    {
        Application.Quit();
    }
}
