
using System.Collections.Generic;
using UnityEngine;

// Enums for transfer operations
public enum TransferType
{
    Move,    // Block is moved from source to destination
    Copy     // Block is copied to destination, original remains
}

public enum TransferResult
{
    Success,
    Failed,
    SourceFull,
    DestinationFull,
    InvalidBlock,
    SameContainer
}

// Base interface for all block containers
public interface IBlockContainer
{
    string ContainerName { get; }
    int BlockCount { get; }
    int MaxCapacity { get; }
    bool IsFull { get; }
    bool IsEmpty { get; }
    
    bool CanAcceptBlock(GameObject block);
    bool CanRemoveBlock(GameObject block);
    TransferResult AddBlock(GameObject block, TransferType transferType = TransferType.Move);
    TransferResult RemoveBlock(GameObject block);
    GameObject GetBlockAt(int index);
    List<GameObject> GetAllBlocks();
    void OnBlockClicked(GameObject block);
    void SetPartnerContainer(IBlockContainer partner);
}