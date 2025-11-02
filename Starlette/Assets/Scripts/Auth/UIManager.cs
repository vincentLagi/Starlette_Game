using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private enum UIState
    {
        Login, Register
    }

    [Header("Input Fields")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    [Header("UI Elements")]
    public TextMeshProUGUI panelTitle;
    public Button changeStateButton;
    public Button submitButton;
    public Button backButton; // Optional: for going back to main menu

    [Header("Validation")]
    [SerializeField] private int minUsernameLength = 3;
    [SerializeField] private int minPasswordLength = 6;

    [Header("References")]
    public AuthManager authManager;

    private UIState currentState = UIState.Login;

    private void Start()
    {
        // Initialize UI state
        SetLoginState();
    }

    public void ToggleUIState()
    {
        if (currentState == UIState.Login)
        {
            SetRegisterState();
        }
        else
        {
            SetLoginState();
        }
    }

    private void SetLoginState()
    {
        currentState = UIState.Login;
        panelTitle.text = "Login";
        changeStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Register new player";
        submitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sign In";

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnLoginButton);
    }

    private void SetRegisterState()
    {
        currentState = UIState.Register;
        panelTitle.text = "Register";
        changeStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back to Sign In";
        submitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Create Account";

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnRegisterButton);
    }

    public void OnRegisterButton()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        // Validate input
        if (!ValidateInput(username, password))
        {
            return;
        }

        // Disable UI during operation
        SetUIInteractable(false);

        authManager.RegisterUser(username, password, (response) =>
        {
            // Re-enable UI
            SetUIInteractable(true);

            // Show result
            if (response.GetSuccess())
            {
                NotificationManager.Instance.ShowNotification(response.GetMessage(), NotificationManager.NotificationType.Success);

                // Clear fields and switch to login
                ClearInputFields();
                SetLoginState();
            }
            else
            {
                NotificationManager.Instance.ShowNotification(response.GetMessage(), NotificationManager.NotificationType.Error);
            }
        });
    }

    public void OnLoginButton()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        // Validate input
        if (!ValidateInput(username, password))
        {
            return;
        }

        // Disable UI during operation
        SetUIInteractable(false);

        authManager.LoginUser(username, password, (response) =>
        {
            // Re-enable UI
            SetUIInteractable(true);

            // Show result
            if (response.GetSuccess())
            {
                NotificationManager.Instance.ShowNotification(response.GetMessage(), NotificationManager.NotificationType.Success);

                // Proceed to main game after a short delay
                PlayerPrefs.SetString("Username", username);
                PlayerPrefs.Save();
                //Ini pindah scene kalua udah login
                StartCoroutine(LoadMainGameAfterDelay(1.5f));
            }
            else
            {
                NotificationManager.Instance.ShowNotification(response.GetMessage(), NotificationManager.NotificationType.Error);
            }
        });
    }

    private bool ValidateInput(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || username.Length < minUsernameLength)
        {
            NotificationManager.Instance.ShowNotification($"Username must be at least {minUsernameLength} characters long.", NotificationManager.NotificationType.Warning);
            return false;
        }

        if (string.IsNullOrEmpty(password) || password.Length < minPasswordLength)
        {
            NotificationManager.Instance.ShowNotification($"Password must be at least {minPasswordLength} characters long.", NotificationManager.NotificationType.Warning);
            return false;
        }

        // Check for invalid characters in username
        if (!IsValidUsername(username))
        {
            NotificationManager.Instance.ShowNotification("Username can only contain letters, numbers, and underscores.", NotificationManager.NotificationType.Warning);
            return false;
        }

        return true;
    }

    private bool IsValidUsername(string username)
    {
        foreach (char c in username)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                return false;
            }
        }
        return true;
    }

    private void SetUIInteractable(bool interactable)
    {
        usernameInput.interactable = interactable;
        passwordInput.interactable = interactable;
        submitButton.interactable = interactable;
        changeStateButton.interactable = interactable;
    }

    private void ClearInputFields()
    {
        usernameInput.text = "";
        passwordInput.text = "";
    }

    private System.Collections.IEnumerator LoadMainGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("mainMenu");
    }
    
    public void OnBackButton()
    {
        // Optional: Implement logic to return to the main menu or previous screen
        SceneManager.LoadScene("mainMenu");
    }
}