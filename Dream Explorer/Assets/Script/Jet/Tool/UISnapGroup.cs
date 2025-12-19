using UnityEngine;

public class UISnapGroup : MonoBehaviour
{
    [Header("这一组的 Snap Targets")]
    public UISnapTarget[] snapTargets;

    [Header("全部填满后激活的 UI")]
    public GameObject[] uiToActivate;

    [Header("全部填满后激活的 Sprite / GameObject")]
    public GameObject[] extraObjectsToActivate;

    private bool hasCompleted = false;

    public void CheckCompletion()
    {
        if (hasCompleted) return;

        foreach (var target in snapTargets)
        {
            if (target == null || !target.IsFilled())
                return;
        }

        // ✅ 全部填满
        hasCompleted = true;

        // 1️⃣ 激活 UI
        foreach (var ui in uiToActivate)
        {
            if (ui != null)
                ui.SetActive(true);
        }

        // 2️⃣ 激活 GameObject + 解除摄像机控制
        foreach (var obj in extraObjectsToActivate)
        {
            if (obj == null) continue;

            obj.SetActive(true);

            // ⭐ 关键：禁用“摄像机显隐控制脚本”
            DisableCameraVisibility(obj);
        }
    }

    /// <summary>
    /// 禁用物体（及其子物体）上的摄像机显隐逻辑
    /// </summary>
    void DisableCameraVisibility(GameObject obj)
    {
        // 情况 A：你用的是 linkedCamera + Update SetActive 的脚本
        var pieces = obj.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var mb in pieces)
        {
            // ⭐ 只要脚本里有 linkedCamera 字段，就禁用
            var field = mb.GetType().GetField("linkedCamera");
            if (field != null)
            {
                mb.enabled = false;
            }
        }
    }
}
