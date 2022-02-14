using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static Camera _currentCamera;
    private static Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        _currentCamera = _mainCamera;
    }

    private void Update()
    {
        if (_currentCamera == null)
        {
            ResetCamera();
        }
        
    }

    public static void SetCamera(Camera camera)
    {
        if (_currentCamera == null)
        {
            _currentCamera = _mainCamera;
            _currentCamera.enabled = true;
        }

        if (camera == _currentCamera)
        {
            return;
        }

        _currentCamera.enabled = false;
        _currentCamera = camera;
        _currentCamera.enabled = true;
    }

    public static void ResetCamera()
    {
        SetCamera(_mainCamera);
    }

    public static bool IsMainCamera()
    {
        return _currentCamera == _mainCamera;
    }
    
}
