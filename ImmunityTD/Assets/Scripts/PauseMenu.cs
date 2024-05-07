using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseMenuObject;
        public GameObject PauseButton;
        public bool IsPaused = false;
        private IInputHandler _inputHandler;

        void Awake()
        {
            _inputHandler = new UnityInputHandler();
        }

        public void Update()
        {
            if (_inputHandler.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            if (IsPaused)
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
            IsPaused = true;
            Time.timeScale = 0;
            PauseMenuObject.SetActive(true);
            PauseButton.SetActive(false);
        }

        public void ResumeGame()
        {
            IsPaused = false;
            Time.timeScale = 1;
            PauseMenuObject.SetActive(false);
            PauseButton.SetActive(true);
        }

        public void SetInputHandlerForTesting(IInputHandler mockInputHandler)
        {
            _inputHandler = mockInputHandler;
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
}
