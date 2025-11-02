using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomProgressManager : MonoBehaviour
{
    public static RoomProgressManager Instance;
    public List<RoomProgressData> allRooms = new();
    private float gameplayStartTime = 0f;
    private float totalGameplayTime = 0f;
    private bool gameplayRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (RoomID id in System.Enum.GetValues(typeof(RoomID)))
            {
                allRooms.Add(new RoomProgressData
                {
                    roomID = id,
                    progress = 0f,
                    unfinishedPuzzles = new List<string>(),
                    timeSpent = 0f,
                    totalPuzzles = 0
                });
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    StartGameplayTimer();
    //}

    //void Update()
    //{
    //    if (gameplayRunning)
    //    {
    //        Debug.Log($"Gameplay Time: {GetTotalGameplayTime():F2} seconds");
    //    }
    //}

    public void StartGameplayTimer()
    {
        if (!gameplayRunning)
        {
            gameplayStartTime = Time.time;
            gameplayRunning = true;
            Debug.Log("username = " + PlayerPrefs.GetString("Username"));
        }
    }

    public void StopGameplayTimer()
    {
        if (gameplayRunning)
        {
            totalGameplayTime += Time.time - gameplayStartTime;
            gameplayRunning = false;
        }
    }

    public float GetTotalGameplayTime()
    {
        if (gameplayRunning)
            return totalGameplayTime + (Time.time - gameplayStartTime);
        else
            return totalGameplayTime;
    }



    public void RegisterPuzzle(RoomID room, string puzzleID)
    {
        var data = GetRoomData(room);
        if (!data.unfinishedPuzzles.Contains(puzzleID))
        {
            data.unfinishedPuzzles.Add(puzzleID);
            data.totalPuzzles++;
        }
        UpdateRoomProgress(room);
    }

    public void MarkPuzzleFinished(RoomID room, string puzzleID)
    {
        var data = GetRoomData(room);
        if (data.unfinishedPuzzles.Contains(puzzleID))
        {
            data.unfinishedPuzzles.Remove(puzzleID);
            UpdateRoomProgress(room);
        }
    }

    private void UpdateRoomProgress(RoomID room)
    {
        var data = GetRoomData(room);
        int puzzlesLeft = data.unfinishedPuzzles.Count;
        int total = Mathf.Max(data.totalPuzzles, 1);
        data.progress = 100f * (total - puzzlesLeft) / total;
    }

    public RoomProgressData GetRoomData(RoomID room)
    {
        return allRooms.Find(r => r.roomID == room);
    }

    public float GetTotalProgress()
    {
        float total = 0f;
        foreach (var room in allRooms)
        {
            total += room.progress;
        }
        return total / allRooms.Count;
    }

    public float GetTotalTime()
    {
        float total = 0f;
        foreach (var room in allRooms)
        {
            total += room.timeSpent;
        }
        return total;
    }

    public void PushAttemptToFirebase()
    {

        //String username = "ZetKa"; // Ganti dengan username yang sesuai

        Debug.Log("username = " + PlayerPrefs.GetString("Username"));

        if (!PlayerPrefs.HasKey("Username"))
        {
            Debug.LogWarning("Username key not found. Skipping attempt push.");
            return;
        }

        string username = PlayerPrefs.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("No valid username found. Skipping attempt push.");
            return;
        }

        var attemptsRef = FirebaseManager.Instance.DBReference
            .Child("gameProgress")
            .Child(username)
            .Child("attempts");

        // Ambil jumlah attempt yang sudah ada
        attemptsRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to get attempts: " + task.Exception);
                return;
            }

            int nextAttemptNumber = 1;

            if (task.Result.Exists)
            {
                // next attemptnya
                nextAttemptNumber = (int)task.Result.ChildrenCount + 1;
            }

            var newAttempt = new RoomUploadPayload
            {
                time = GetTotalGameplayTime(),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            attemptsRef.Child(nextAttemptNumber.ToString())
                .SetRawJsonValueAsync(JsonUtility.ToJson(newAttempt))
                .ContinueWithOnMainThread(pushTask =>
                {
                    if (pushTask.IsCompleted)
                        Debug.Log($"Attempt {nextAttemptNumber} pushed for user {username}");
                    else
                        Debug.LogError("Failed to push attempt: " + pushTask.Exception);
                });
        });
    }

    public void OnGameFinished()
    {
        StopGameplayTimer();

        PushAttemptToFirebase();

        Debug.Log($"Game finished! Total gameplay time: {GetTotalGameplayTime()} seconds");
    }


    [System.Serializable]
    public class RoomUploadPayload
    {
        public float time;
        public string timestamp;
    }

}

