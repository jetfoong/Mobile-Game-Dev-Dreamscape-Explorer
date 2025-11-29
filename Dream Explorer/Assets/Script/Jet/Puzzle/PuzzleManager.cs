using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("所有 Puzzle Pieces")]
    public List<PuzzlePiece> puzzlePieces;

    [Header("对应槽位（和 PuzzlePiece 一一对应）")]
    public List<RectTransform> slots;

    [Header("当全部到位时激活的 UI")]
    public List<GameObject> uiToActivate;

    [Header("激活显示的摄像机")]
    public Camera linkedCamera;

    private void Update()
    {
        if (linkedCamera == null) return;

        bool camActive = linkedCamera.gameObject.activeInHierarchy;

        // 控制 PuzzlePiece 显示/隐藏
        foreach (var piece in puzzlePieces)
        {
            if (piece != null)
                piece.gameObject.SetActive(camActive);
        }

        // 控制槽位显示/隐藏
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.gameObject.SetActive(camActive);
        }

        // 默认隐藏 uiToActivate
        foreach (var ui in uiToActivate)
        {
            if (ui != null)
                ui.SetActive(false);
        }

        // 检查是否全部到位
        if (AllPiecesSnapped())
        {
            foreach (var ui in uiToActivate)
            {
                if (ui != null)
                    ui.SetActive(camActive); // 激活 UI 并随摄像机显示
            }
        }
    }

    private bool AllPiecesSnapped()
    {
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            var piece = puzzlePieces[i];
            if (piece == null || i >= slots.Count || slots[i] == null) return false;

            float distance = Vector2.Distance(piece.GetComponent<RectTransform>().position, slots[i].position);
            if (distance > piece.snapDistance)
                return false;
        }
        return true;
    }
}
