using UnityEngine;

public class ItemActivator : MonoBehaviour
{
    public GameObject hiddenObject; // Ссылка на скрытый объект
    public KeyCode activationKey = KeyCode.F; // Клавиша для активации
    
    private SphereCollider sphereCollider;
    private MeshRenderer meshRenderer;
    private Color initialColor;
    private bool isActivated = false;   
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        // Получаем компонент MeshRenderer и начальный цвет объекта
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false);
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            initialColor = meshRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок вошел в триггер
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(true); // Активируем объект
            SetObjectTransparency(0.5f); // Устанавливаем прозрачность на 50%
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Проверяем, что это игрок вышел из триггера
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            if (isActivated)
            {
                hiddenObject.SetActive(true);                
            }
            else
            {
                hiddenObject.SetActive(false); // Деактивируем объект
                isActivated = false;
            }
        }
    }

    void Update()
    {
        if (!isActivated)
        {
            // Проверяем, что объект активен и нажата клавиша активации
            if (hiddenObject != null && hiddenObject.activeSelf && Input.GetKeyDown(activationKey))
            {
                SetObjectTransparency(1.0f); // Устанавливаем прозрачность на 100%
                isActivated = true;
                sphereCollider.enabled = false;
                Destroy(this.gameObject);
            }
        }
    }

    // Метод для установки прозрачности объекта
    private void SetObjectTransparency(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = alpha; // Устанавливаем прозрачность
            meshRenderer.material.color = color;

            // Убедитесь, что материал поддерживает прозрачность
            if (alpha < 1.0f)
            {
                meshRenderer.material.SetFloat("_Mode", 2); // Задаем режим прозрачности
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
                meshRenderer.material.SetFloat("_Mode", 0); // Возвращаем режим непрозрачности
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
    /*
    public GameObject hiddenObject; // Ссылка на скрытый объект
    public KeyCode activationKey = KeyCode.F; // Клавиша для активации

    private MeshRenderer meshRenderer;
    private Color initialColor;

    void Start()
    {
        // Получаем компонент MeshRenderer и начальный цвет объекта
        if (hiddenObject != null)
        {
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            initialColor = meshRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок вошел в триггер
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(true); // Активируем объект
            SetObjectTransparency(0.5f); // Устанавливаем прозрачность на 50%
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Проверяем, что это игрок вышел из триггера
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(false); // Деактивируем объект
        }
    }

    void Update()
    {
        // Проверяем, что объект активен и нажата клавиша активации
        if (hiddenObject != null && hiddenObject.activeSelf && Input.GetKeyDown(activationKey))
        {
            SetObjectTransparency(1.0f); // Устанавливаем прозрачность на 100%
        }
    }

    // Метод для установки прозрачности объекта
    private void SetObjectTransparency(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = initialColor;
            color.a = alpha; // Устанавливаем прозрачность
            meshRenderer.material.color = color;
        }
    }*/
}
