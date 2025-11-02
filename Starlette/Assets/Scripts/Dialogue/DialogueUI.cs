using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI continuePrompt;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float continuePromptDelay = 0.2f;

    private Coroutine typingCoroutine;
    private bool skip = false;
    public bool IsTyping { get; private set; }

    public IEnumerator TypeLine(string line, System.Action onComplete)
    {
        IsTyping = true;
        dialogueText.text = "";
        continuePrompt.gameObject.SetActive(false);

        foreach (char c in line)
        {
            if (skip)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
        skip = false;

        yield return new WaitForSeconds(continuePromptDelay);
        onComplete?.Invoke();
    }

    public void SkipTyping()
    {
        skip = true;
    }

    public void ShowContinuePrompt(bool show)
    {
        continuePrompt.gameObject.SetActive(show);
    }

    public void ShowBox(bool show)
    {
        dialogueBox.SetActive(show);
    }

    public void ClearText()
    {
        dialogueText.text = "";
        ShowContinuePrompt(false);
    }
}