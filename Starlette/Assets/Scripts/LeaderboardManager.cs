using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardContent, userDataPrefab;

    private DatabaseReference db;
    private Dictionary<string, List<AttemptData>> userAttempts = new Dictionary<string, List<AttemptData>>();

    void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        db = FirebaseDatabase.DefaultInstance.GetReference("gameProgress");

        db.ChildAdded += OnUserAdded;
        db.ChildChanged += OnUserChanged;
    }

    void OnUserAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.Snapshot.Exists)
        {
            LoadUserAttempts(args.Snapshot);
        }
    }

    void OnUserChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.Snapshot.Exists)
        {
            LoadUserAttempts(args.Snapshot);
        }
    }

    void LoadUserAttempts(DataSnapshot userSnapshot)
    {
        string username = userSnapshot.Key;

        if (!userAttempts.ContainsKey(username))
        {
            userAttempts[username] = new List<AttemptData>();
        }
        else
        {
            userAttempts[username].Clear();
        }

        // Check if user has attempts
        if (userSnapshot.HasChild("attempts"))
        {
            DataSnapshot attemptsSnapshot = userSnapshot.Child("attempts");

            foreach (DataSnapshot attemptSnapshot in attemptsSnapshot.Children)
            {
                // int room = attemptSnapshot.HasChild("room") ? int.Parse(attemptSnapshot.Child("room").Value.ToString()) : 0;
                float time = attemptSnapshot.HasChild("time") ? Convert.ToSingle(attemptSnapshot.Child("time").Value.ToString()) : 0;
                long timestamp = attemptSnapshot.HasChild("timestamp") ? ParseLong(attemptSnapshot.Child("timestamp").Value) : 0;


                AttemptData attempt = new AttemptData(username, time, timestamp);
                userAttempts[username].Add(attempt);
            }
        }

        UpdateLeaderboardUI();
    }

    AttemptData GetBestAttempt(List<AttemptData> attempts)
    {
        if (attempts == null || attempts.Count == 0)
            return null;

        // Filter attempts with room >= 6
        // var validAttempts = attempts.Where(a => a.room >= 6).ToList();

        // if (validAttempts.Count == 0)
        //     return null;

        // Sort by room (descending) then by time (ascending) then by timestamp (ascending for earliest)
        attempts.Sort((a, b) =>
        {
            // int roomComparison = b.room.CompareTo(a.room); // Higher room is better
            // if (roomComparison != 0)
            //     return roomComparison;

            int timeComparison = a.time.CompareTo(b.time); // Lower time is better
            if (timeComparison != 0)
                return timeComparison;

            return a.timestamp.CompareTo(b.timestamp); // Earlier timestamp is better (in case of ties)
        });

        return attempts[0];
    }

    void UpdateLeaderboardUI()
    {
        // Clear existing UI elements
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Get best attempts for each user
        List<AttemptData> bestAttempts = new List<AttemptData>();

        foreach (var kvp in userAttempts)
        {
            AttemptData bestAttempt = GetBestAttempt(kvp.Value);
            if (bestAttempt != null)
            {
                bestAttempts.Add(bestAttempt);
            }
        }

        // Sort the best attempts for leaderboard display
        bestAttempts.Sort((a, b) =>
        {
            // int roomComparison = b.room.CompareTo(a.room); // Higher room is better
            // if (roomComparison != 0)
            //     return roomComparison;

            int timeComparison = a.time.CompareTo(b.time); // Lower time is better
            if (timeComparison != 0)
                return timeComparison;

            return a.timestamp.CompareTo(b.timestamp); // Earlier timestamp is better
        });

        int rank = 1;

        foreach (AttemptData attempt in bestAttempts)
        {
            GameObject entry = Instantiate(userDataPrefab, leaderboardContent.transform);
            entry.transform.localScale = Vector3.one;

            if (rank % 2 == 0)
            {
                Image background = entry.GetComponent<Image>();
                background.color = new Color32(24, 7, 38, 255);
            }

            DataUI dataUI = entry.GetComponent<DataUI>();

            dataUI.rankText.text = rank.ToString();
            dataUI.usernameText.text = attempt.username;
            // dataUI.roomText.text = attempt.room.ToString();
            dataUI.timeText.text = string.Format("{0:0.00}s", attempt.time);

            rank++;
        }
    }

    // Method to add a new attempt for a user
    public void AddNewAttempt(string username, int room, int time)
    {
        DatabaseReference userRef = db.Child(username).Child("attempts");

        // Get current timestamp
        long timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Get the next attempt number
        userRef.RunTransaction(mutableData =>
        {
            if (mutableData.Value == null)
            {
                // First attempt
                Dictionary<string, object> firstAttempt = new Dictionary<string, object>
                {
                    ["room"] = room,
                    ["time"] = time,
                    ["timestamp"] = timestamp
                };
                mutableData.Child("1").Value = firstAttempt;
            }
            else
            {
                // Find the next attempt number
                int nextAttemptNumber = 1;
                foreach (var child in mutableData.Children)
                {
                    if (int.TryParse(child.Key, out int attemptNum))
                    {
                        nextAttemptNumber = Mathf.Max(nextAttemptNumber, attemptNum + 1);
                    }
                }

                Dictionary<string, object> newAttempt = new Dictionary<string, object>
                {
                    ["room"] = room,
                    ["time"] = time,
                    ["timestamp"] = timestamp
                };
                mutableData.Child(nextAttemptNumber.ToString()).Value = newAttempt;
            }

            return TransactionResult.Success(mutableData);
        });
    }

    public void NavigateToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu");
    }
    
    long ParseLong(object value)
    {
        if (value == null) return 0;
        
        if (value is long longValue)
            return longValue;
        
        if (value is int intValue)
            return intValue;
        
        if (value is double doubleValue)
            return (long)doubleValue;
        
        // Try parsing as Unix timestamp first
        if (long.TryParse(value.ToString(), out long result))
            return result;
        
        // Try parsing as DateTime string (ISO 8601 format)
        string dateString = value.ToString();
        if (System.DateTime.TryParse(dateString, out System.DateTime dateTime))
        {
            // Convert DateTime to Unix timestamp
            return ((System.DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }
        
        Debug.LogWarning($"Failed to parse long from value: {value} (Type: {value.GetType()})");
        return 0;
    }
}

public class AttemptData
{
    public string username;
    // public int room;
    public float time;
    public long timestamp;

    public AttemptData(string username, float time, long timestamp)
    {
        this.username = username;
        // this.room = room;
        this.time = time;
        this.timestamp = timestamp;
    }
}