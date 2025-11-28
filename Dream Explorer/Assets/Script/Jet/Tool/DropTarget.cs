using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropTarget : MonoBehaviour
{
    [Header("能被哪一个工具触发？(toolID 对应 UIDragItem)")]
    public string acceptToolID;

    [Header("目标 SpriteRenderer（被替换的图片）")]
    public SpriteRenderer targetRenderer;

    [Header("动画帧（按顺序播放，每帧 2 秒）")]
    public List<Sprite> frames = new List<Sprite>();

    [Header("动画结束后激活的物体（可选）")]
    public GameObject activateObject;

    // --- 播放动画 ---
    public void PlayReplaceAnimation()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            if (targetRenderer != null)
                targetRenderer.sprite = frames[i];

            if (i == frames.Count - 1 && activateObject != null)
                activateObject.SetActive(true);

            yield return new WaitForSeconds(2f);
        }
    }
}
