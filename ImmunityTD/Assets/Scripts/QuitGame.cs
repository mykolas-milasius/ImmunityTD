using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] GameObject quitPanel; // Quit_Confirmation_Panel

    public void OpenConfirmationMenu()
    {

        quitPanel.SetActive(true); 
    }


    public void CloseConfirmationMenu()
    {
        quitPanel.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
