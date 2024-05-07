using UnityEngine;

namespace Assets.Scripts
{
    public class SettingsScript : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject SettingsMenu;

        // Call this function when the Settings Button is clicked
        public void OpenSettingsMenu()
        {
            PauseMenu.SetActive(false);
            SettingsMenu.SetActive(true);
        }

        // Call this function when the Back Button in the Settings menu is clicked
        public void ReturnToPauseMenu()
        {
            SettingsMenu.SetActive(false);
            PauseMenu.SetActive(true);
        }
    }
}