using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float coins = 100f;
    public TextMeshProUGUI coinsText;

    public void FixedUpdate()
    {
        coinsText.text = coins.ToString();
    }
}
