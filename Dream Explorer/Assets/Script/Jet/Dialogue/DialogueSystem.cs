using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("对话面板")]
    public GameObject dialoguePanel;

    [Header("对话文字")]
    public TextMeshProUGUI dialogueText;

    [Header("打字速度")]
    public float typingSpeed = 0.05f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        StopAllCoroutines();
        StartCoroutine(PlayDialogue(lines));
    }

    private IEnumerator PlayDialogue(string[] lines)
    {
        dialoguePanel.SetActive(true);

        foreach (string line in lines)
        {
            dialogueText.text = "";

            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        dialoguePanel.SetActive(false);
    }
}
