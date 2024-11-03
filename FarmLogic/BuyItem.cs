using TMPro;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public int price;
    public GameObject decline;
    public KeyCode activationKey = KeyCode.F;
    public TextMeshProUGUI textMeshProUGUI;

    public static bool canBuy = false; // Статический флаг для синхронизации покупок
    private bool isInTrigger = false;
    private Wallet wallet;
    void OnEnable()
    {
        textMeshProUGUI.text = price.ToString();

        wallet = FindObjectOfType<Wallet>();
        decline.gameObject.SetActive(false);
        Debug.Log("Начальное количество звёзд: " + PlayerPrefs.GetInt("Stars"));
    }

    void Update()
    {
        if (isInTrigger)
        {
            if (Input.GetKeyDown(activationKey))
            {
                int currentStars = PlayerPrefs.GetInt("Stars"); // Всегда считываем актуальное количество звёзд
                Debug.Log("Текущее количество звёзд перед покупкой: " + currentStars);

                if (currentStars >= price)
                {
                    currentStars -= price;
                    PlayerPrefs.SetInt("Stars", currentStars);
                    PlayerPrefs.Save();
                    Debug.Log("Оставшиеся звёзды после покупки: " + currentStars);
                    wallet.GetStars();
                    decline.gameObject.SetActive(false);
                    canBuy = true; // Разрешаем покупку, устанавливаем флаг
                }
                else
                {
                    Debug.LogWarning("Недостаточно звёзд для покупки");
                    decline.gameObject.SetActive(true); // Показываем decline, если недостаточно звёзд
                    canBuy = false; // Отключаем флаг, если покупка невозможна
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Игрок в зоне покупки. Цена: " + price);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            decline.gameObject.SetActive(false);
            canBuy = false; // Сбрасываем флаг, если игрок уходит
            Debug.Log("Игрок покинул зону покупки.");
        }
    }
}
