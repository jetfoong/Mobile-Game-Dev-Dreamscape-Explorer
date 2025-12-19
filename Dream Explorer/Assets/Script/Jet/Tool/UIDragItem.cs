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

        // ⭐ 核心：获取所有命中的对象（而不是 pointerEnter）
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool usedAny = false;

        foreach (var hit in results)
        {
            // ① 拼图吸附（如果你有）
            var snapTarget = hit.gameObject.GetComponent<UISnapTarget>();
            if (snapTarget != null &&
                (string.IsNullOrEmpty(snapTarget.acceptToolID) ||
                 snapTarget.acceptToolID == toolID))
            {
                snapTarget.Snap(this);
                ReturnToSlot();
                return;
            }

            // ② DropTarget（⭐ 可多个）
            var dropTarget = hit.gameObject.GetComponent<DropTarget>();
            if (dropTarget != null && dropTarget.acceptToolID == toolID)
            {
                dropTarget.PlayReplaceAnimation();
                usedAny = true;
            }
        }

        // 无论命中 0 个还是多个，最后都回槽
        ReturnToSlot();
    }

    public void ReturnToSlot()
    {
        if (homeSlot != null)
            rect.position = homeSlot.position;
    }
}
