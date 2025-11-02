using UnityEngine;


public class BlockSlot : MonoBehaviour
{
    [SerializeField] private CodeBlock block;
    private GameObject background;
    private void Awake()
    {
        background = transform.Find("Background").gameObject;
        if (block != null)
        {
            SetBlock(block);
        }
    }

    public CodeBlock GetBlock()
    {
        return block;
    }

    public void SetBlock(CodeBlock newBlock)
    {
        block = newBlock;
        if (block != null)
        {
            block.gameObject.transform.SetParent(transform, false);
            block.gameObject.transform.localPosition = Vector3.zero;
            background.SetActive(false);
        }
    }

    public void Clear()
    {
        block = null;
        background.SetActive(true);
    }
    
    public void DestroyBlock()
    {
        if (block != null)
        {
            Destroy(block.gameObject);
            Clear();
        }
    }
}