using UnityEngine;

// 触发类型枚举
public enum DialogueTriggerType
{
    OnSceneStart,       // 场景开始时触发
    OnDropTargetUsed,   // 使用某个物品触发
    OnButtonPress,      // 按钮按下触发
    OnPuzzleComplete    // 解谜完成触发
}

public class DialogueTrigger : MonoBehaviour
{
    [Header("触发类型")]
    public DialogueTriggerType triggerType;

    [Header("对话内容")]
    [TextArea(2, 4)]
    public string[] dialogueLines;

    [Header("是否只触发一次")]
    public bool triggerOnce = true;

    // 内部记录是否已经触发过
    private bool hasTriggered = false;

    /// <summary>
    /// 手动触发对话
    /// </summary>
    public void Trigger()
    {
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;

        // 调用 DialogueSystem 来显示对话
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.StartDialogue(dialogueLines);
        }
        else
        {
            Debug.LogWarning("DialogueSystem instance not found!");
        }
    }

    // 场景开始时自动触发（如果类型为 OnSceneStart）
    private void Start()
    {
        if (triggerType == DialogueTriggerType.OnSceneStart)
        {
            Trigger();
        }
    }
}
