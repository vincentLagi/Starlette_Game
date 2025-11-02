using TMPro;
using UnityEngine;

public class ForBlock : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    ForPuzzleEmptyBlock forPuzzleEmptyBlock;
    void Start()
    {
        forPuzzleEmptyBlock = GameObject.Find("ForPuzzleContent").GetComponent<ForPuzzleEmptyBlock>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {

        if (HasParentNamedBlock(gameObject))
        {
            forPuzzleEmptyBlock.RemoveBlock(gameObject, this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        else
        {
            forPuzzleEmptyBlock.AddBlock(this.gameObject);
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
