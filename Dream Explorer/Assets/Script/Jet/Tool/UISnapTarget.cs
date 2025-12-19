using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UISnapTarget : MonoBehaviour
{
    [Header("绑定的摄像机")]
    public Camera linkedCamera;

    [Header("允许吸附的工具 ID（留空 = 都可以）")]
    public string acceptToolID;

    [Header("吸附后的本地位置偏移")]
    public Vector2 snapOffset;

    [HideInInspector]
    public UIDragItem currentItem;   // ⭐ 当前吸附的工具

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (linkedCamera == null) return;

        bool active = linkedCamera.gameObject.activeInHierarchy;

        canvasGroup.alpha = active ? 1f : 0f;
        canvasGroup.blocksRaycasts = active;
        canvasGroup.interactable = active;
    }

    public bool CanAccept(string toolID)
    {
        return string.IsNullOrEmpty(acceptToolID) || acceptToolID == toolID;
    }

    public bool IsFilled()
    {
        return currentItem != null;
    }

    public void Snap(UIDragItem item)
    {
        // 已经被占用就不再吸附
        if (currentItem != null) return;

        RectTransform rt = item.GetComponent<RectTransform>();
        rt.SetParent(transform, false);
        rt.anchoredPosition = snapOffset;

        currentItem = item;

        // ⭐ 通知所属 Group 检查完成情况
        var group = GetComponentInParent<UISnapGroup>();
        if (group != null)
            group.CheckCompletion();
    }
}
