using Unity.VisualScripting;
using UnityEngine;


public class FreeBlockContainer : BaseBlockContainer
{
    [Header("Free Container Settings")]
    [SerializeField] private float blockSpacing = 1.0f;
    [SerializeField] private float blockHeight = 1.0f;
    [SerializeField] private bool centerHolderHorizontally = true;
    [SerializeField] private bool snapToGrid = false;
    [SerializeField] private float gridSize = 0.5f;

    private float totalWidth = 0f;
    public void Awake()
    {
        // get all children then add it one by one
        foreach (Transform child in transform)
        {
            // Debug.Log($"Adding child: {child.name}");
            blocks.Add(child.gameObject);
        }
    }


    protected override void UpdateBlockPositions()
    {
        // CleanupNullBlocks();

        // if (blocks.Count == 0)
        // {
        //     totalWidth = 0f;
        //     return;
        // }

        // float currentX = 0f;
        // totalWidth = 0f;

        // RectTransform panel = GetComponent<RectTransform>();
        // float panelWidth = 1f;
        // if (panel != null)
        // {
        //     panelWidth = Mathf.Abs(panel.rect.width);
        //     if (panelWidth <= 0)
        //     {
        //         panelWidth = Mathf.Abs(panel.sizeDelta.x);
        //     }
        // }

        // // Calculate total width first
        // for (int i = 0; i < blocks.Count; i++)
        // {
        //     float blockWidth = GetBlockWidth(blocks[i]);
        //     totalWidth += blockWidth;
        //     if (i < blocks.Count - 1)
        //     {
        //         totalWidth += blockSpacing;
        //     }
        // }

        // // Calculate starting position
        // float startX = centerHolderHorizontally ? 0f : -panelWidth / 2f;
        // currentX = startX;

        // // Position each block
        // for (int i = 0; i < blocks.Count; i++)
        // {
        //     GameObject block = blocks[i];
        //     float blockWidth = GetBlockWidth(block);

        //     Vector3 newPosition = new Vector3(currentX + blockWidth / 2f, 0f, 0f);

        //     if (snapToGrid)
        //     {
        //         newPosition.x = Mathf.Round(newPosition.x / gridSize) * gridSize;
        //         newPosition.y = Mathf.Round(newPosition.y / gridSize) * gridSize;
        //     }

        //     RectTransform rectTransform = block.GetComponent<RectTransform>();
        //     if (rectTransform != null)
        //     {
        //         rectTransform.anchoredPosition = newPosition;
        //         if (rectTransform.anchorMin != new Vector2(0.5f, 0.5f) ||
        //             rectTransform.anchorMax != new Vector2(0.5f, 0.5f))
        //         {
        //             rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        //             rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        //         }
        //     }
        //     else
        //     {
        //         block.transform.localPosition = newPosition;
        //     }

        //     currentX += blockWidth + blockSpacing;
        // }
    }

    protected override bool ValidateBlockPlacement(GameObject block, int index = -1)
    {
        // Free container accepts any block
        return block != null;
    }

    public static float GetBlockWidth(GameObject block)
    {
        if (block == null) return 1f;

        float width = 1f;

        RectTransform rectTransform = block.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            width = Mathf.Abs(rectTransform.rect.width);
            if (width <= 0)
            {
                width = Mathf.Abs(rectTransform.sizeDelta.x);
            }
            if (width <= 0)
            {
                width = 60f;
            }
            return width;
        }

        SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            return spriteRenderer.bounds.size.x;
        }

        Collider2D collider = block.GetComponent<Collider2D>();
        if (collider != null)
        {
            return collider.bounds.size.x;
        }

        return width;
    }

    public float GetSpacing()
    {
        return blockSpacing;
    }

    public float GetTotalWidth()
    {
        return totalWidth;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(totalWidth, blockHeight, 0.1f);
        Gizmos.DrawWireCube(center, size);

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != null)
            {
                Vector3 blockPos = transform.TransformPoint(blocks[i].transform.localPosition);
                Gizmos.DrawWireSphere(blockPos, 0.1f);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(blockPos + Vector3.up * 0.5f, i.ToString());
#endif
            }
        }
    }
}