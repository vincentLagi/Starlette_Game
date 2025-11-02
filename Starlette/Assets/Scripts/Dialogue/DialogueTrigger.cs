using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private List<DialogueData> dialogueDatas;
    [SerializeField] private DialogueManager dialogManager;

    private void Update()
    {
        // Ini gue cmn ngetest jalan ato ngga, triggernya blom gue buat
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dialogManager.StartDialogue(dialogueDatas[1]);
        }
    }
}