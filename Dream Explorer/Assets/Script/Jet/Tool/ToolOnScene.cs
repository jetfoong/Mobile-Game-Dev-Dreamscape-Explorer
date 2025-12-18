using UnityEngine;
using UnityEngine.EventSystems;

public class ToolOnScene : MonoBehaviour, IPointerClickHandler
{
    public Sprite toolSprite;
    public string toolID;
    public ToolType toolType;

    [Header("是否可以被拾取")]
    public bool canPickUp = true;   // ⭐ 新增

    public void OnPointerClick(PointerEventData eventData)
    {
        // ❌ 不可拾取 → 直接返回
        if (!canPickUp)
        {
            // 可选：提示、音效、抖动等
            Debug.Log($"Tool {toolID} cannot be picked up yet.");
            return;
        }

        // ✅ 可拾取 → 原逻辑
        if (InventoryUI.Instance != null && toolSprite != null)
        {
            InventoryUI.Instance.AddTool(toolSprite, toolID, toolType);
            gameObject.SetActive(false);
        }
    }
}
