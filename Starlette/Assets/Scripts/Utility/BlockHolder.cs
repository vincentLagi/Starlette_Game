using System.Collections.Generic;
using UnityEngine;

public class BlockHolder : BaseBlockContainer
{
    [Header("Free Container Settings")]
    [SerializeField] private float blockSpacing = 1.0f;
    [SerializeField] private float blockHeight = 1.0f;
    [SerializeField] private bool centerHolderHorizontally = true;
    [SerializeField] private bool snapToGrid = false;
    [SerializeField] private float gridSize = 0.5f;
    
    private float totalWidth = 0f;
    
    protected override void UpdateBlockPositions()
    {
        CleanupNullBlocks();

        if (blocks.Count == 0)
        {
            totalWidth = 0f;
            return;
        }

        RectTransform panel = GetComponent<RectTransform>();
        float panelWidth = panel.rect.width;

        float currentX = !centerHolderHorizontally ? 0f - panelWidth / 2f : 0f;

        // Prepare to track rows
        List<List<GameObject>> rows = new List<List<GameObject>>();
        List<GameObject> currentRow = new List<GameObject>();

        // First, determine how many rows there will be and which blocks go where
        for (int i = 0; i < blocks.Count; i++)
        {
            GameObject block = blocks[i];
            float blockWidth = GetBlockWidth(block);

            // Check if this block would overflow
            if (currentX + (panelWidth / 2f) + blockWidth > panelWidth && currentRow.Count > 0)
            {
                // Move to next row
                rows.Add(currentRow);
                currentRow = new List<GameObject>();
                currentX = !centerHolderHorizontally ? 0f - panelWidth / 2f : 0f;
            }

            currentRow.Add(block);
            currentX += blockWidth + blockSpacing;
        }

        // Add the final row
        if (currentRow.Count > 0)
            rows.Add(currentRow);

        int rowsCount = rows.Count;

        // Determine the starting y position to center rows
        float totalRowsHeight = (rowsCount - 1) * (blockHeight + blockSpacing);
        float startY = totalRowsHeight / 2f;

        // Now position each block in each row
        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            List<GameObject> row = rows[rowIndex];

            // Reset x for this row
            float currentRowX = !centerHolderHorizontally ? 0f - panelWidth / 2f : 0f;

            float currentY = startY - rowIndex * (blockHeight + blockSpacing);

            foreach (GameObject block in row)
            {
                float blockWidth = GetBlockWidth(block);

                Vector3 newPosition = new Vector3(currentRowX + blockWidth / 2f, currentY, 0f);

                if (snapToGrid)
                {
                    newPosition.x = Mathf.Round(newPosition.x / gridSize) * gridSize;
                    newPosition.y = Mathf.Round(newPosition.y / gridSize) * gridSize;
                }

                RectTransform rectTransform = block.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = newPosition;
                }
                else
                {
                    block.transform.localPosition = newPosition;
                }

                currentRowX += blockWidth + blockSpacing;
            }
        }

        // Update Content's height to fit all rows
        float contentHeight = totalRowsHeight + blockHeight; // add one blockHeight for first row
        RectTransform contentRect = GetComponent<RectTransform>();
        if (contentRect != null)
        {
            Vector2 size = contentRect.sizeDelta;
            size.y = contentHeight;
            contentRect.sizeDelta = size;
        }
    }
    
    protected override bool ValidateBlockPlacement(GameObject block, int index = -1)
    {
        // Free container accepts any block
        return block != null;
    }
    
    private float GetBlockWidth(GameObject block)
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