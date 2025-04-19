using UnityEngine;
using UnityEngine.UI;
//using YG;

public class Wallet : MonoBehaviour
{
    public Text starsCounter;
    private int starsInWallet;
    private int starsFromYandexCloud;

    void Start()
    {
        //PlayerPrefs.SetInt("Stars", 1500);
        GetStars();
    }

    public void GetStars()
    {
        starsInWallet = PlayerPrefs.GetInt("Stars");
        starsCounter.text = starsInWallet.ToString();
    }
}
