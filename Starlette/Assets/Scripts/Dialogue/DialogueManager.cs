using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueUI.ShowBox(true);
        PlayCurrentLine();
    }

    private void PlayCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        var line = currentDialogue.lines[currentLineIndex];
        typingCoroutine = StartCoroutine(dialogueUI.TypeLine(line, OnTypingComplete));
    }

    private void OnTypingComplete()
    {
        dialogueUI.ShowContinuePrompt(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueUI.IsTyping)
            {
                dialogueUI.SkipTyping();
            }
            else
            {
                dialogueUI.ShowContinuePrompt(false);
                currentLineIndex++;

                if (currentLineIndex < currentDialogue.lines.Count)
                {
                    PlayCurrentLine();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private void EndDialogue()
    {
        dialogueUI.ClearText();
        dialogueUI.ShowBox(false);
    }
}