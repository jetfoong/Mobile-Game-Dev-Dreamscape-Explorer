using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("UI Canvas")]
    public Canvas canvas;

    [Header("主工具栏槽位")]
    public Transform[] mainSlots;

    [Header("副工具栏槽位")]
    public Transform[] subSlots;

    [Header("工具UI预制体")]
    public GameObject toolPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateCanvasCamera(Camera cam)
    {
        if (canvas != null)
            canvas.worldCamera = cam;
    }

    // ⭐ 新版本 AddTool
    public void AddTool(Sprite toolSprite, string toolID, ToolType toolType)
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

        GameObject uiTool = Instantiate(toolPrefab, emptySlot);

        Image img = uiTool.GetComponent<Image>();
        UIDragItem dragItem = uiTool.GetComponent<UIDragItem>();
        RectTransform rt = uiTool.GetComponent<RectTransform>();

        if (img != null)
            img.sprite = toolSprite;

        if (dragItem != null)
        {
            dragItem.toolID = toolID;
            dragItem.toolType = toolType;
            dragItem.homeSlot = emptySlot;
            dragItem.ReturnToSlot();
        }
    }
}
