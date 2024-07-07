using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Настройка параметров размещения плиток
    public int rows = 8; // Количество рядов
    public int columns = 10; // Количество столбцов
    public float tileSpacing = 100f; // Расстояние между плитками
    public GameObject[] tilePrefabs; // Массив префабов плиток
    public LevelsLayers tileManager; // Ссылка на TileManager

    // Имена слоев
    public string deacLevel0Name;
    public string deacLevel1Name;
    public string deacLevel2Name;
    public string deacLevel3Name;
    public string deacLevel4Name;
    public string deacLevel5Name;

    //public string defaultLayerName = "ClickLevel0";

    // Список плиток
    private List<GameObject> tiles = new List<GameObject>();
    private int totalTileCount;
    void Start()
    {
        // Создаем плитки
        CreateTiles();        
        ShuffleTiles();        
        //LayoutTilesInCircularPattern();
        // Размещаем плитки
        LayoutTiles();
        AssignLayers();
    }

    // Создает плитки и добавляет их в список
    private void CreateTiles()
    {
        // Определяем общее количество плиток
        int tileCount = rows * columns;
        totalTileCount = tileCount;
        // Убедимся, что общее количество плиток кратно 3
        if (tileCount % 3 != 0)
        {
            tileCount += 3 - (tileCount % 3);
        }

        // Создаем плитки по префабам, чтобы для каждого префаба было кратно 3
        int prefabIndex = 0;
        while (tiles.Count < tileCount)
        {
            for (int i = 0; i < 3; i++) // Добавляем по 3 плитки каждого типа
            {
                GameObject tilePrefab = tilePrefabs[prefabIndex];
                GameObject tile = Instantiate(tilePrefab, transform);
                tiles.Add(tile);
            }

            prefabIndex = (prefabIndex + 1) % tilePrefabs.Length;
        }
    }

    // Размещает плитки по уровням
    private void LayoutTiles()
    {
        string[] layerNames = { deacLevel0Name, deacLevel1Name, deacLevel2Name, deacLevel3Name,
        deacLevel4Name, deacLevel5Name};
        int tileIndex = 0;

        for (int layerIndex = layerNames.Length - 1; layerIndex >= 0; layerIndex--)
        {
            if (tileIndex >= tiles.Count) break;

            string layerName = layerNames[layerIndex];
            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                Debug.LogError("Layer not found: " + layerName);
                continue;
            }

            int layerTileCount = (rows * columns) / layerNames.Length;
            layerTileCount = (layerTileCount % 3 == 0) ? layerTileCount : layerTileCount + 3 - (layerTileCount % 3);

            for (int i = 0; i < layerTileCount && tileIndex < tiles.Count; i++, tileIndex++)
            {
                tiles[tileIndex].layer = layer;
            }
        }

        LayoutTilesInCircularPattern();
    }

    // Размещает плитки в соответствии с маджонгом по круговому шаблону
    private void LayoutTilesInCircularPattern()
    {
        int tileIndex = 0;
        int layer = 0;

        while (tileIndex < tiles.Count)
        {
            // Генерация плиток по круговому шаблону
            for (int i = 0; i <= layer; i++)
            {
                float offset = layer * 0.5f * tileSpacing;
                // Верхняя сторона
                if (tileIndex < tiles.Count)
                {
                    tiles[tileIndex].transform.localPosition = new Vector3(i * tileSpacing - offset, offset, -layer * tileSpacing);
                    tileIndex++;
                }
                // Правая сторона
                if (tileIndex < tiles.Count)
                {
                    tiles[tileIndex].transform.localPosition = new Vector3(offset, i * tileSpacing - offset, -layer * tileSpacing);
                    tileIndex++;
                }
                // Нижняя сторона
                if (tileIndex < tiles.Count)
                {
                    tiles[tileIndex].transform.localPosition = new Vector3(i * tileSpacing - offset, -offset, -layer * tileSpacing);
                    tileIndex++;
                }
                // Левая сторона
                if (tileIndex < tiles.Count)
                {
                    tiles[tileIndex].transform.localPosition = new Vector3(-offset, i * tileSpacing - offset, -layer * tileSpacing);
                    tileIndex++;
                }
            }

            layer++;
        }
    }
    private void ShuffleTiles()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject temp = tiles[i];
            int randomIndex = Random.Range(i, tiles.Count);
            tiles[i] = tiles[randomIndex];
            tiles[randomIndex] = temp;
        }
    }

    private void AssignLayers()
    {
        string[] layerNames = { deacLevel0Name, deacLevel1Name, deacLevel2Name, deacLevel3Name,
        deacLevel4Name, deacLevel5Name};
        int totalLayers = layerNames.Length;
        int tilesPerLayer = totalTileCount / totalLayers;

        for (int i = 0; i < tiles.Count; i++)
        {
            int layerIndex = (i / tilesPerLayer) % totalLayers;
            string layerName = layerNames[layerIndex];
            int layer = LayerMask.NameToLayer(layerName);
            if (layer != -1)
            {
                tiles[i].layer = layer;
            }
            else
            {
                Debug.LogError("Layer not found: " + layerName);
            }
        }
    }
}
