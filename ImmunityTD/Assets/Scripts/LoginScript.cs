using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    public InputField Input_UserName;
    public InputField Input_PassWord;

    public void OnLoginButtonClicked()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", Input_UserName.text);
        form.AddField("password", Input_PassWord.text);

        UnityWebRequest www = UnityWebRequest.Post("localhost/login.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            // Here you can handle the response from the server
            if (www.downloadHandler.text == "OK")
            {
                OnLoginSuccess();
            }
            else
            {
                OnLoginFailed();
            }
        }
    }

    void OnLoginSuccess()
    {
        Debug.Log("Login successful");
        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }

    void OnLoginFailed()
    {
        Debug.Log("Login failed");
        // Display a message to the user
        // This assumes you have a text element in your UI where you display messages to the user
        GameObject.Find("MessageText").GetComponent<Text>().text = "Login failed. Please check your username and password.";
    }
}
