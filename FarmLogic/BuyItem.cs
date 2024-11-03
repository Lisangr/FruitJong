using TMPro;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public int price;
    public GameObject decline;
    public KeyCode activationKey = KeyCode.F;
    public TextMeshProUGUI textMeshProUGUI;

    public static bool canBuy = false; // ����������� ���� ��� ������������� �������
    private bool isInTrigger = false;
    private Wallet wallet;
    void OnEnable()
    {
        textMeshProUGUI.text = price.ToString();

        wallet = FindObjectOfType<Wallet>();
        decline.gameObject.SetActive(false);
        Debug.Log("��������� ���������� ����: " + PlayerPrefs.GetInt("Stars"));
    }

    void Update()
    {
        if (isInTrigger)
        {
            if (Input.GetKeyDown(activationKey))
            {
                int currentStars = PlayerPrefs.GetInt("Stars"); // ������ ��������� ���������� ���������� ����
                Debug.Log("������� ���������� ���� ����� ��������: " + currentStars);

                if (currentStars >= price)
                {
                    currentStars -= price;
                    PlayerPrefs.SetInt("Stars", currentStars);
                    PlayerPrefs.Save();
                    Debug.Log("���������� ����� ����� �������: " + currentStars);
                    wallet.GetStars();
                    decline.gameObject.SetActive(false);
                    canBuy = true; // ��������� �������, ������������� ����
                }
                else
                {
                    Debug.LogWarning("������������ ���� ��� �������");
                    decline.gameObject.SetActive(true); // ���������� decline, ���� ������������ ����
                    canBuy = false; // ��������� ����, ���� ������� ����������
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("����� � ���� �������. ����: " + price);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            decline.gameObject.SetActive(false);
            canBuy = false; // ���������� ����, ���� ����� ������
            Debug.Log("����� ������� ���� �������.");
        }
    }
}
