using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("主工具栏槽位")]
    public Transform[] mainSlots;

    [Header("副工具栏槽位")]
    public Transform[] subSlots;

    private void Awake()
    {
        Instance = this;
    }

    // ⭐ 新版本 AddTool：直接放已存在的 UI Image
    public void AddTool(GameObject toolUI, string toolID, ToolType toolType)
    {
        Transform[] targetSlots =
            toolType == ToolType.Main ? mainSlots : subSlots;

        Transform emptySlot = null;
        foreach (var slot in targetSlots)
        {
            if (slot.childCount == 0)
            {
                emptySlot = slot;
                break;
            }
        }

        if (emptySlot == null)
        {
            Debug.LogWarning("No empty slot in " + toolType + " toolbar");
            return;
        }

        // 将场景里已有的 UI Image 移动到槽位
        toolUI.transform.SetParent(emptySlot, false);
        toolUI.SetActive(true);

        // 设置 UIDragItem
        UIDragItem dragItem = toolUI.GetComponent<UIDragItem>();
        if (dragItem != null)
        {
            dragItem.toolID = toolID;
            dragItem.toolType = toolType;
            dragItem.homeSlot = emptySlot;
            dragItem.ReturnToSlot();
        }
    }
}
