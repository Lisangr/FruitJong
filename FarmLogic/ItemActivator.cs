using UnityEngine;

public class ItemActivator : MonoBehaviour
{
    public GameObject hiddenObject; // ������ �� ������� ������
    public KeyCode activationKey = KeyCode.F; // ������� ��� ���������
    
    private SphereCollider sphereCollider;
    private MeshRenderer meshRenderer;
    private Color initialColor;
    private bool isActivated = false;   
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        // �������� ��������� MeshRenderer � ��������� ���� �������
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false);
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            initialColor = meshRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ��� ����� ����� � �������
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(true); // ���������� ������
            SetObjectTransparency(0.5f); // ������������� ������������ �� 50%
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ���������, ��� ��� ����� ����� �� ��������
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            if (isActivated)
            {
                hiddenObject.SetActive(true);                
            }
            else
            {
                hiddenObject.SetActive(false); // ������������ ������
                isActivated = false;
            }
        }
    }

    void Update()
    {
        if (!isActivated)
        {
            // ���������, ��� ������ ������� � ������ ������� ���������
            if (hiddenObject != null && hiddenObject.activeSelf && Input.GetKeyDown(activationKey))
            {
                SetObjectTransparency(1.0f); // ������������� ������������ �� 100%
                isActivated = true;
                sphereCollider.enabled = false;
                Destroy(this.gameObject);
            }
        }
    }

    // ����� ��� ��������� ������������ �������
    private void SetObjectTransparency(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = alpha; // ������������� ������������
            meshRenderer.material.color = color;

            // ���������, ��� �������� ������������ ������������
            if (alpha < 1.0f)
            {
                meshRenderer.material.SetFloat("_Mode", 2); // ������ ����� ������������
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
                meshRenderer.material.SetFloat("_Mode", 0); // ���������� ����� ��������������
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
    public GameObject hiddenObject; // ������ �� ������� ������
    public KeyCode activationKey = KeyCode.F; // ������� ��� ���������

    private MeshRenderer meshRenderer;
    private Color initialColor;

    void Start()
    {
        // �������� ��������� MeshRenderer � ��������� ���� �������
        if (hiddenObject != null)
        {
            meshRenderer = hiddenObject.GetComponent<MeshRenderer>();
            initialColor = meshRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ��� ����� ����� � �������
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(true); // ���������� ������
            SetObjectTransparency(0.5f); // ������������� ������������ �� 50%
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ���������, ��� ��� ����� ����� �� ��������
        if (other.CompareTag("Player") && hiddenObject != null)
        {
            hiddenObject.SetActive(false); // ������������ ������
        }
    }

    void Update()
    {
        // ���������, ��� ������ ������� � ������ ������� ���������
        if (hiddenObject != null && hiddenObject.activeSelf && Input.GetKeyDown(activationKey))
        {
            SetObjectTransparency(1.0f); // ������������� ������������ �� 100%
        }
    }

    // ����� ��� ��������� ������������ �������
    private void SetObjectTransparency(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = initialColor;
            color.a = alpha; // ������������� ������������
            meshRenderer.material.color = color;
        }
    }*/
}
