using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public GameScene gameScene;
    public string targetSceneName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameScene != null && !string.IsNullOrEmpty(targetSceneName))
        {
            gameScene.SwitchTo(targetSceneName);
        }
    }
}
