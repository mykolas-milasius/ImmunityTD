using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused = false;
    void Start()
    {
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) { // prideti kad paspaudus pause mygtuka
            if (isPaused) {
                DisablePauseMenu();
            } else {
                EnablePauseMenu();
            }
        }
    }

    public void EnablePauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void DisablePauseMenu(){
        isPaused = false;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }
}
