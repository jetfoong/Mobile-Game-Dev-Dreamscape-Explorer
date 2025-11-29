using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("拖拽使用的 Canvas")]
    public Canvas canvas;

    [Header("初始位置")]
    public Vector3 originalPosition;

    [Header("可吸附槽位")]
    public RectTransform slot; // 这个字段必须在 Inspector 指定

    [Header("最大吸附距离")]
    public float snapDistance = 50f;

    private RectTransform rect;
    private CanvasGroup group;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPosition = rect.localPosition;

        group = GetComponent<CanvasGroup>();
        if (group == null)
            group = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        transform.SetParent(canvas.transform); // 拖动到最上层
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = true;

        if (slot != null)
        {
            float distance = Vector2.Distance(rect.position, slot.position);
            if (distance <= snapDistance)
            {
                rect.position = slot.position; // 吸附
            }
            else
            {
                rect.localPosition = originalPosition; // 回到初始位置
            }
        }
        else
        {
            rect.localPosition = originalPosition;
        }
    }
}
