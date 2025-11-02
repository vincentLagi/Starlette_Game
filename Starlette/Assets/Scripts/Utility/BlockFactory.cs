
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BlockType
{
    Literal_Int,
    Literal_Float,
    Literal_Bool,
    Variable_Int,
    Variable_Float,
    Variable_Bool,
    Variable_Adapter,
    AssignmentOperator,
    LogicalOperator,
    ArithmeticOperator,
    ComparisonOperator,
    Parenthesis,
}

public class BlockFactory : MonoBehaviour
{
    public static BlockFactory Instance;
    [Serializable]
    public struct BlockEntry
    {
        public BlockType BlockType;
        public GameObject Prefab;
    }

    public List<BlockEntry> BlockEntries;
    private Dictionary<BlockType, GameObject> blockPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        blockPrefabs = new Dictionary<BlockType, GameObject>();
        foreach (var entry in BlockEntries)
        {
            blockPrefabs[entry.BlockType] = entry.Prefab;
        }
    }

    /// how do i get block type based on class type
    public BlockType GetBlockType<T>() where T : CodeBlock
    {
        foreach (var entry in BlockEntries)
        {
            if (entry.Prefab.GetComponent<T>() != null)
            {
                return entry.BlockType;
            }
        }
        throw new ArgumentException($"No block type found for class {typeof(T).Name}");
    }

    public static BlockType GetBlockTypeFromVariables(VariableBlock variable)
    {
        if (variable.GetValue().GetValue() is Integer)
        {
            return BlockType.Variable_Int;
        }
        else if (variable.GetValue().GetValue() is FloatType)
        {
            return BlockType.Variable_Float;
        }
        else if (variable.GetValue().GetValue() is Boolean)
        {
            return BlockType.Variable_Bool;
        }
        else
        {
            throw new ArgumentException($"Unsupported variable type: {variable.GetType().Name}");
        }
    }

    public GameObject CreateBlock(BlockType type, object value, Transform parent = null)
    {
        if (!blockPrefabs.ContainsKey(type))
        {
            throw new ArgumentException($"Block type {type} not found in factory.");
        }

        GameObject prefab = blockPrefabs[type];
        GameObject instance = Instantiate(prefab, parent);
        if (value != null)
        {
            CodeBlock block = instance.GetComponent<CodeBlock>();
            if (block != null)
            {
                block.Init(value);
            }
            TextMeshProUGUI text = instance.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = block.ToString();
            }
        }
        return instance;
    }
}