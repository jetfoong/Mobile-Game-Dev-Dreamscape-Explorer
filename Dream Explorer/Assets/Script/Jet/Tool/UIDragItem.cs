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
            rect.position = homeSlot.position;
            return;
        }

        var target = obj.GetComponent<DropTarget>();
        if (target == null || target.acceptToolID != toolID)
        {
            rect.position = homeSlot.position;
            return;
        }

        // 正确放置，播放动画
        target.PlayReplaceAnimation();

        rect.position = homeSlot.position;
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
