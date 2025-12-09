using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public GameScene gameScene;
    public string targetSceneName;

    [Header("对话触发器（可选）")]
    public DialogueTrigger onClickTrigger;

    private bool triggeredOnce = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameScene != null && !string.IsNullOrEmpty(targetSceneName))
        {
            gameScene.SwitchTo(targetSceneName);
        }

        // 只触发一次对话（如果有设置）
        if (onClickTrigger != null && !triggeredOnce)
        {
            triggeredOnce = true;
            onClickTrigger.Trigger();
        }
    }
}
