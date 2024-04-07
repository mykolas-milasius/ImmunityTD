using UnityEngine;
using UnityEngine.UI; // Make sure you are using the UI namespace
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI HighScoresOutPut; // Assign this in the inspector with your Text element

    void Start()
    {
        DisplayHighScores();
    }

    public void DisplayHighScores()
    {
        string[] highScores = GetHighScores();
        HighScoresOutPut.text = "High Scores\n";
        foreach (string score in highScores)
        {
            HighScoresOutPut.text += score + "\n";
        }
    }

    // This is a placeholder for your method to get high scores
    private string[] GetHighScores()
    {
        // Replace this with actual score fetching logic
        return new string[] { "1000", "900", "800", "700", "600" };
    }
}
