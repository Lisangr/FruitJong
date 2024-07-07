using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGeneratorHexagon : MonoBehaviour
{
    public float tileSpacing = 100f;
    public GameObject[] tilePrefabs;
    public LevelsLayers tileManager;
    public int rows;
    public int columns;
    public string[] levelNames = new string[] { "ClickLevel1", "ClickLevel2", "ClickLevel3", 
        "ClickLevel4", "ClickLevel5", "ClickLevel6" };
    public string defaultLayerName = "ClickLevel0";

    private List<GameObject> tiles = new List<GameObject>();
    private int totalTileCount;
    private float xOffset = 300f;

    void Start()
    {
        // Создаем плитки
        CreateTiles();
        ShuffleTiles();
        // Размещаем плитки
        LayoutTiles();
        AssignLayers();
    }

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
        string[] layerNames = { "ClickLevel1", "ClickLevel2", "ClickLevel3",
        "ClickLevel4", "ClickLevel5", "ClickLevel6" };
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

        LayoutTilesInHexPattern();
    }

    private void LayoutTilesInHexPattern()
    {
        int tileIndex = 0;
        float hexWidth = tileSpacing * Mathf.Sqrt(3) / 2; // horizontal distance between centers of two hexes
        float hexHeight = tileSpacing * 3 / 4; // vertical distance between centers of two hexes

        // Estimate the rows and columns needed for the hexagon
        int radius = Mathf.CeilToInt(Mathf.Sqrt(totalTileCount / 2f)); // rough estimate for hexagonal grid radius

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                if (tileIndex >= tiles.Count) return;
                float x = hexWidth * (3f / 2f * q) + xOffset;
                float y = hexHeight * (Mathf.Sqrt(3f) * (r + q / 2f));
                tiles[tileIndex++].transform.localPosition = new Vector3(x, y, 0);
            }
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
        string[] layerNames = { "ClickLevel1", "ClickLevel2", "ClickLevel3",
        "ClickLevel4", "ClickLevel5", "ClickLevel6" };
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
    /*
    public float tileSpacing = 100f;
    public GameObject[] tilePrefabs;
    public LevelsLayers tileManager;    
    public int rows;
    public int columns;
    public string[] levelNames = new string[] { "ClickLevel1", "ClickLevel2", "ClickLevel3", "ClickLevel4" };
    public string defaultLayerName = "ClickLevel0";

    private List<GameObject> tiles = new List<GameObject>();
    private int totalTileCount;
    void Start()
    {/*
        //CreateTiles();
        //LayoutTiles();
        //ShuffleTiles();
        //AssignLayers();
        //AssignRandomLevels();
        // Создаем плитки
        CreateTiles();
        ShuffleTiles();
        // Размещаем плитки
        LayoutTiles();
        AssignLayers();
    }
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
        string[] layerNames = { "ClickLevel1", "ClickLevel2", "ClickLevel3", "ClickLevel4" };
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

        LayoutTilesInHexPattern();
    }
    private void LayoutTilesInHexPattern()
    {
        int tileIndex = 0;
        float hexWidth = tileSpacing * Mathf.Sqrt(3) / 2; // horizontal distance between centers of two hexes
        float hexHeight = tileSpacing * 3 / 4; // vertical distance between centers of two hexes

        // Estimate the rows and columns needed for the hexagon
        int radius = Mathf.CeilToInt(Mathf.Sqrt(totalTileCount / 2f)); // rough estimate for hexagonal grid radius

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                if (tileIndex >= tiles.Count) return;
                float x = hexWidth * (3f / 2f * q);
                float y = hexHeight * (Mathf.Sqrt(3f) * (r + q / 2f));
                tiles[tileIndex++].transform.localPosition = new Vector3(x, y, 0);
            }
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
        string[] layerNames = { "ClickLevel1", "ClickLevel2", "ClickLevel3", "ClickLevel4" };
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
    }*/
}
