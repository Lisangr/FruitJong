using TMPro;
using UnityEngine;
using YG;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI starsCounter;
    private int starsInWallet;
    private int starsFromYandexCloud;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadSaveFromCloud;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= LoadSaveFromCloud;
    }

    public void LoadSaveFromCloud()
    {
        starsFromYandexCloud = YandexGame.savesData.stars;
        Debug.Log("загружено звезд из облака" + starsFromYandexCloud);

        if (starsFromYandexCloud > PlayerPrefs.GetInt("Stars"))
        {
            starsInWallet = starsFromYandexCloud;
            starsCounter.text = starsInWallet.ToString();
        }
        else
        {
            starsInWallet = PlayerPrefs.GetInt("Stars");
            starsCounter.text = starsInWallet.ToString();
            YandexGame.savesData.stars = starsFromYandexCloud;
            YandexGame.SaveProgress();
        }
    }
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
