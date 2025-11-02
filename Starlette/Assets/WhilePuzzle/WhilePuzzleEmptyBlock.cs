using System.Collections.Generic;
using UnityEngine;

public class WhilePuzzleEmptyBlock : MonoBehaviour
{
    private List<GameObject> listBlockMenu = new List<GameObject>();
    [SerializeField] private GameObject[] listEmptyValueBlock;
    [SerializeField] private GameObject[] listEmptyOperatorBlock;
    public WhileChoiceBlockMenu choiceBlockMenu;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetLengthListBlockMenu()
    {
        return listBlockMenu.Count;
    }

    public GameObject[] GetListEmptyValueBlock()
    {
        return listEmptyValueBlock;
    }
    public GameObject[] GetListEmptyOperatorBlock()
    {
        return listEmptyOperatorBlock;
    }
    public void RemoveBlock(GameObject gameObject, string text)
    {
        listBlockMenu.Remove(gameObject);
        choiceBlockMenu.SpawnNewObject(gameObject, text);
        if (gameObject.name.Contains("WhileValueBlock"))
        {
            foreach (GameObject item in listEmptyValueBlock)
            {
                foreach (Transform child in item.transform)
                {
                    if (child.gameObject == gameObject)
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject item in listEmptyOperatorBlock)
            {
                foreach (Transform child in item.transform)
                {
                    if (child.gameObject == gameObject)
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
            }
        }

    }

    public void AddBlock(GameObject gameObject)
    {
        listBlockMenu.Add(gameObject);
        if (gameObject.name.Contains("WhileValueBlock"))
        {
            CheckListEmptyValueBlock(gameObject);

        }
        else
        {
            CheckListEmptyOperatorBlock(gameObject);
        }
        choiceBlockMenu.RecalculateAllPositions();
    }

    void CheckListEmptyOperatorBlock(GameObject gameObject)
    {
        foreach (GameObject item in listEmptyOperatorBlock)
        {
            if (item.transform.childCount == 0)
            {
                gameObject.transform.SetParent(item.transform);

                gameObject.transform.localPosition = new Vector3(-2.5f, 0f, 0f);
                gameObject.transform.localRotation = Quaternion.identity;
    
                break;
            }
        }
    }

    void CheckListEmptyValueBlock(GameObject gameObject)
    {
        foreach (GameObject item in listEmptyValueBlock)
        {
            if (item.transform.childCount == 0)
            {
                gameObject.transform.SetParent(item.transform);

                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;


                break;
            }
        }
    }
}
