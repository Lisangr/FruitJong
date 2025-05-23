using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CameraControllerForMainCamera cameraController;
    
    // Ссылка на визуальный джойстик
    public Joystick moveJoystick;

    private Rigidbody rb;
    private Transform player;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();

        if (cameraController == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraController = mainCamera.GetComponent<CameraControllerForMainCamera>();
            }
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Получаем ввод от джойстика или используем нулевое значение, если джойстик не назначен
        float vInput = 0f;
        float hInput = 0f;
        
        if (moveJoystick != null)
        {
            vInput = moveJoystick.Vertical * moveSpeed;
            hInput = moveJoystick.Horizontal * moveSpeed;
        }

        Vector3 moveDirection = cameraController.GetCameraForward() * vInput 
            + cameraController.GetCameraRight() * hInput;
        moveDirection *= Time.fixedDeltaTime;

        rb.MovePosition(this.transform.position + moveDirection);
    }

    private void SavePlayerPosition()
    {
        Vector3 position = player.transform.position;
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetFloat("PlayerZ", position.z);
        PlayerPrefs.Save();
    }

    private IEnumerator LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");
        player.transform.position = new Vector3(x, y, z);

        yield return null;
    }
}