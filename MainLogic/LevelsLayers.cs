using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsLayers : MonoBehaviour
{
    public string deacLevel0Name;
    public string deacLevel1Name;
    public string deacLevel2Name;
    public string deacLevel3Name;
    public string deacLevel4Name;
    public string deacLevel5Name;

    public string defaultLayerName = "CliclLevel0";  // Слой, который активен изначально
    public GameObject winPanel;

    private List<string> layerNames;
    private int currentLevelIndex = -1;

    public static List<GameObject> FindGameObjectsWithLayerMasks(List<int> layers)
    {
        List<GameObject> objectsInLayers = new List<GameObject>();

        foreach (int layer in layers)
        {
            int layerMask = 1 << layer;
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if ((layerMask & (1 << obj.layer)) != 0)
                {
                    objectsInLayers.Add(obj);
                }
            }
        }

        return objectsInLayers;
    }

    // Метод для деактивации и отключения интерактивности объектов
    public static void DeactivateAndDisableInteractivity(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            // Отключение интерактивности для компонентов Image и Button
            Image imageComponent = obj.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.raycastTarget = false;
            }

            Button buttonComponent = obj.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = false;
            }
        }
    }

    void Start()
    {
        winPanel.SetActive(false);
        layerNames = new List<string> { deacLevel0Name, deacLevel1Name, deacLevel2Name, deacLevel3Name,
        deacLevel4Name, deacLevel5Name};

        foreach (string layerName in layerNames)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                Debug.LogError("Layer not found: " + layerName);
            }
            else
            {
                List<GameObject> objectsOnLayer = FindGameObjectsWithLayerMasks(new List<int> { layer });
                DeactivateAndDisableInteractivity(objectsOnLayer);
            }
        }

        // Не вызываем ActivateNextLevel() напрямую
        CheckAndActivateNextLevel();
        //ActivateNextLevel();
    }

    void Update()
    {
        CheckAndActivateNextLevel();
    }
    
    private void ActivateNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= layerNames.Count)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
            return;
        }

        string nextLevelName = layerNames[currentLevelIndex];
        int nextLevel = LayerMask.NameToLayer(nextLevelName);

        if (nextLevel == -1)
        {
            Debug.LogError("Layer not found: " + nextLevelName);
            return;
        }

        List<GameObject> nextLevelObjects = FindGameObjectsWithLayerMasks(new List<int> { nextLevel });
        foreach (GameObject obj in nextLevelObjects)
        {
            obj.SetActive(true);
            // Восстанавливаем интерактивность
            Image imageComponent = obj.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.raycastTarget = true;
            }

            Button buttonComponent = obj.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = true;
            }
        }

        Debug.Log("Activated level: " + nextLevelName);
    }
    
    public void CheckAndActivateNextLevel()
    {
        if (currentLevelIndex == -1 || currentLevelIndex >= layerNames.Count)
        {
            ActivateNextLevel();
            return;
        }

        string currentLevelName = layerNames[currentLevelIndex];
        int currentLevel = LayerMask.NameToLayer(currentLevelName);

        if (currentLevel == -1)
        {
            Debug.LogError("Layer not found: " + currentLevelName);
            return;
        }

        List<GameObject> currentLevelObjects = FindGameObjectsWithLayerMasks(new List<int> { currentLevel });

        Debug.Log("Current Level: " + currentLevelName + " has " + currentLevelObjects.Count + " objects.");

        if (currentLevelObjects.Count == 0)
        {
            ActivateNextLevel();
        }
    }    
}
