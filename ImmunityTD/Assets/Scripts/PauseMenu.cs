using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused = false;
    private IInputHandler inputHandler;

    void Awake()
    {
        inputHandler = new UnityInputHandler();
    }

    public void Update()
    {
        if (inputHandler.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void SetInputHandlerForTesting(IInputHandler mockInputHandler)
    {
        inputHandler = mockInputHandler;
    }
}

public interface IInputHandler
{
    bool GetKeyDown(KeyCode key);
}

public class UnityInputHandler : IInputHandler
{
    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}
