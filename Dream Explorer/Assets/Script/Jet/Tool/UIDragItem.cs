using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public enum ToolType
{
    Main,   
    Sub     
}

public class UIDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string toolID;
    public ToolType toolType;
    [HideInInspector] public Transform homeSlot;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    void Awake()
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

        var obj = eventData.pointerEnter;
        if (obj == null)
        {
            ReturnToSlot();
            return;
        }

        // ⭐ 优先检测吸附目标
        var snapTarget = obj.GetComponent<UISnapTarget>();
        if (snapTarget != null)
        {
            if (string.IsNullOrEmpty(snapTarget.acceptToolID) ||
                snapTarget.acceptToolID == toolID)
            {
                snapTarget.Snap(this);
                return;
            }
        }

        // 原有 DropTarget 逻辑
        var target = obj.GetComponent<DropTarget>();
        if (target == null || target.acceptToolID != toolID)
        {
            ReturnToSlot();
            return;
        }

        target.PlayReplaceAnimation();
        ReturnToSlot();
    }


    public void ReturnToSlot()
    {
        if (homeSlot != null)
        {
            var rect = GetComponent<RectTransform>();
            rect.position = homeSlot.position;
        }
    }
}
