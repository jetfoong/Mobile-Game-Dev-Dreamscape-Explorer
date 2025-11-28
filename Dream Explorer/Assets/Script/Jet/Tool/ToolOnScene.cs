using UnityEngine;
using UnityEngine.EventSystems;

public class ToolOnScene : MonoBehaviour, IPointerClickHandler
{
    public Sprite toolSprite; // 点击后生成工具栏的 Sprite
    public string toolID;     // 工具唯一 ID

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryUI.Instance != null && toolSprite != null)
        {
            InventoryUI.Instance.AddTool(toolSprite, toolID);
            // 可选：点击后隐藏或销毁场景工具
            gameObject.SetActive(false);
        }
    }
}
