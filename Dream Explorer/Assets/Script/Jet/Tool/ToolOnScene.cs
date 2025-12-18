using UnityEngine;
using UnityEngine.EventSystems;

public class ToolOnScene : MonoBehaviour, IPointerClickHandler
{
    public string toolID;
    public ToolType toolType;

    [Header("对应的 UI Image（已在场景里）")]
    public GameObject uiImage;  // 直接拖入场景里的 Image

    [Header("是否可以被拾取")]
    public bool canPickUp = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canPickUp)
        {
            Debug.Log($"Tool {toolID} cannot be picked up yet.");
            return;
        }

        if (InventoryUI.Instance != null && uiImage != null)
        {
            InventoryUI.Instance.AddTool(uiImage, toolID, toolType);
            gameObject.SetActive(false); // 场景里的 Tool 消失
        }
    }
}
