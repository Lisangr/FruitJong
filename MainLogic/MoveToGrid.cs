using System.Collections;
using UnityEngine;

public class MoveToGrid : MonoBehaviour
{
    private Transform gridTransform;
    [SerializeField] private float moveDuration = 1f; // ������������ �����������
    private bool isActive = true; // ���� ��� �������� ��������� ����������
    [SerializeField] private int newLayer; // ����� ����, �� ������� ����� ��������� ������
    private LevelsLayers levelsLayers;
    private void Start()
    {
        gridTransform = FindObjectOfType<Grid>().GetComponent<Transform>();
        levelsLayers = FindObjectOfType<LevelsLayers>().GetComponent<LevelsLayers>();   
    }

    public void OnPointerClick()
    {
        if (isActive)
        {
            StartCoroutine(MoveToGridRoutine());            
        }
    }

    private IEnumerator MoveToGridRoutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = gridTransform.position;
        RectTransform rectTransform = transform as RectTransform;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        transform.SetParent(gridTransform);
        gameObject.layer = newLayer; // ������ ���� ������� ����� �����������

        CheckAndDeactivateLayers();
        levelsLayers.CheckAndActivateNextLevel(); // ��������� � ���������� ��������� �������, ���� ������� ����
    }

    public void SetActiveState(bool state)
    {
        isActive = state;
    }

    private void CheckAndDeactivateLayers()
    {
        int currentLayer = gameObject.layer;
        bool tilesOnCurrentLayer = false;

        // Check if there are any tiles on the current layer
        GameObject[] allTiles = FindObjectsOfType<GameObject>();
        foreach (GameObject tile in allTiles)
        {
            if (tile.layer == currentLayer && tile != this.gameObject)
            {
                tilesOnCurrentLayer = true;
                break;
            }
        }

        if (!tilesOnCurrentLayer)
        {
            // Deactivate all tiles on other layers
            foreach (GameObject tile in allTiles)
            {
                if (tile.layer != currentLayer && tile.tag == "Tile")
                {
                    MoveToGrid tileScript = tile.GetComponent<MoveToGrid>();
                    if (tileScript != null)
                    {
                        tileScript.SetActiveState(false);
                    }
                }
            }
        }
    }   
}
