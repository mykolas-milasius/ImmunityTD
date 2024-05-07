using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Exit : MonoBehaviour
    {
        public IApplicationQuitter ApplicationQuitter = new ApplicationQuitter();

        [SerializeField] public GameObject QuitPanel;

        public void OpenConfirmationMenu()
        {
            QuitPanel.SetActive(true);
        }

        public void CloseConfirmationMenu()
        {
            QuitPanel.SetActive(false);
        }

        public void QuitGame()
        {
            ApplicationQuitter.Quit();
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
}
