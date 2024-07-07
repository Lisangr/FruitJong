using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public int price;
    public GameObject decline;
    public KeyCode activationKey = KeyCode.F;

    private int currentStars;
    private bool canBuy = false;

    void Start()
    {
        decline.gameObject.SetActive(false);
        currentStars = PlayerPrefs.GetInt("Stars");
    }

    void Update()
    {
        if (canBuy)
        {
            if (Input.GetKeyDown(activationKey))
            {
                if (currentStars >= price)
                {
                    currentStars -= price;
                    PlayerPrefs.SetInt("Stars", currentStars);
                    PlayerPrefs.Save();
                    decline.gameObject.SetActive(false); // �������� decline ��� �������� �������
                }
                else
                {
                    decline.gameObject.SetActive(true); // ���������� decline, ���� ������������ �����
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBuy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBuy = false;
            decline.gameObject.SetActive(false); // �������� decline, ����� ����� ������� �� ��������
        }
    }
}
