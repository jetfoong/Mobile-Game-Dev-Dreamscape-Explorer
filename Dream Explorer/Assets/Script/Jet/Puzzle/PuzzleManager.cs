using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; // 单例

    [Header("拼图块")]
    public PuzzlePiece[] pieces;

    [Header("槽位")]
    public RectTransform[] slots;

    [Header("完成后要激活的UI")]
    public GameObject[] uiToActivate;

    [Header("控制显隐的摄像机")]
    public Camera linkedCamera;

    private void Awake()
    {
        // 初始化单例
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

        // 控制PuzzlePiece显隐
        foreach (var piece in pieces)
        {
            if (piece != null)
                piece.gameObject.SetActive(camActive);
        }

        // 控制Slot显隐
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.gameObject.SetActive(camActive);
        }

        // 如果摄像机关闭，激活UI也要隐藏
        if (!camActive)
        {
            foreach (var ui in uiToActivate)
            {
                if (ui != null)
                    ui.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 检查所有拼图块是否在正确位置
    /// </summary>
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

        // 全部正确才激活UI
        if (allCorrect)
        {
            foreach (var ui in uiToActivate)
            {
                if (ui != null)
                    ui.SetActive(true);
            }
        }
    }
}
