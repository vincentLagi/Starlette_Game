using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Loading Manager - Handles all loading states
public class LoadingManager : MonoBehaviour
{
    [Header("Loading UI Components")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingSpinner;
    [SerializeField] private Button cancelButton;
    
    [Header("Loading Animation")]
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private string[] loadingMessages = { "Loading", "Loading.", "Loading..", "Loading..." };
    [SerializeField] private float messageChangeInterval = 0.5f;
    
    private static LoadingManager _instance;
    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LoadingManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("LoadingManager");
                    _instance = go.AddComponent<LoadingManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    private Coroutine loadingCoroutine;
    private Action onCancelCallback;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CancelLoading);
        }
        
        HideLoading();
    }
    
    public void ShowLoading(string message = "Loading...", bool showCancelButton = false, Action onCancel = null)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
        
        if (cancelButton != null)
        {
            cancelButton.gameObject.SetActive(showCancelButton);
            onCancelCallback = onCancel;
        }
        
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }
        
        loadingCoroutine = StartCoroutine(LoadingAnimation(message));
    }
    
    public void HideLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
        
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }
        
        onCancelCallback = null;
    }
    
    public void UpdateLoadingMessage(string message)
    {
        if (loadingText != null)
        {
            loadingText.text = message;
        }
    }
    
    private void CancelLoading()
    {
        onCancelCallback?.Invoke();
        HideLoading();
    }
    
    private IEnumerator LoadingAnimation(string baseMessage)
    {
        int messageIndex = 0;
        float timer = 0f;
        
        while (true)
        {
            // Animate spinner
            if (loadingSpinner != null)
            {
                loadingSpinner.transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
            }
            
            // Animate loading text
            timer += Time.deltaTime;
            if (timer >= messageChangeInterval)
            {
                timer = 0f;
                messageIndex = (messageIndex + 1) % loadingMessages.Length;
                
                if (loadingText != null)
                {
                    loadingText.text = baseMessage.Replace("Loading", loadingMessages[messageIndex]);
                }
            }
            
            yield return null;
        }
    }
}