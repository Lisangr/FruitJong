using UnityEngine;
/// <summary>
/// nor used because generation don't working
/// </summary>
public class GameZoneSwitcher : MonoBehaviour
{
    public GameObject[] zones;

    private void Start()
    {
        int t = Random.Range(0, zones.Length);
        zones[t].gameObject.SetActive(true);
    }
}
