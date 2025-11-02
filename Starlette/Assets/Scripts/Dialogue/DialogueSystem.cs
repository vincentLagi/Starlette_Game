using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI continuePrompt;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float continuePromptDelay = 0.2f;

    [Header("Dialogue Setup")]
    [SerializeField] private DialogTextDB dialogDB;

    [SerializeField] private PlayerMovement playerMovement;
    private List<string> dialogueLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine typingCoroutine;
    private bool dialogueFinished = false;

    private void Awake()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueCanvas.gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
                skipTyping = true;
            else
            {
                if (continuePrompt != null)
                    continuePrompt.gameObject.SetActive(false);

                ShowNextLine();
            }
        }
    }

    public void StartDialogue(RoomID room, DialogueID dialogue)
    {
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            GameObject gameInterface = GameObject.Find("Toolbar");
            gameInterface.SetActive(false);
        }

        dialogueLines = dialogDB.GetDialogueLines(room, dialogue);

        if (dialogueLines == null || dialogueLines.Count == 0)
        {
            Debug.LogWarning($"Dialogue empty for Room: {room} and Dialogue: {dialogue}");
            return;
        }

        if (dialogueCanvas != null)
            dialogueCanvas.gameObject.SetActive(true);

        dialogueFinished = false;
        currentLineIndex = 0;
        typingCoroutine = StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            if (skipTyping)
            {
                dialogueText.text = line;
                skipTyping = false;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (continuePrompt != null)
        {
            yield return new WaitForSeconds(continuePromptDelay);
            continuePrompt.gameObject.SetActive(true);
        }
    }

    private void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Count)
        {
            typingCoroutine = StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
        }
        else
        {
            dialogueFinished = true;
            dialogueText.text = "";
            if (continuePrompt != null)
                continuePrompt.gameObject.SetActive(false);

            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueCanvas.gameObject.SetActive(false);
        if (playerMovement != null)
        {
            playerMovement.canMove = true;
            GameObject gameInterface = GameObject.Find("Inventory").transform.GetChild(1).gameObject;
            gameInterface.SetActive(true);
        }
    }
}
