using TMPro;
using UnityEngine;

public class WhileBlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    WhilePuzzleEmptyBlock whilePuzzleEmptyBlock;
    void Start()
    {
        whilePuzzleEmptyBlock = GameObject.Find("PuzzleContent").GetComponent<WhilePuzzleEmptyBlock>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        Debug.Log("WhileValueBlock clicked!");

        if (HasParentNamedBlock(gameObject))
        {
            whilePuzzleEmptyBlock.RemoveBlock(gameObject, this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        else
        {
            whilePuzzleEmptyBlock.AddBlock(this.gameObject);
        }
    }
     bool HasParentNamedBlock(GameObject gameObject)
    {
        Transform currentParent = gameObject.transform.parent;
        
        if (currentParent.name.Contains("block"))
        {
            return true;
        }
        
        return false;
    }
}
