using UnityEngine;

public enum DialogueTriggerType
{
    OnSceneStart,
    OnDropTargetUsed,
    OnButtonPress,
    OnPuzzleComplete
}

public class DialogueTrigger : MonoBehaviour
{
    [Header("????")]
    public DialogueTriggerType triggerType;

    [Header("?????????")]
    [TextArea(2, 4)]
    public string[] dialogueLines;

    [Header("?????")]
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    // ???????
    public void Trigger()
    {
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;

        // ?????? DialogueSystem
        DialogueSystem.Instance.StartDialogue(dialogueLines);
    }

    // Scene ??????
    private void Start()
    {
        if (triggerType == DialogueTriggerType.OnSceneStart)
            Trigger();
    }
}
