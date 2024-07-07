using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorTriangle : MonoBehaviour
{
    
    public int rows = 8;
    public int columns = 10;
    public float tileSpacing = 100f;
    public GameObject[] tilePrefabs;
    public LevelsLayers tileManager;

    public string deacLevel0Name;
    public string deacLevel1Name;
    public string deacLevel2Name;
    public string deacLevel3Name;
    public string defaultLayerName = "ClickLevel0";

    private List<GameObject> tiles = new List<GameObject>();
    private int totalTileCount;
    private Vector3 vertex1 = new Vector3(350, 550, 0);
    private Vector3 vertex2 = new Vector3(-400, -450, 0);
    private Vector3 vertex3 = new Vector3(500, -450, 0);

    void Start()
    {/*
        CreateTiles();
        LayoutTiles();     
        ShuffleTiles();
        AssignLayers();
       */
        CreateTiles();
        ShuffleTiles();
        
        // Размещаем плитки
        //LayoutTiles();
        LayoutTilesInTriangularPattern();
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

        LayoutTilesInTriangularPattern();
    }
    private void LayoutTilesInTriangularPattern()
    {
        int tileIndex = 0;

        float minX = Mathf.Min(vertex1.x, vertex2.x, vertex3.x);
        float maxX = Mathf.Max(vertex1.x, vertex2.x, vertex3.x);
        float minY = Mathf.Min(vertex1.y, vertex2.y, vertex3.y);
        float maxY = Mathf.Max(vertex1.y, vertex2.y, vertex3.y);

        float innerOffsetX = tileSpacing * 0.5f;
        float innerOffsetY = tileSpacing * 0.5f;

        for (float y = maxY; y >= minY && tileIndex < tiles.Count; y -= innerOffsetY)
        {
            for (float x = minX; x <= maxX && tileIndex < tiles.Count; x += innerOffsetX)
            {
                Vector3 point = new Vector3(x, y, 0);
                if (IsPointInTriangle(point, vertex1, vertex2, vertex3))
                {
                    tiles[tileIndex].transform.localPosition = point;
                    tileIndex++;
                }
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

    private bool IsPointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float dX = p.x - p2.x;
        float dY = p.y - p2.y;
        float dX21 = p2.x - p1.x;
        float dY12 = p1.y - p2.y;
        float D = dY12 * (p0.x - p2.x) + dX21 * (p0.y - p2.y);
        float s = dY12 * dX + dX21 * dY;
        float t = (p2.y - p0.y) * dX + (p0.x - p2.x) * dY;
        if (D < 0) return s <= 0 && t <= 0 && s + t >= D;
        return s >= 0 && t >= 0 && s + t <= D;
    }
    
}
