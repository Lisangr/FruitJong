using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI starsCounter;
    private int starsInWallet;

    void Start()
    {
        starsInWallet = PlayerPrefs.GetInt("Stars");
        starsCounter.text = starsInWallet.ToString();
    }
}
