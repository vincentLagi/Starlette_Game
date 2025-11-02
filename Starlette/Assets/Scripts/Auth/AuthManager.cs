// Updated AuthManager with proper async handling
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AuthResponse
{
    public bool success;
    public string message;
    
    public AuthResponse(bool success, string message)
    {
        this.success = success;
        this.message = message;
    }
    
    public void SetMessage(string message)
    {
        this.message = message;
    }
    
    public string GetMessage()
    {
        return message;
    }
    
    public void SetSuccess(bool success)
    {
        this.success = success;
    }
    
    public bool GetSuccess()
    {
        return success;
    }
}

public class AuthManager : MonoBehaviour
{
    public void RegisterUser(string username, string password, Action<AuthResponse> onComplete)
    {
        // Show loading
        LoadingManager.Instance.ShowLoading("Creating account...", true, () =>
        {
            // Handle cancel
            onComplete?.Invoke(new AuthResponse(false, "Registration cancelled"));
        });

        FirebaseManager.Instance.DBReference.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            AuthResponse response = new AuthResponse(false, "");

            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error checking user: " + task.Exception);
                response.SetMessage("Network error. Please check your connection.");
                LoadingManager.Instance.HideLoading();
                onComplete?.Invoke(response);
                return;
            }

            if (task.Result.Exists)
            {
                Debug.Log("User already exists.");
                response.SetMessage("Username already taken. Please choose another.");
                LoadingManager.Instance.HideLoading();
                onComplete?.Invoke(response);
            }
            else
            {
                // Update loading message
                LoadingManager.Instance.UpdateLoadingMessage("Saving account data...");

                FirebaseManager.Instance.DBReference.Child("users").Child(username)
                    .Child("password").SetValueAsync(password).ContinueWithOnMainThread(setTask =>
                    {
                        LoadingManager.Instance.HideLoading();

                        if (setTask.IsCompleted && !setTask.IsFaulted)
                        {
                            Debug.Log("User registered successfully.");
                            response.SetSuccess(true);
                            response.SetMessage("Account created successfully!");
                        }
                        else
                        {
                            Debug.LogError("Error creating user: " + setTask.Exception);
                            response.SetMessage("Failed to create account. Please try again.");
                        }

                        onComplete?.Invoke(response);
                    });
            }
        });
    }

    public void LoginUser(string username, string password, Action<AuthResponse> onComplete)
    {
        // Show loading
        LoadingManager.Instance.ShowLoading("Signing in...", true, () =>
        {
            // Handle cancel
            onComplete?.Invoke(new AuthResponse(false, "Login cancelled"));
        });

        FirebaseManager.Instance.DBReference.Child("users").Child(username).Child("password")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                LoadingManager.Instance.HideLoading();
                AuthResponse response = new AuthResponse(false, "");

                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Error logging in: " + task.Exception);
                    response.SetMessage("Network error. Please check your connection.");
                    onComplete?.Invoke(response);
                    return;
                }

                if (task.Result.Exists && task.Result.Value.ToString() == password)
                {
                    Debug.Log("Login successful!");
                    response.SetSuccess(true);
                    response.SetMessage("Welcome back!");

                    // Store current user (you might want to add this)
                    PlayerPrefs.SetString("CurrentUser", username);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("Invalid username or password.");
                    response.SetMessage("Invalid username or password.");
                }

                onComplete?.Invoke(response);
            });
    }
}