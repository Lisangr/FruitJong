using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneratorX : MonoBehaviour
{
    public int rows = 8; // Количество рядов
    public int columns = 10; // Количество столбцов
    public float tileSpacing = 100f; // Расстояние между плитками
    public GameObject[] tilePrefabs; // Массив префабов плиток
    public LevelsLayers tileManager; // Ссылка на TileManager  
    private int totalTileCount;
    public string deacLevel0Name;
    public string deacLevel1Name;
    public string deacLevel2Name;
    public string deacLevel3Name;
    public string defaultLayerName = "ClickLevel0";

    private List<GameObject> tiles = new List<GameObject>();
    void OnEnable()
    {// Создаем плитки
        CreateTiles();
        ShuffleTiles();
        //LayoutTilesInCircularPattern();
        // Размещаем плитки
        LayoutTiles();
        AssignLayers();
        /*
        CreateTiles();
        LayoutTiles();
        ShuffleTiles();
        AssignLayers();*/
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
        string[] layerNames = { deacLevel0Name, deacLevel1Name, deacLevel2Name, deacLevel3Name };
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

        LayoutTilesInCrossPattern();
    }

    private void LayoutTilesInCrossPattern()
    {
        int tileIndex = 0;
        float halfTileSpacing = tileSpacing * 0.5f;
        float startX = -(columns / 2) * tileSpacing;
        float startY = -(rows / 2) * tileSpacing;

        // Размещаем основной крест
        for (int i = 0; i < rows; i++)
        {
            if (tileIndex >= tiles.Count) break;

            float y = startY + i * tileSpacing;

            // Центральная вертикальная линия
            tiles[tileIndex].transform.localPosition = new Vector3(0, y, 0);
            tileIndex++;
        }

        for (int j = 0; j < columns; j++)
        {
            if (tileIndex >= tiles.Count) break;

            float x = startX + j * tileSpacing;

            // Центральная горизонтальная линия
            tiles[tileIndex].transform.localPosition = new Vector3(x, 0, 0);
            tileIndex++;
        }

        // Размещаем внутренний крест со смещением 0,5 tileSpacing
        for (int i = 0; i < rows; i++)
        {
            if (tileIndex >= tiles.Count) break;

            float y = startY + i * tileSpacing;

            // Внутренняя вертикальная линия со смещением
            tiles[tileIndex].transform.localPosition = new Vector3(halfTileSpacing, y, 0);
            tileIndex++;

            if (tileIndex >= tiles.Count) break;

            tiles[tileIndex].transform.localPosition = new Vector3(-halfTileSpacing, y, 0);
            tileIndex++;
        }

        for (int j = 0; j < columns; j++)
        {
            if (tileIndex >= tiles.Count) break;

            float x = startX + j * tileSpacing;

            // Внутренняя горизонтальная линия со смещением
            tiles[tileIndex].transform.localPosition = new Vector3(x, halfTileSpacing, 0);
            tileIndex++;

            if (tileIndex >= tiles.Count) break;

            tiles[tileIndex].transform.localPosition = new Vector3(x, -halfTileSpacing, 0);
            tileIndex++;
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
        string[] layerNames = { deacLevel0Name, deacLevel1Name, deacLevel2Name, deacLevel3Name };
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
