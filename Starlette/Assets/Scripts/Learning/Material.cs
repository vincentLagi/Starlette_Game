using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MaterialPage
{
    public string pageName;
    public GameObject canvas;
    public MaterialPage nextPage;
    public MaterialPage prevPage;
    
    public MaterialPage(string name, GameObject canvasObj)
    {
        pageName = name;
        canvas = canvasObj;
        nextPage = null;
        prevPage = null;
    }
}

public class Material : MonoBehaviour
{
    [Header("Tutorial Pages")]
    [SerializeField] private List<GameObject> tutorialCanvases = new List<GameObject>();
    
    [Header("Navigation Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backToMenuButton;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI pageIndicatorText; // Optional: shows current page
    [SerializeField] private TextMeshProUGUI pageTitle; // Optional: reference to main menu canvas

    // Linked list structure
    private MaterialPage firstPage;
    private MaterialPage currentPage;
    private MaterialPage lastPage;
    
    // Tutorial topics in order
    private readonly List<string> tutorialTopics = new List<string>
    {
        "Introduction",
        "Input Output",
        "Data Types",
        "Variables",
        "Operators",
        "Operators - 2",
        "Operators - 3",
        "Input Output - 2",
        "Selection",
        "Looping",
        "Looping - 2",
    };
    
    void Start()
    {
        InitializeMaterialPages();
        SetupNavigationButtons();
    }
    
    private void InitializeMaterialPages()
    {
        if (tutorialCanvases.Count == 0)
        {
            Debug.LogError("No tutorial canvases assigned!");
            return;
        }

        // Create linked list of pages
        MaterialPage previousPage = null;

        for (int i = 0; i < tutorialCanvases.Count; i++)
        {
            string topicName = i < tutorialTopics.Count ? tutorialTopics[i] : $"Page {i + 1}";
            MaterialPage newPage = new MaterialPage(topicName, tutorialCanvases[i]);

            if (i == 0)
            {
                firstPage = newPage;
                currentPage = newPage;
            }

            if (previousPage != null)
            {
                previousPage.nextPage = newPage;
                newPage.prevPage = previousPage;
            }

            previousPage = newPage;

            if (i == tutorialCanvases.Count - 1)
            {
                lastPage = newPage;
            }
        }

        //Debug.Log($"Initialized {tutorialCanvases.Count} tutorial pages");
    }
    
    private void SetupNavigationButtons()
    {
        if (prevButton != null)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(GoToPreviousPage);
        }
        
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToNextPage);
        }
        
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveAllListeners();
            backToMenuButton.onClick.AddListener(BackToMainMenu);
        }
    }
    
    public void GoToNextPage()
    {
        if (currentPage != null && currentPage.nextPage != null)
        {
            currentPage = currentPage.nextPage;
            ShowCurrentPage();
            Debug.Log($"Navigated to: {currentPage.pageName}");
        }
        else
        {
            Debug.Log("No next page available");
        }
    }
    
    public void GoToPreviousPage()
    {
        if (currentPage != null && currentPage.prevPage != null)
        {
            currentPage = currentPage.prevPage;
            ShowCurrentPage();
            Debug.Log($"Navigated to: {currentPage.pageName}");
        }
        else
        {
            Debug.Log("No previous page available");
        }
    }

    public void BackToMainMenu()
    {
        // Hide all tutorial canvases
        HideAllPages();

        // You can implement your main menu logic here
        // For example:
        // SceneManager.LoadScene("MainMenu");
        // or activate main menu canvas

        Debug.Log("Returning to main menu");

        // Optional: Reset to first page for next time
        currentPage = firstPage;
    }
    
    private void ShowCurrentPage()
    {
        if (currentPage == null)
        {
            Debug.LogError("Current page is null!");
            return;
        }
        
        // Hide all canvases first
        HideAllPages();
        
        // Show current page canvas
        if (currentPage.canvas != null)
        {
            currentPage.canvas.SetActive(true);
        }
        
        // Update navigation button states
        UpdateNavigationButtons();
        
        // Update page indicator if available
        UpdatePageIndicator();
    }
    
    private void HideAllPages()
    {
        foreach (GameObject canvas in tutorialCanvases)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }
    
    private void UpdateNavigationButtons()
    {
        if (prevButton != null)
        {
            prevButton.interactable = (currentPage.prevPage != null);
        }
        
        if (nextButton != null)
        {
            nextButton.interactable = (currentPage.nextPage != null);
        }
    }
    
    private void UpdatePageIndicator()
    {
        if (pageIndicatorText != null && currentPage != null)
        {
            int currentIndex = GetCurrentPageIndex() + 1;
            pageIndicatorText.text = $"{currentIndex}/{tutorialCanvases.Count} - {currentPage.pageName}";
            pageTitle.text = currentPage.pageName; // Update main menu title if available
        }
    }
    
    private int GetCurrentPageIndex()
    {
        MaterialPage temp = firstPage;
        int index = 0;
        
        while (temp != null && temp != currentPage)
        {
            temp = temp.nextPage;
            index++;
        }
        
        return index;
    }
    
    // Public methods for external access
    public void GoToFirstPage()
    {
        if (firstPage != null)
        {
            currentPage = firstPage;
            ShowCurrentPage();
        }
    }
    
    public void GoToLastPage()
    {
        if (lastPage != null)
        {
            currentPage = lastPage;
            ShowCurrentPage();
        }
    }
    
    public void OpenOnSpecificPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= tutorialCanvases.Count)
        {
            Debug.LogError($"Invalid page index: {pageIndex}");
            return;
        }
        // GameObject.Find("LearningPageComponent").SetActive(true);
        
        MaterialPage temp = firstPage;
        for (int i = 0; i < pageIndex && temp != null; i++)
        {
            temp = temp.nextPage;
        }
        
        if (temp != null)
        {
            currentPage = temp;
            ShowCurrentPage();
        }
    }
    
    public string GetCurrentPageName()
    {
        return currentPage?.pageName ?? "Unknown";
    }
    
    public int GetTotalPages()
    {
        return tutorialCanvases.Count;
    }
    
    // Validation method for editor
    void OnValidate()
    {
        if (tutorialCanvases.Count != tutorialTopics.Count)
        {
            Debug.LogWarning($"Canvas count ({tutorialCanvases.Count}) doesn't match topic count ({tutorialTopics.Count})");
        }
    }
}