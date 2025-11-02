using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogTextDB", menuName = "Dialogue/Dialog Text Database")]
public class DialogTextDB : ScriptableObject
{
    private Dictionary<RoomID, Dictionary<DialogueID, List<string>>> dialogueDatabase;

    private void OnEnable()
    {
        dialogueDatabase = new Dictionary<RoomID, Dictionary<DialogueID, List<string>>>
        {
            {
                RoomID.Room1, new Dictionary<DialogueID, List<string>>
                {
                    { 
                        DialogueID.Dialogue1, new List<string> {
                        "Where is everyone...? Why am I the only one here?",
                        "No signal figures. Whatever happened, we're way too far out.",
                        "I need to find another way. Maybe there's something still working around here.",
                        }
                    },
                    { 
                        DialogueID.Dialogue2, new List<string> {
                        "These mechanisms... they've been completely torn apart. What the hell happened here?",
                        "An AI assistant? Maybe you're the only help I�ve got right now..."
                        }
                    },
                    {
                        DialogueID.Success, new List<string> {
                        "Alright... something's working again. One step at a time. Let's keep moving.",
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "This isn't working. I need to find another clue.",
                        }
                    },
                }
            },
            {
                RoomID.Room2, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                        "These systems... i think they're linked.",
                        }
                    },
                    {
                        DialogueID.Failed, new List<string> {
                        "Come on, think. I've never done this before, but someone has to try.",
                        }
                    },
                    {
                        DialogueID.Success, new List<string> {
                        "That should do it. Wait... what's that on the floor?",
                        }
                    },
                    {
                        DialogueID.Dialogue2, new List<string> {
                        "This message... it's corrupted. Someone tried to say something.",
                        "Why does everything feel... off? I need to find out what happened here.",
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "No response... Maybe I missed something.",
                        }
                    },
                }
            },
            {
                RoomID.Room3, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                        "Four doors. One central system. And that feeling again...",
                        "Why do I feel like I've done this before? I know I haven't... have I?",
                        }
                    },
                    {
                        DialogueID.Dialogue2, new List<string> {
                         "This setup’s massive... but what does it actually do?",
                         "Looks like more than just a terminal. Maybe it’s managing everything?",
                        }
                    },
                    {
                        DialogueID.Dialogue3, new List<string> {
                        "Huh... looks like a data log.",
                        "Wait... this isn't just a data log. It's a message. Someone was trying to warn me.",
                        "Did someone write this for me? It feels like they’re trying to tell me something.",
                        "I need to figure this out.",
                        }
                    },
                }
            },
            {
                RoomID.Room4, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                        "That sound... it's heavier here. Like something deeper is going on.",
                        "These paths... different colors, different directions. Is this some kind of test?",
                        }
                    },
                    {
                        DialogueID.Dialogue2, new List<string> {
                        "What the...? My reflection... is late. This isn't just my mind playing tricks.",
                        }
                    },
                    {
                        DialogueID.Dialogue3, new List<string> { //Saat lagi puzzle
                        "These system logs referencing things that haven't happened yet? How is that even possible?",
                        }
                    },
                    {
                        DialogueID.Dialogue4, new List<string> {
                        "Hey wait! What happened to you...? No... they�re not real. They can't be real.",
                        }
                    },
                    {
                        DialogueID.Dialogue5, new List<string> {
                        "Hold on... this wall it doesn't match. Something's hidden here.",
                        "Whatever's on the other side I need to know. I can't stop now.",
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "I need to solve the puzzle...",
                        }
                    },
                    {
                        DialogueID.Incomplete2, new List<string> {
                        "I can't go back, i need to figure this out...",
                        }
                    },
                }
            },
            {
                RoomID.Room5, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                        "Everything in here... it's looping. Like it's alive and stuck in its own memory.",
                        "No, no... I've seen this. I've walked this path before. Haven't I?",
                        "There's something wrong in my mind. Like I'm sharing space with... with someone else.",
                        "Those aren't reflections. They're echoes. Versions of me that never made it out?"
                        }
                    },
                    {
                        DialogueID.Dialogue2, new List<string> {
                        "These inputs... they're mimicking past events. This isn't just a loop it's a memory trap.",
                        "Okay... focus.",
                        }
                    },
                    {
                        DialogueID.Success, new List<string> {
                        "Damn it. It's like this room is trying to trap me.",
                        }
                    },
                    {
                        DialogueID.Dialogue4, new List<string> { //Text To Choose Decision
                        "The capsule... it's active. I could leave. I could just go home.",
                        "But if I go now, I may never know what happened. Why everyone's gone. Why I'm still here.",
                        "This might be my only chance to find the truth.",
                        }
                    },
                    {
                        DialogueID.Dialogue5, new List<string> { // Ending 2 (BAD ENDING)
                        "I�ve seen enough. I can't take this anymore... maybe it's better not knowing.",
                        }
                    },
                    {
                        DialogueID.Dialogue6, new List<string> { // Ending 1 (GOOD ENDING)
                        "No. I need answers. I owe them that much.",
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "I can't go back, i need to figure this out...",
                        }
                    },
                }
            },
            {
                RoomID.Room6, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                        "This was the room we used to gather... where we shared stories, laughed�",
                        "They were just here. It's like they vanished mid-sentence."
                        }
                    },
                    {
                        DialogueID.Dialogue2, new List<string> {
                        "All the pieces from before... they're here.",
                        "I remember this moment... we were happy. They were real.",
                        }
                    },
                    {
                        DialogueID.Dialogue3, new List<string> {
                        "The seventh room. That's where this ends. Or maybe where it all began.",
                        "I need to know the truth. No more running.",
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "Something's off. I'll come back to this.",
                        }
                    },
                }
            },
            {
                RoomID.Room7, new Dictionary<DialogueID, List<string>>
                {
                    {
                        DialogueID.Dialogue1, new List<string> {
                         "Let's hope this capsule still knows how to get to Earth.",
                         "I wish I had more time...",
                         "Alright… take me home."
                        }
                    },
                    {
                        DialogueID.Incomplete1, new List<string> {
                        "It seems need a power, i need to find something.",
                        }
                    },
                }
            },
        };
    }

    public List<string> GetDialogueLines(RoomID room, DialogueID id)
    {
        if (dialogueDatabase.ContainsKey(room) && dialogueDatabase[room].ContainsKey(id))
            return dialogueDatabase[room][id];

        return new List<string> { "Dialogue not found." };
    }
}
