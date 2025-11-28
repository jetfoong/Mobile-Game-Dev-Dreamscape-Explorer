using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class SpriteButton : MonoBehaviour, IPointerClickHandler
{
    public GameScene gameScene;
    public string targetSceneName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameScene != null && !string.IsNullOrEmpty(targetSceneName))
            gameScene.SwitchTo(targetSceneName);
    }
}
