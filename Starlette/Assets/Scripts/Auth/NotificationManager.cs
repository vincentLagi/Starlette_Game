using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [Header("Notification UI")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Image notificationBackground;
    [SerializeField] private Button closeButton;

    [Header("Notification Colors")]
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color errorColor = Color.red;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color infoColor = Color.blue;

    [Header("Auto Hide Settings")]
    [SerializeField] private float autoHideDelay = 3f;

    private static NotificationManager _instance;
    public static NotificationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<NotificationManager>();
            }
            return _instance;
        }
    }

    private Coroutine autoHideCoroutine;

    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideNotification);
        }

        HideNotification();
    }

    public void ShowNotification(string message, NotificationType type = NotificationType.Info, bool autoHide = true)
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
        }

        if (notificationText != null)
        {
            notificationText.text = message;
        }

        if (notificationBackground != null)
        {
            notificationBackground.color = GetColorForType(type);
        }

        if (autoHide)
        {
            if (autoHideCoroutine != null)
            {
                StopCoroutine(autoHideCoroutine);
            }
            autoHideCoroutine = StartCoroutine(AutoHideCoroutine());
        }
    }

    public void HideNotification()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
    }

    private Color GetColorForType(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.Success:
                return successColor;
            case NotificationType.Error:
                return errorColor;
            case NotificationType.Warning:
                return warningColor;
            case NotificationType.Info:
            default:
                return infoColor;
        }
    }

    private IEnumerator AutoHideCoroutine()
    {
        yield return new WaitForSeconds(autoHideDelay);
        HideNotification();
    }
}