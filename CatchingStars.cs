using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatchingStars : MonoBehaviour
{
    public Image timerBar;
    public Image star1;
    public Image star2;
    public Image star3;
    public string sceneName;
    private int currentStarsFromPlayerPrefs;
    private void Update()
    {
        StarsColoring();
    }

    private void StarsColoring()
    {
        if (PlayerPrefs.HasKey("Stars"))
        {
            currentStarsFromPlayerPrefs = PlayerPrefs.GetInt("Stars");
        }
        else
        {
            currentStarsFromPlayerPrefs = 0;
            PlayerPrefs.SetInt("Stars", currentStarsFromPlayerPrefs);
            PlayerPrefs.Save();
        }

        if (timerBar.fillAmount < 0.62 && timerBar.fillAmount >= 0.41)
        {
            star3.color = Color.gray;
        }
        else if (timerBar.fillAmount < 0.41 && timerBar.fillAmount >= 0.21)
        {
            star2.color = Color.gray;
        }
        else if (timerBar.fillAmount < 0.21)
        {
            star1.color = Color.gray;
        }
    }

    public void OnClick()
    {
        if (timerBar.fillAmount >= 0.62)
        {
            PlayerPrefs.SetInt("Stars", currentStarsFromPlayerPrefs + 3);
            PlayerPrefs.Save();
        }
        else if (timerBar.fillAmount < 0.62 && timerBar.fillAmount >= 0.41)
        {
            star3.color = Color.gray;
            PlayerPrefs.SetInt("Stars", currentStarsFromPlayerPrefs + 2);            
            PlayerPrefs.Save();            
        }
        else if (timerBar.fillAmount < 0.41 && timerBar.fillAmount >= 0.11)
        {
            star3.color = Color.gray;
            star2.color = Color.gray;
            PlayerPrefs.SetInt("Stars", currentStarsFromPlayerPrefs + +1);
            PlayerPrefs.Save();
        }
        else if (timerBar.fillAmount < 0.11)
        {
            star3.color = Color.gray;
            star2.color = Color.gray;
            star1.color = Color.gray;
            PlayerPrefs.SetInt("Stars", currentStarsFromPlayerPrefs);
            PlayerPrefs.Save();            
        }

        SceneManager.LoadScene(sceneName);
    }
}
