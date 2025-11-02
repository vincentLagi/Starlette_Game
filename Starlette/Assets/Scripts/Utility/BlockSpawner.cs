using TMPro;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private BlockFactory blockFactory;
    private Canvas targetCanvas;
    private void Start()
    {
        blockFactory = GetComponent<BlockFactory>();
        targetCanvas = GameObject.FindGameObjectWithTag("PuzzleCanvas")?.GetComponent<Canvas>();
        if (targetCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
        }
    }


    public void SpawnBlock(BlockType blockType, Vector3 position)
    {
        GameObject testBlock = blockFactory.CreateBlock(BlockType.Literal_Int, DataType.CreateDataType<int>(5), targetCanvas.transform);
        testBlock.transform.position = new Vector3(0, 0, 0);
        testBlock.transform.localScale = new Vector3(1, 1, 1);
        TextMeshProUGUI text = testBlock.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = testBlock.GetComponent<CodeBlock>().ToString();
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on the block.");
        }
    }

}