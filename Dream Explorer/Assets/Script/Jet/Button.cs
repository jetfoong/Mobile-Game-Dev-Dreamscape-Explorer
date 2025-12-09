using UnityEngine;
using UnityEngine.EventSystems;


public class Button : MonoBehaviour, IPointerClickHandler
{
    [Header("按下按钮触发对话（可选）")]
    public DialogueTrigger onPressTrigger;

    public GameScene gameScene;
    public string targetSceneName;

    public void OnPointerClick(PointerEventData eventData)
    {
        // ⭐ 新增
        if (onPressTrigger != null)
            onPressTrigger.Trigger();

        // 原本切换场景
        if (gameScene != null && !string.IsNullOrEmpty(targetSceneName))
            gameScene.SwitchTo(targetSceneName);
    }

}
