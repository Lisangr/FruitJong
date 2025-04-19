using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public int price;
    public GameObject decline;
    public KeyCode activationKey = KeyCode.F;
    public Text textMeshProUGUI;
    
    // UI элементы для тапа по экрану
    public Button buyButton;
    public GameObject buyButtonContainer;

    public static bool canBuy = false; // переменная флаг для подтверждения покупки
    private bool isInTrigger = false;
    private Wallet wallet;
    
    void OnEnable()
    {
        textMeshProUGUI.text = price.ToString();

        wallet = FindObjectOfType<Wallet>();
        decline.gameObject.SetActive(false);
        
        // Настраиваем кнопку покупки
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(TryBuyItem);
            SetBuyButtonVisible(false);
        }
        
        Debug.Log("Загружаем количество звезд: " + PlayerPrefs.GetInt("Stars"));
    }
    
    void OnDisable()
    {
        // Отписываемся от события клика по кнопке
        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(TryBuyItem);
        }
    }

    void Update()
    {
        if (isInTrigger)
        {
            // Поддерживаем старый способ через клавишу
            if (Input.GetKeyDown(activationKey))
            {
                TryBuyItem();
            }
        }
    }
    
    // Метод для попытки покупки
    public void TryBuyItem()
    {
        int currentStars = PlayerPrefs.GetInt("Stars"); // получаем актуальное количество собранных звезд
        Debug.Log("Текущее количество звезд перед покупкой: " + currentStars);

        if (currentStars >= price)
        {
            currentStars -= price;
            PlayerPrefs.SetInt("Stars", currentStars);
            PlayerPrefs.Save();
            Debug.Log("Количество звезд после покупки: " + currentStars);
            wallet.GetStars();
            decline.gameObject.SetActive(false);
            canBuy = true; // Разрешаем покупку, устанавливаем флаг
            
            // Скрываем кнопку покупки
            SetBuyButtonVisible(false);
        }
        else
        {
            Debug.LogWarning("Недостаточно звезд для покупки");
            decline.gameObject.SetActive(true); // Показываем decline, если недостаточно звезд
            canBuy = false; // Блокируем флаг, если покупка невозможна
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Вошли в зону покупки. Цена: " + price);
            
            // Показываем кнопку покупки
            SetBuyButtonVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            decline.gameObject.SetActive(false);
            canBuy = false; // Сбрасываем флаг, если игрок вышел
            Debug.Log("Игрок покинул зону покупки.");
            
            // Скрываем кнопку покупки
            SetBuyButtonVisible(false);
        }
    }
    
    // Метод для отображения/скрытия кнопки покупки
    private void SetBuyButtonVisible(bool isVisible)
    {
        if (buyButtonContainer != null)
        {
            buyButtonContainer.SetActive(isVisible);
        }
    }
}
