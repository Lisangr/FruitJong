using UnityEngine;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour
{
    public GameObject hiddenObject; // Ссылка на скрытый объект или родительский объект группы
    public KeyCode activationKey = KeyCode.F; // Клавиша для активации
 

    private SphereCollider sphereCollider;
    private MeshRenderer meshRenderer;
    private Color initialColor;
    private bool isActivated = false;

    private const string ActivatedObjectsKey = "ActivatedObjects";

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();

        if (hiddenObject != null)
        {
            SetActiveRecursive(hiddenObject, false); // Деактивируем все дочерние объекты
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                initialColor = meshRenderer.material.color;
        }

        // Проверка состояния объектов в памяти
        if (hiddenObject != null && IsObjectActivated(hiddenObject.name))
        {
            ActivateHiddenObject(); // Активируем группу объектов
            isActivated = true;
        }

        if (IsObjectActivated(gameObject.name))
        {
            Destroy(gameObject); // Удаляем табличку, если она уже активирована
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            SetActiveRecursive(hiddenObject, true);
            SetObjectTransparency(0.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            SetActiveRecursive(hiddenObject, isActivated);
        }
    }

    void Update()
    {
        if (!isActivated && BuyItem.canBuy) // Проверяем флаг canBuy
        {
            if (hiddenObject != null && hiddenObject.activeSelf && Input.GetKeyDown(activationKey))
            {
                SetObjectTransparency(1.0f);
                isActivated = true;
                sphereCollider.enabled = false;

                SaveObjectActivation(hiddenObject.name);
                SaveObjectActivation(gameObject.name);

                Destroy(gameObject);
            }
        }
    }

    private void SetObjectTransparency(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = alpha;
            meshRenderer.material.color = color;

            if (alpha < 1.0f)
            {
                meshRenderer.material.SetFloat("_Mode", 2);
                meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                meshRenderer.material.SetInt("_ZWrite", 0);
                meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
                meshRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                meshRenderer.material.renderQueue = 3000;
            }
            else
            {
                meshRenderer.material.SetFloat("_Mode", 0);
                meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                meshRenderer.material.SetInt("_ZWrite", 1);
                meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
                meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
                meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                meshRenderer.material.renderQueue = -1;
            }
        }
    }

    private void SaveObjectActivation(string objectName)
    {
        string savedData = PlayerPrefs.GetString(ActivatedObjectsKey, "");
        List<string> activatedObjects = new List<string>(savedData.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

        if (!activatedObjects.Contains(objectName))
        {
            activatedObjects.Add(objectName);
        }

        PlayerPrefs.SetString(ActivatedObjectsKey, string.Join(",", activatedObjects));
        PlayerPrefs.Save();

        // Отладка
        Debug.Log($"Сохранено состояние объекта {objectName}. Текущий список: {string.Join(",", activatedObjects)}");
    }

    private bool IsObjectActivated(string objectName)
    {
        string savedData = PlayerPrefs.GetString(ActivatedObjectsKey, "");
        List<string> activatedObjects = new List<string>(savedData.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

        bool isActivated = activatedObjects.Contains(objectName);
        Debug.Log($"Проверка состояния объекта {objectName}: {isActivated}");
        return isActivated;
    }

    private void ActivateHiddenObject()
    {
        SetActiveRecursive(hiddenObject, true); // Активируем все дочерние объекты
        SetObjectTransparency(1.0f);
        sphereCollider.enabled = false;
    }

    // Вспомогательный метод для активации/деактивации всех дочерних объектов
    private void SetActiveRecursive(GameObject obj, bool state)
    {
        obj.SetActive(state);
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}