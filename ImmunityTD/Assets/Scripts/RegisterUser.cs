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
    public TextMeshProUGUI confirmationMessage; // Reference to the UI element for confirmation messages
    public TextMeshProUGUI passwordWarningText;
    public TextMeshProUGUI confirmPasswordWarningText;
    public TextMeshProUGUI errorMessageText; // Reference to the UI element for error messages




    // Enter server URL here
    private string registerUrl = "http://localhost/immunityTD.php";

    private bool isRegistering = false;

    void Awake()
    {
        registerButton.onClick.AddListener(Register);
        passwordInput.onValueChanged.AddListener(UpdatePasswordStrengthText);
        confirmationMessage.gameObject.SetActive(false); // Hide confirmation message initially
        errorMessageText.gameObject.SetActive(false); // Hide error message initially
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
        string passwordStrength = EvaluatePasswordStrength(passwordInput.text);
        if (passwordStrength == "Very Weak" || passwordStrength == "Weak" || passwordStrength == "Password is too weak")
        {
            passwordWarningText.text = passwordStrength;
            passwordWarningText.gameObject.SetActive(true);
            return;
        }

        if (!IsPasswordStrong(password))
        {
            passwordWarningText.text = "Password is not strong enough";
            passwordWarningText.gameObject.SetActive(true);
            return;
        }
        else
        {
            passwordWarningText.gameObject.SetActive(false);
        }

        if (password != confirmPassword)
        {
            confirmPasswordWarningText.text = "Passwords do not match";
            confirmPasswordWarningText.gameObject.SetActive(true);
            return;
        }
        else
        {
            confirmPasswordWarningText.gameObject.SetActive(false);
        }

        isRegistering = true;
        registerButton.interactable = false;

        StartCoroutine(RegisterOnServer(username, password));
    }

    public void UpdatePasswordStrengthText(string password)
    {
        string passwordStrength = EvaluatePasswordStrength(password);
        // Customize the feedback for each strength level
        switch (passwordStrength)
        {
            case "Very Weak":
                passwordWarningText.text = "Strength: Very Weak - Password is too simple.";
                break;
            case "Weak":
                passwordWarningText.text = "Strength: Weak - Consider adding numbers and symbols.";
                break;
            case "Medium":
                passwordWarningText.text = "Strength: Medium - Good, but could be stronger.";
                break;
            case "Strong":
                passwordWarningText.text = "Strength: Strong - Your password is strong.";
                break;
            default:
                passwordWarningText.text = "Strength: Very Weak - Password is too simple.";
                break;
        }
        passwordWarningText.gameObject.SetActive(true);
    }


    private bool IsPasswordStrong(string password)
    {
        // Implement your own password strength criteria here
        // Example: Minimum 8 characters, at least one letter and one number
        return password.Length >= 8 && System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-zA-Z]).{8,}$");
    }
    private string EvaluatePasswordStrength(string password)
    {
        int strengthScore = 0;

        // Check for common weak patterns
        if (password == "123" || password == "password" || password.Length < 4)
        {
            return "Password is too weak";
        }

        // Strength criteria checks
        if (password.Length >= 8) strengthScore++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"\d")) strengthScore++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, "[a-zA-Z]")) strengthScore++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#\$%\^&\*\(\)_\+\-=\[\]\{\};':""\\|,.<>\/\?`~]")) strengthScore++;

        // Password strength evaluation
        switch (strengthScore)
        {
            case 0:
            case 1:
                return "Very Weak";
            case 2:
                return "Weak";
            case 3:
                return "Medium";
            case 4:
                return "Strong";
            default:
                return "Unknown";
        }
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
                ShowError("Error in registration: " + www.error); // Show error using the new method
            }
            else
            {
                string responseText = www.downloadHandler.text.Trim();

                if (responseText.Contains("Username already exists"))
                {
                    ShowError("Registration failed: Username already exists.");
                }
                else
                {
                    Debug.Log("Registration successful! Response: " + responseText);
                    ShowConfirmation("Registration successful!");
                }
            }
        }
        isRegistering = false;
        registerButton.interactable = true;
    }

    private void ShowError(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        confirmationMessage.gameObject.SetActive(false); // Hide the confirmation message if an error is shown
                                                         // Optionally, hide the error message after a few seconds
        Invoke("HideErrorMessage", 5);
    }

    private void HideErrorMessage()
    {
        errorMessageText.gameObject.SetActive(false);
    }

    private void ShowConfirmation(string message)
    {
        confirmationMessage.text = message;
        confirmationMessage.gameObject.SetActive(true);
        errorMessageText.gameObject.SetActive(false); // Hide the error message if a confirmation is shown
                                                      // Optionally, hide the message after a few seconds
        Invoke("HideConfirmationMessage", 5);
    }

    private void HideConfirmationMessage()
    {
        confirmationMessage.gameObject.SetActive(false);
    }

}
