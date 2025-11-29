using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("正确槽位")]
    public RectTransform correctSlot;

    [Header("可吸附的所有槽位")]
    public RectTransform[] slots;

    [Header("拖拽使用的摄像机")]
    public Camera linkedCamera;

    [HideInInspector]
    public Vector3 initialPosition;

    private RectTransform rect;
    private Transform originalParent;
    public float snapDistance = 50f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.position;
    }

    private void Update()
    {
        // 根据linkedCamera控制显隐
        if (linkedCamera != null)
            gameObject.SetActive(linkedCamera.gameObject.activeInHierarchy);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (linkedCamera == null || !linkedCamera.gameObject.activeInHierarchy)
            return;

        originalParent = transform.parent;
        transform.SetParent(transform.root); // 拖到最上层
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (linkedCamera == null || !linkedCamera.gameObject.activeInHierarchy)
            return;

        rect.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (linkedCamera == null || !linkedCamera.gameObject.activeInHierarchy)
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
            rect.position = nearestSlot.position; // 吸附
        else
            rect.position = initialPosition; // 超出范围回初始位置

        transform.SetParent(originalParent);

        // 松手后才检查拼图完成
        if (PuzzleManager.Instance != null)
            PuzzleManager.Instance.CheckPuzzleCompletion();
    }

    public bool IsCorrectlyPlaced()
    {
        return correctSlot != null && Vector2.Distance(rect.position, correctSlot.position) <= snapDistance;
    }
}
