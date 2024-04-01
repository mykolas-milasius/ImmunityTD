using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LoginTests
{
    [UnityTest]
    public IEnumerator TestLoginProcess()
    {
        GameObject loginGameObject = new GameObject();
        Login loginScript = loginGameObject.AddComponent<Login>();

        TMP_InputField usernameInput = new GameObject().AddComponent<TMP_InputField>();
        TMP_InputField passwordInput = new GameObject().AddComponent<TMP_InputField>();
        Button loginButton = new GameObject().AddComponent<Button>();
        TextMeshProUGUI errorMessageText = new GameObject().AddComponent<TextMeshProUGUI>();
        GameObject loginCanvas = new GameObject();
        GameObject mainMenuCanvas = new GameObject();

        loginScript.usernameInput = usernameInput;
        loginScript.passwordInput = passwordInput;
        loginScript.loginButton = loginButton;
        loginScript.errorMessageText = errorMessageText;
        loginScript.loginCanvas = loginCanvas;
        loginScript.mainMenuCanvas = mainMenuCanvas;

        usernameInput.text = "test_username";
        passwordInput.text = "test_password";

        loginButton.onClick.Invoke();

        yield return null;

        Assert.IsFalse(loginScript.isLoggingIn); 
        Assert.IsFalse(loginButton.interactable); 

        Assert.IsFalse(loginCanvas.activeSelf); 
        Assert.IsTrue(mainMenuCanvas.activeSelf); 
    }

    [UnityTest]
    public IEnumerator TestInputValidation()
    {
        GameObject loginGameObject = new GameObject();
        Login loginScript = loginGameObject.AddComponent<Login>();

        TMP_InputField usernameInput = new GameObject().AddComponent<TMP_InputField>();
        TMP_InputField passwordInput = new GameObject().AddComponent<TMP_InputField>();
        Button loginButton = new GameObject().AddComponent<Button>();
        TextMeshProUGUI errorMessageText = new GameObject().AddComponent<TextMeshProUGUI>();
        GameObject loginCanvas = new GameObject();
        GameObject mainMenuCanvas = new GameObject();

        loginScript.usernameInput = usernameInput;
        loginScript.passwordInput = passwordInput;
        loginScript.loginButton = loginButton;
        loginScript.errorMessageText = errorMessageText;
        loginScript.loginCanvas = loginCanvas;
        loginScript.mainMenuCanvas = mainMenuCanvas;

        usernameInput.text = ""; 
        passwordInput.text = "test_password";

        loginButton.onClick.Invoke();

        yield return null;

        Assert.IsTrue(errorMessageText.gameObject.activeSelf); 

        usernameInput.text = "test_username";
        passwordInput.text = ""; 

        loginButton.onClick.Invoke();

        yield return null;

        Assert.IsTrue(errorMessageText.gameObject.activeSelf); 
    }

    [UnityTest]
    public IEnumerator TestErrorMessageDisplay()
    {
        GameObject loginGameObject = new GameObject();
        Login loginScript = loginGameObject.AddComponent<Login>();

        TMP_InputField usernameInput = new GameObject().AddComponent<TMP_InputField>();
        TMP_InputField passwordInput = new GameObject().AddComponent<TMP_InputField>();
        Button loginButton = new GameObject().AddComponent<Button>();
        TextMeshProUGUI errorMessageText = new GameObject().AddComponent<TextMeshProUGUI>();
        GameObject loginCanvas = new GameObject();
        GameObject mainMenuCanvas = new GameObject();

        loginScript.usernameInput = usernameInput;
        loginScript.passwordInput = passwordInput;
        loginScript.loginButton = loginButton;
        loginScript.errorMessageText = errorMessageText;
        loginScript.loginCanvas = loginCanvas;
        loginScript.mainMenuCanvas = mainMenuCanvas;

        usernameInput.text = "test_username";
        passwordInput.text = "test_password";

        loginButton.onClick.Invoke();

        yield return null;

        Assert.IsTrue(errorMessageText.gameObject.activeSelf); 

        yield return new WaitForSeconds(6); 

        Assert.IsFalse(errorMessageText.gameObject.activeSelf); 
    }
}
