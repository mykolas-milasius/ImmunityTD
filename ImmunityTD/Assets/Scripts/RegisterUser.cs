using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RegisterScript : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public Button registerButton;

    // Enter server URL here
    private string registerUrl = "http://localhost/register.php";

    private bool isRegistering = false;

    void Awake()
    {
        registerButton.onClick.AddListener(Register);
    }

    public void Register()
    {
        if (isRegistering)
        {
            Debug.LogWarning("Registration in progress...");
            return;
        }

        string username = usernameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("User name is empty");
            return;
        }

        if (password != confirmPassword)
        {
            Debug.LogWarning("Passwords do not match");
            return;
        }

        isRegistering = true;

        registerButton.interactable = false;

        StartCoroutine(RegisterOnServer(username, password));
    }

    private IEnumerator RegisterOnServer(string userName, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", userName);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning("Error in registration: " + www.error);
            }
            else
            {
                Debug.Log("Registration successful! Response: " + www.downloadHandler.text);
            }
        }

        isRegistering = false;
        registerButton.interactable = true;
    }
}
