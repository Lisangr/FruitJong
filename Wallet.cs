using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI starsCounter;
    private int starsInWallet;

    void Start()
    {
        starsInWallet = PlayerPrefs.GetInt("Stars");
        //PlayerPrefs.SetInt("Stars", starsInWallet);
        //PlayerPrefs.Save();
        starsCounter.text = starsInWallet.ToString();
    }
}
