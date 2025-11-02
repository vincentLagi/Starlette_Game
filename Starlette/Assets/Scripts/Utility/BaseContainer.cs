using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Base abstract class for block containers
public abstract class BaseBlockContainer : MonoBehaviour
{
    [Header("Container Settings")]
    [SerializeField] protected string containerName = "Container";
    [SerializeField] protected int maxCapacity = -1; // -1 means unlimited
    [SerializeField] protected bool showGizmos = true;
    [SerializeField] protected Color gizmoColor = Color.green;

    [Header("Partner Container")]
    [SerializeField] protected BaseBlockContainer partnerContainer;
    [SerializeField] protected TransferType defaultTransferType = TransferType.Move;
    [SerializeField] protected bool receiver = true;

    protected List<GameObject> blocks = new List<GameObject>();

    // Properties
    public string ContainerName => containerName;
    public int BlockCount => blocks.Count;
    public int MaxCapacity => maxCapacity;
    public bool IsFull => maxCapacity > 0 && blocks.Count >= maxCapacity;
    public bool IsEmpty => blocks.Count == 0;

    // Abstract methods that derived classes must implement
    protected abstract void UpdateBlockPositions();
    protected abstract bool ValidateBlockPlacement(GameObject block, int index = -1);

    // Virtual methods that can be overridden
    public virtual bool CanAcceptBlock(GameObject block)
    {
        if (block == null) return false;
        if (IsFull) return false;
        return ValidateBlockPlacement(block);
    }

    public virtual bool CanRemoveBlock(GameObject block)
    {
        return block != null && blocks.Contains(block);
    }

    public void SetBlocks(List<GameObject> newBlocks)
    {
        blocks = newBlocks;
        UpdateBlockPositions();
    }
    public virtual TransferResult AddBlock(GameObject block, TransferType transferType = TransferType.Move)
    {
        if (block == null) return TransferResult.InvalidBlock;
        if (!CanAcceptBlock(block)) return TransferResult.DestinationFull;
       

        // Handle the block based on transfer type
        GameObject blockToAdd = block;
        if (transferType == TransferType.Copy)
        {
            blockToAdd = Instantiate(block);
            blockToAdd.GetComponent<CodeBlock>().Init(block.GetComponent<CodeBlock>());
        }
        // Add to container
        blockToAdd.transform.SetParent(transform);
        blockToAdd.transform.localPosition = Vector3.zero; // Reset position
        blockToAdd.transform.localScale = Vector3.one; // Reset scale
        blocks.Add(blockToAdd);

        
        UpdateBlockPositions();
        return TransferResult.Success;
    }

    public virtual TransferResult RemoveBlock(GameObject block)
    {
        if (!CanRemoveBlock(block)) return TransferResult.InvalidBlock;

        blocks.Remove(block);
        UpdateBlockPositions();
        return TransferResult.Success;
    }

    public virtual GameObject GetBlockAt(int index)
    {
        if (index >= 0 && index < blocks.Count)
            return blocks[index];
        return null;
    }

    public virtual List<GameObject> GetAllBlocks()
    {
        return new List<GameObject>(blocks);
    }

    public virtual void SetPartnerContainer(IBlockContainer partner)
    {
        if (partner is BaseBlockContainer basePartner)
        {
            partnerContainer = basePartner;
        }
    }

    public virtual TransferResult TransferBlockToPartner(GameObject block)
    {
        if (partnerContainer == null) return TransferResult.Failed;
        if (partnerContainer == this) return TransferResult.SameContainer;
        // Check if partner can accept the block
        Debug.Log(partnerContainer);
        if (partnerContainer.CanAcceptBlock(block) && partnerContainer.receiver && partnerContainer is FreeBlockContainer)
        {
            // find an empty slot in the partner's block list, then Set the block with the parameter
            // then remove the block from this container list
            BlockSlot[] slots = partnerContainer.GetComponentsInChildren<BlockSlot>(true);
            foreach (BlockSlot slot in slots)
            {
                if (slot.GetBlock() == null)
                {
                    CodeBlock newBlock = block.GetComponent<CodeBlock>();


                    if (defaultTransferType == TransferType.Move)
                    {
                        RemoveBlock(block);
                    }
                    else
                    {
                        newBlock = Instantiate(newBlock);
                        newBlock.Init(block.GetComponent<CodeBlock>());
                        newBlock.GetComponentInChildren<TextMeshProUGUI>().text = newBlock.ToString();
                        // Debug.Log($"New Value on block slot: {newBlock.GetComponent<VariableBlock>().GetValue().GetValue()}");
                    }
                    slot.SetBlock(newBlock);
                    return TransferResult.Success;
                }
            }

        }
        else if (partnerContainer is BlockHolder)
        {
            BlockSlot slot = block.GetComponentInParent<BlockSlot>();
            if (partnerContainer.receiver)
            {
                if (slot != null)
                {
                    slot.Clear();
                    partnerContainer.AddBlock(block);
                    Debug.Log("Moved Block Back");
                    return TransferResult.Success;
                }
                else if (defaultTransferType == TransferType.Move)
                {
                    RemoveBlock(block);
                    Destroy(block);
                    Debug.Log("Removed Block");
                    return TransferResult.Success;
                }
                else
                {
                    partnerContainer.AddBlock(block, defaultTransferType);
                    return TransferResult.Success;
                }
            }
            else
            {
                if (slot != null)
                {
                    RemoveBlock(block);
                    slot.DestroyBlock();
                    return TransferResult.Success;
                }
            }

        }
        else if (!partnerContainer.receiver)
        {
            RemoveBlock(block);
            Destroy(block);
            Debug.Log("Removed Block");
        }
        else
        {
            Debug.LogError("Fail");

        }

        return TransferResult.DestinationFull;
    }

    protected virtual void CleanupNullBlocks()
    {
        blocks.RemoveAll(block => block == null);
    }

    // Unity lifecycle
    protected virtual void Start()
    {
        UpdateBlockPositions();
    }

    protected virtual void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateBlockPositions();
        }
    }
}