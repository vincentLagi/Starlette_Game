using System;
using System.Collections.Generic;

[Serializable]
public class RoomProgressData
{
    public RoomID roomID;
    public float progress;
    public List<string> unfinishedPuzzles = new();
    public float timeSpent;
    public float startTime;
    public int totalPuzzles;
}