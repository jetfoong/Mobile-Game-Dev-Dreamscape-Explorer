using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PuzzlePiece : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("所属 Puzzle Manager")]
    public PuzzleManager puzzleManager;

    [Header("正确槽位")]
    public RectTransform correctSlot;

    [Header("可吸附的所有槽位")]
    public RectTransform[] slots;

    [Header("拖拽使用的摄像机")]
    public Camera linkedCamera;

    public float snapDistance = 50f;

    private RectTransform rect;
    private Vector3 initialPosition;
    private Transform originalParent;
    private Canvas rootCanvas;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.position;
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsCameraActive()) return;

        if (puzzleManager != null)
            puzzleManager.isDragging = true;

        originalParent = transform.parent;

        // ⭐ 关键：只能移动到 Canvas 下
        transform.SetParent(rootCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsCameraActive()) return;

        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsCameraActive())
        {
            rect.position = initialPosition;
            return;
        }

        RectTransform nearestSlot = null;
        float minDist = float.MaxValue;

        foreach (var slot in slots)
        {
            float dist = Vector2.Distance(rect.position, slot.position);
            if (dist < minDist && dist <= snapDistance)
            {
                minDist = dist;
                nearestSlot = slot;
            }
        }

        if (nearestSlot != null)
            rect.position = nearestSlot.position;
        else
            rect.position = initialPosition;

        transform.SetParent(originalParent, true);

        if (puzzleManager != null)
        {
            puzzleManager.isDragging = false;
            puzzleManager.CheckPuzzleCompletion();
        }
    }

    private bool IsCameraActive()
    {
        return linkedCamera != null &&
               linkedCamera.gameObject.activeInHierarchy;
    }

    public bool IsCorrectlyPlaced()
    {
        return correctSlot != null &&
               Vector2.Distance(rect.position, correctSlot.position) <= snapDistance;
    }
}
