using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Image timerBar; // Ссылка на Image компонент
    public float initialTime = 30f; // Начальное время
    public GameObject defeatPanel;
    public Text timeText;
    public Image star1;
    public Image star2;
    public Image star3;
    private float currentTime; // Текущее время

    void Start()
    {
        currentTime = initialTime;
        Time.timeScale = 1f;
        defeatPanel.SetActive(false);        
    }
    void Update()
    {
        // Уменьшение времени на каждом кадре
        currentTime -= Time.deltaTime;

        // Вычисление процента заполнения
        float fillAmount = currentTime / initialTime;

        // Установка заполнения изображения
        timerBar.fillAmount = fillAmount;

        // Округление текущего времени до целых секунд
        int roundedTime = Mathf.CeilToInt(currentTime);

        // Форматирование времени в минуты и секунды
        string timeFormatted = string.Format("{0:00}:{1:00}", roundedTime / 60, roundedTime % 60);
        timeText.text = timeFormatted;
    
        if(timerBar.fillAmount < 0.62 && timerBar.fillAmount >= 0.41)
        {
            star3.color = Color.gray;
        }
        else if (timerBar.fillAmount < 0.41 && timerBar.fillAmount >= 0.11)
        {          
            star2.color = Color.gray;
        }
        else if (timerBar.fillAmount < 0.11)
        {
            star1.color = Color.gray;
        }
        // Проверка, достигли ли мы нуля
        if (currentTime <= 0)
        {
            //Time.timeScale = 0;
            currentTime = 0;
            defeatPanel.SetActive(true);
        }
    }    
}
