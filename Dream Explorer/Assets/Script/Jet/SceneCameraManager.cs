using UnityEngine;

public class SceneCameraManager : MonoBehaviour
{
    public static Camera currentCamera;
    public Canvas mainCanvas;

    public void SwitchToCamera(Camera cam)
    {
        // 禁用旧相机
        if (currentCamera != null)
        {
            currentCamera.tag = "Untagged";
            currentCamera.enabled = false;
        }

        // 启用新相机
        cam.enabled = true;
        cam.tag = "MainCamera";
        currentCamera = cam;

        // 更新 Canvas
        mainCanvas.worldCamera = cam;

        Debug.Log("Switched to camera: " + cam.name);
    }
}
