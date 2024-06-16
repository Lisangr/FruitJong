using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomSpeed = 10f;
    public float minFOV = 15f;
    public float maxFOV = 90f;

    private void LateUpdate()
    {
        MapForComputers();
    }

    private void MapForComputers()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            ZoomCamera(scrollInput);
        }
    }

    private void ZoomCamera(float increment)
    {
        float currentFOV = virtualCamera.m_Lens.FieldOfView;
        float newFOV = currentFOV - increment * zoomSpeed;
        virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
    }

}
