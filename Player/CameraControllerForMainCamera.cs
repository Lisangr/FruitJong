using UnityEngine;

public class CameraControllerForMainCamera : MonoBehaviour
{
    public Transform player;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    
    // Джойстик для управления камерой
    public Joystick cameraJoystick;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        RotateWithJoystick();
    }

    void RotateWithJoystick()
    {
        if (player)
        {
            // Получаем ввод джойстика для камеры
            float joystickX = cameraJoystick.Horizontal;
            float joystickY = cameraJoystick.Vertical;

            if (Mathf.Abs(joystickX) > 0 || Mathf.Abs(joystickY) > 0)
            {
                x += joystickX * xSpeed * 0.02f;
                y -= joystickY * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            UpdateCameraPosition();
        }
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + player.position;

        transform.rotation = rotation;
        transform.position = position;

        player.rotation = Quaternion.Euler(0, x, 0);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
    
    public Vector3 GetCameraForward()
    {
        return transform.forward; 
    }

    public Vector3 GetCameraRight()
    {
        return transform.right; 
    }
}
