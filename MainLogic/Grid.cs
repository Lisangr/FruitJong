using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[System.Serializable]
    public class TagPoints
    {
        public string tag;
        public int points;
    }

public class Grid : MonoBehaviour
{
    [SerializeField] private List<TagPoints> pointsPerTagList = new List<TagPoints>(); // Список тегов и очков для задания через инспектор
    [SerializeField] private TextMeshProUGUI scoreText; // Поле для вывода текста
    [SerializeField] private GameObject defeatPanel; // Панель для окна "Game Over"

    private Dictionary<string, int> pointsPerTag; // Словарь тегов и очков
    private int totalScore = 0; // Общий счет

    private void Start()
    {
        // Инициализация словаря очков
        pointsPerTag = new Dictionary<string, int>();
        foreach (var tagPoints in pointsPerTagList)
        {
            pointsPerTag[tagPoints.tag] = tagPoints.points;
        }

        // Убедитесь, что панель "Game Over" изначально скрыта
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Подсчет общего количества объектов в сетке
        int totalObjects = transform.childCount;

        // Создаем копию общего количества объектов для дальнейшего подсчета
        int remainingObjects = totalObjects;

        foreach (var tag in pointsPerTag.Keys)
        {
            // Проверяем количество объектов с текущим тегом в Grid
            var objectsWithTag = transform.Cast<Transform>()
                .Where(child => child.CompareTag(tag)).ToList();

            // Если количество объектов больше или равно 3, уничтожаем по три объекта за раз
            while (objectsWithTag.Count >= 3)
            {
                for (int i = 3; i > 0; i--)
                {
                    Destroy(objectsWithTag[i - 1].gameObject);
                }

                // Обновляем список после уничтожения объектов
                objectsWithTag = objectsWithTag.Skip(3).ToList();

                // Начисляем очки в зависимости от тега
                totalScore += pointsPerTag[tag];

                // Обновляем количество оставшихся объектов
                remainingObjects -= 3;
            }
        }

        // Обновляем текстовое поле
        scoreText.text = totalScore.ToString();

        // Проверка на количество оставшихся объектов после удаления групп по три
        if (remainingObjects > 7)
        {
            if (defeatPanel != null)
            {
                defeatPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}