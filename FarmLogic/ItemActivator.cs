using UnityEngine;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour
{
    public GameObject hiddenObject; // Ссылка на внешний объект для активирования после покупки
    public KeyCode activationKey = KeyCode.F; // Клавиша для активации
 
    private SphereCollider sphereCollider;
    private MeshRenderer meshRenderer;
    private Color initialColor;
    private bool isActivated = false;
    private bool isPlayerInRange = false;

    private const string ActivatedObjectsKey = "ActivatedObjects";

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();

        if (hiddenObject != null)
        {
            SetActiveRecursive(hiddenObject, false); // Деактивируем при загрузке объекта
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                initialColor = meshRenderer.material.color;
        }

        // Проверяем состояние объектов в памяти
        if (hiddenObject != null && IsObjectActivated(hiddenObject.name))
        {
            ActivateHiddenObject(); // Активируем нужные элементы
            isActivated = true;
        }

        if (IsObjectActivated(gameObject.name))
        {
            Destroy(gameObject); // Удаляем активатор, если он уже активирован
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            isPlayerInRange = true;
            SetActiveRecursive(hiddenObject, true);
            SetObjectTransparency(0.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            isPlayerInRange = false;
            SetActiveRecursive(hiddenObject, isActivated);
        }
    }

    void Update()
    {
        // Проверяем флаг покупки и нажатие кнопки активации (клавиатура)
        if (!isActivated && isPlayerInRange)
        {
            if (BuyItem.canBuy) // Проверяем флаг canBuy
            {
                if (hiddenObject != null && hiddenObject.activeSelf)
                {
                    // Активируем объект автоматически после покупки
                    ActivateObjectPermanently();
                }
            }
            else if (Input.GetKeyDown(activationKey)) // Поддерживаем старый метод активации через клавишу
            {
                // Здесь мы просто показываем, что нужно приобрести объект
                Debug.Log("Объект нужно приобрести!");
            }
        }
    }

    // Публичный метод для активации объекта из UI
    public void ActivateObjectPermanently()
    {
        if (!isActivated && BuyItem.canBuy)
        {
            SetObjectTransparency(1.0f);
            isActivated = true;
            sphereCollider.enabled = false;

            SaveObjectActivation(hiddenObject.name);
            SaveObjectActivation(gameObject.name);

            // Сбрасываем флаг покупки
            BuyItem.canBuy = false;

            Destroy(gameObject);
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
        Debug.Log($"Сохранили активацию объекта {objectName}. Текущий список: {string.Join(",", activatedObjects)}");
    }

    private bool IsObjectActivated(string objectName)
    {
        string savedData = PlayerPrefs.GetString(ActivatedObjectsKey, "");
        List<string> activatedObjects = new List<string>(savedData.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

        bool isActivated = activatedObjects.Contains(objectName);
        Debug.Log($"Проверка активации объекта {objectName}: {isActivated}");
        return isActivated;
    }

    private void ActivateHiddenObject()
    {
        SetActiveRecursive(hiddenObject, true); // Активируем все дочерние объекты
        SetObjectTransparency(1.0f);
        sphereCollider.enabled = false;
    }

    // Вспомогательный метод для включения/выключения всех дочерних объектов
    private void SetActiveRecursive(GameObject obj, bool state)
    {
        obj.SetActive(state);
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}