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
    [SerializeField] private List<TagPoints> pointsPerTagList = new List<TagPoints>(); // ������ ����� � ����� ��� ������� ����� ���������
    [SerializeField] private TextMeshProUGUI scoreText; // ���� ��� ������ ������
    [SerializeField] private GameObject defeatPanel; // ������ ��� ���� "Game Over"

    private Dictionary<string, int> pointsPerTag; // ������� ����� � �����
    private int totalScore = 0; // ����� ����

    private void Start()
    {
        // ������������� ������� �����
        pointsPerTag = new Dictionary<string, int>();
        foreach (var tagPoints in pointsPerTagList)
        {
            pointsPerTag[tagPoints.tag] = tagPoints.points;
        }

        // ���������, ��� ������ "Game Over" ���������� ������
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // ������� ������ ���������� �������� � �����
        int totalObjects = transform.childCount;

        // ������� ����� ������ ���������� �������� ��� ����������� ��������
        int remainingObjects = totalObjects;

        foreach (var tag in pointsPerTag.Keys)
        {
            // ��������� ���������� �������� � ������� ����� � Grid
            var objectsWithTag = transform.Cast<Transform>()
                .Where(child => child.CompareTag(tag)).ToList();

            // ���� ���������� �������� ������ ��� ����� 3, ���������� �� ��� ������� �� ���
            while (objectsWithTag.Count >= 3)
            {
                for (int i = 3; i > 0; i--)
                {
                    Destroy(objectsWithTag[i - 1].gameObject);
                }

                // ��������� ������ ����� ����������� ��������
                objectsWithTag = objectsWithTag.Skip(3).ToList();

                // ��������� ���� � ����������� �� ����
                totalScore += pointsPerTag[tag];

                // ��������� ���������� ���������� ��������
                remainingObjects -= 3;
            }
        }

        // ��������� ��������� ����
        scoreText.text = totalScore.ToString();

        // �������� �� ���������� ���������� �������� ����� �������� ����� �� ���
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