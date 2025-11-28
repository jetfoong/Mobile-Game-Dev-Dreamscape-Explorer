using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class VirtualScene
{
    public string sceneName;
    public Camera camera;
}

public class GameScene : MonoBehaviour
{
    public List<VirtualScene> scenes = new List<VirtualScene>();
    private Camera currentCamera;

    public Canvas mainCanvas;   // ★ 你要拖你的 Canvas 进这里

    void Start()
    {
        if (scenes.Count > 0)
            SwitchTo(scenes[0].sceneName);
    }

    public void SwitchTo(string sceneName)
    {
        var targetScene = scenes.Find(s => s.sceneName == sceneName);
        if (targetScene == null) return;

        // 关闭旧相机
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
            currentCamera.tag = "Untagged";
        }

        // 启用新相机
        targetScene.camera.gameObject.SetActive(true);
        targetScene.camera.tag = "MainCamera";   // ★ 重点！
        currentCamera = targetScene.camera;

        // 更新 Canvas Camera (Screen Space - Camera 必须跟着换)
        if (mainCanvas != null)
            mainCanvas.worldCamera = currentCamera;

        Debug.Log("Switched to camera: " + targetScene.camera.name);
    }
}
