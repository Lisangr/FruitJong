using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public Image timerBar; // ������ �� Image ���������
    public float initialTime = 30f; // ��������� �����
    public GameObject defeatPanel;
    public TextMeshProUGUI timeText;
    public Image star1;
    public Image star2;
    public Image star3;
    private float currentTime; // ������� �����

    void Start()
    {
        currentTime = initialTime;
        defeatPanel.SetActive(false);        
    }
    void Update()
    {
        // ���������� ������� �� ������ �����
        currentTime -= Time.deltaTime;

        // ���������� �������� ����������
        float fillAmount = currentTime / initialTime;

        // ��������� ���������� �����������
        timerBar.fillAmount = fillAmount;

        // ���������� �������� ������� �� ����� ������
        int roundedTime = Mathf.CeilToInt(currentTime);

        // �������������� ������� � ������ � �������
        string timeFormatted = string.Format("{0:00}:{1:00}", roundedTime / 60, roundedTime % 60);
        timeText.text = timeFormatted;
    
        if(timerBar.fillAmount < 0.62 && timerBar.fillAmount >= 0.41)
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
        // ��������, �������� �� �� ����
        if (currentTime <= 0)
        {
            Time.timeScale = 0;
            currentTime = 0;
            defeatPanel.SetActive(true);
        }
    }    
}
