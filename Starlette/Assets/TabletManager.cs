using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dialog1;
    public GameObject dialog2;
    public GameObject puzzleInterface;
    public GameObject tabletInterface;
    public GameObject errorInterface;
    public GameObject successInterface;

    public GameObject materialOneInterface;

    public TextMeshProUGUI error;
    public TextMeshProUGUI greetingText;
    public bool isDialogFinish;
    private string username;
    public TMP_InputField textarea;
    void Start()
    {
        isDialogFinish = true;
        error.gameObject.SetActive(false);
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDialogFinish)
        {

            setStatusDialog2Interface(false);
            setStatusPuzzleInterface(true);
            isDialogFinish = true;
        }
    }
    public void CheckUsername()
    {

        Debug.Log(textarea.text.ToString());
        Debug.Log(String.IsNullOrWhiteSpace(textarea.text));
        if (string.IsNullOrWhiteSpace(textarea.text))
        {
            error.gameObject.SetActive(true);
            error.text = "Username text is empty!";
            return;
        }

        isDialogFinish = false;

        username = textarea.text;
        setStatusDialog1Interface(false);
        setStatusDialog2Interface(true);
    }

    public string getUsername()
    {
        return username;
    }

    public void setStatusPuzzleInterface(bool b)
    {
        puzzleInterface.SetActive(b);
    }

    public void setStatusTabletInterface(bool b)
    {
        tabletInterface.SetActive(b);
    }

    public void setStatusDialog1Interface(bool b)
    {
        dialog1.SetActive(b);
    }

    public void setStatusDialog2Interface(bool b)
    {
        dialog2.SetActive(b);
    }

    public void setStatusErrorInterface(bool b)
    {
        errorInterface.SetActive(b);
    }

    public void setStatusSuccessInterface(bool b)
    {
        successInterface.SetActive(b);
    }
    
    public void setStatusMaterialOneInterface(bool b)
    {
        materialOneInterface.SetActive(b);
    }
}
