using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("UI Canvas")]
    public Canvas canvas;

    [Header("工具栏槽位")]
    public Transform[] slots;

    [Header("工具UI预制体")]
    public GameObject toolPrefab;

    private List<GameObject> toolsInBar = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // 切换摄像机时调用
    public void UpdateCanvasCamera(Camera cam)
    {
        if (canvas != null)
            canvas.worldCamera = cam;
    }

    // 添加工具到工具栏
    public void AddTool(Sprite toolSprite, string toolID)
    {
        if (toolPrefab == null)
        {
            Debug.LogError("Tool prefab is not assigned in InventoryUI!");
            return;
        }

        Transform emptySlot = null;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount == 0)
            {
                emptySlot = slots[i];
                break;
            }
        }

        if (emptySlot == null)
        {
            Debug.LogWarning("No empty slot available in the tool bar!");
            return;
        }

        GameObject uiTool = Instantiate(toolPrefab, emptySlot);

        Image img = uiTool.GetComponent<Image>();
        UIDragItem dragItem = uiTool.GetComponent<UIDragItem>();
        RectTransform rt = uiTool.GetComponent<RectTransform>();

        if (img != null) img.sprite = toolSprite;
        else Debug.LogError("Tool prefab missing Image component!");

        if (dragItem != null)
        {
            dragItem.homeSlot = emptySlot;
            dragItem.toolID = toolID;

            // 立即对齐槽位
            if (rt != null)
            {
                dragItem.ReturnToSlot();
            }
        }
        else
        {
            Debug.LogError("Tool prefab missing UIDragItem component!");
        }

        toolsInBar.Add(uiTool);
    }
}
