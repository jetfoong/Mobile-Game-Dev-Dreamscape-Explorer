using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
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
    public DialogueTrigger puzzleCompleteTrigger;

    [HideInInspector]
    public bool isDragging = false;

    private bool hasCompleted = false;

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (isDragging) return;
        if (linkedCamera == null) return;

        bool camActive = linkedCamera.gameObject.activeInHierarchy;

        foreach (var piece in pieces)
            if (piece != null)
                piece.gameObject.SetActive(camActive);

        foreach (var slot in slots)
            if (slot != null)
                slot.gameObject.SetActive(camActive);

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
        if (hasCompleted) return;

        foreach (var piece in pieces)
        {
            if (piece != null && !piece.IsCorrectlyPlaced())
                return;
        }

        hasCompleted = true;

        foreach (var ui in uiToActivate)
            if (ui != null) ui.SetActive(true);

        foreach (var obj in extraObjectsToActivate)
            if (obj != null) obj.SetActive(true);

        if (puzzleCompleteTrigger != null)
            puzzleCompleteTrigger.Trigger();
    }
}
