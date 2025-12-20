using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum ToolType
{
    Main,
    Sub
}

public class UIDragItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string toolID;
    public ToolType toolType;

    [HideInInspector] public Transform homeSlot;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool snapped = false;

        foreach (var hit in results)
        {
            // ① UISnapTarget
            var snapTarget = hit.gameObject.GetComponent<UISnapTarget>();
            if (snapTarget != null &&
                (string.IsNullOrEmpty(snapTarget.acceptToolID) ||
                 snapTarget.acceptToolID == toolID))
            {
                snapTarget.Snap(this);
                snapped = true;
                break;   // ⭐ 已吸附，直接结束检测
            }

            // ② DropTarget（保留你原本逻辑）
            var dropTarget = hit.gameObject.GetComponent<DropTarget>();
            if (dropTarget != null && dropTarget.acceptToolID == toolID)
            {
                dropTarget.PlayReplaceAnimation();
            }
        }

        // ⭐ 只有“没有吸附”才回工具栏
        if (!snapped)
            ReturnToSlot();
    }


    public void ReturnToSlot()
    {
        if (homeSlot != null)
            rect.position = homeSlot.position;
    }
}
