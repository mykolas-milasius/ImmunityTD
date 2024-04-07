using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI errorMessageText;
    public GameObject loginCanvas;
    public GameObject mainMenuCanvas;
    private string loginUrl = "http://localhost/immunityTDLogin.php";
    public bool isLoggingIn = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void Awake()
    {
        loginButton.onClick.AddListener(Loginn);
        errorMessageText.gameObject.SetActive(false);
    }

    public void Loginn()
    {
        if (isLoggingIn)
        {
            Debug.LogWarning("Login in progress...");
            return;
        }

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("User name is empty");
            return;
        }

        isLoggingIn = true;
        loginButton.interactable = false;

        StartCoroutine(LoginOnServer(username, password));
    }

    public IEnumerator LoginOnServer(string userName, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", userName);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning("Error in login: " + www.error);
                ShowInfo("Error in login: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text.Trim();

                if (responseText.Contains("Invalid username or password"))
                {
                    ShowInfo("Login failed: Invalid username or password.");
                }
                else
                {
                    Debug.Log("Login successful! Response: " + responseText);
                    ShowInfo("Login successful!");
                    Invoke("SwitchToMainMenu", 3f);
                }
            }
        }
        isLoggingIn = false;
        loginButton.interactable = true;
    }

    private void ShowInfo(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        Invoke("HideErrorMessage", 5);
    }

    private void SwitchToMainMenu()
    {
        loginCanvas.SetActive(false); 
        mainMenuCanvas.SetActive(true); 
    }
}
