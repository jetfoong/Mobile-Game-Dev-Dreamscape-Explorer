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
    public GameObject[] extraObjectsToActivate;   // ← 新增

    [Header("控制显隐的摄像机")]
    public Camera linkedCamera;

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

            foreach (var obj in extraObjectsToActivate)  // ← 新增
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

        if (allCorrect)
        {
            // 激活 UI
            foreach (var ui in uiToActivate)
                if (ui != null) ui.SetActive(true);

            // 激活额外指定的 Sprite/GameObject
            foreach (var obj in extraObjectsToActivate)   // ← 新增
                if (obj != null) obj.SetActive(true);
        }
    }
}
