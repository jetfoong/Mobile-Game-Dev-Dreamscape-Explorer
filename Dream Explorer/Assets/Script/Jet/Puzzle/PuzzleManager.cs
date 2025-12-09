using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("拼图块")]
    public PuzzlePiece[] pieces;

    [Header("槽位")]
    public RectTransform[] slots;

    [Header("完成后要激活的UI")]
    public GameObject[] uiToActivate;

    [Header("完成后也要激活的 Sprite/GameObject（可选）")]
    public GameObject[] extraObjectsToActivate;

    [Header("控制显隐的摄像机")]
    public Camera linkedCamera;

    [Header("拼图完成时触发的对话（可选）")]
    public DialogueTrigger puzzleCompleteTrigger;   // ✅ 新增但不影响旧逻辑

    private bool hasCompleted = false;   // 防止重复触发

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (linkedCamera == null) return;

        bool camActive = linkedCamera.gameObject.activeInHierarchy;

        foreach (var piece in pieces)
            if (piece != null)
                piece.gameObject.SetActive(camActive);

        foreach (var slot in slots)
            if (slot != null)
                slot.gameObject.SetActive(camActive);

        // 摄像机关闭时隐藏UI
        if (!camActive)
        {
            foreach (var ui in uiToActivate)
                if (ui != null) ui.SetActive(false);

            foreach (var obj in extraObjectsToActivate)
                if (obj != null) obj.SetActive(false);
        }
    }

    public void CheckPuzzleCompletion()
    {
        bool allCorrect = true;

        foreach (var piece in pieces)
        {
            if (piece != null && !piece.IsCorrectlyPlaced())
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect && !hasCompleted)
        {
            hasCompleted = true;

            // ✅ 原本功能保留
            foreach (var ui in uiToActivate)
                if (ui != null) ui.SetActive(true);

            foreach (var obj in extraObjectsToActivate)
                if (obj != null) obj.SetActive(true);

            // ✅ 新增：对话触发（可选）
            if (puzzleCompleteTrigger != null)
                puzzleCompleteTrigger.Trigger();
        }
    }
}
