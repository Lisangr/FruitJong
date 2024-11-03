using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public string scene = "";
    public string sceneBuild ="";
    public GameObject menuPanel;
    public GameObject winPanel;
    public GameObject settingPanel;
    private void OnEnable()
    {
        LevelsLayers.OnLevelEnd += LevelsLayers_OnLevelEnd;
    }

    private void LevelsLayers_OnLevelEnd()
    {
        winPanel.SetActive(true);
    }
    private void OnDisable()
    {
        LevelsLayers.OnLevelEnd -= LevelsLayers_OnLevelEnd;
    }
    private void Start()
    {
        if (menuPanel != null)
        menuPanel.SetActive(false);

        if (winPanel != null)
        winPanel.SetActive(false);

        if (settingPanel != null)
            settingPanel.SetActive(false);
    }
    public void OnSettingsButtonClick()
    {
        settingPanel.SetActive(true);
    }
    public void onClickStart()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    public void onClickBuildStart()
    {
        SceneManager.LoadScene(sceneBuild, LoadSceneMode.Single);
    }
    public void RestartCurrentScene()
    {
        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Перезагружаем сцену по её индексу
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void OnClickMenuButton()
    {
        menuPanel.SetActive(true);
    }
    public void OnCloseMenueButton()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (settingPanel != null)
            settingPanel.SetActive(false);
    }
}
