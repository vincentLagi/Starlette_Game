using TMPro;
using UnityEngine;

public class ForChoiceBlockMenu : MonoBehaviour
{
    public void SpawnNewObject(GameObject prefabToSpawn, string text)
    {
        GameObject newObj = Instantiate(prefabToSpawn, transform);
        newObj.GetComponentInChildren<TextMeshProUGUI>().text = text;
        SetupNewObject(newObj);
        RecalculateAllPositions();
    }

    private void SetupNewObject(GameObject newObj)
    {
        RectTransform newRect = newObj.GetComponent<RectTransform>();
        newRect.localScale = Vector3.one;
        newRect.anchorMin = new Vector2(0, 0.5f);
        newRect.anchorMax = new Vector2(0, 0.5f);
        newRect.pivot = new Vector2(0.5f, 0.5f);
    }

    public void OnObjectDestroyed()
    {
        RecalculateAllPositions();
    }

    public void RecalculateAllPositions()
    {
        float currentX = 0f;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childRect = transform.GetChild(i).GetComponent<RectTransform>();
            
            if (i == 0)
            {
                currentX = childRect.rect.width / 2;
            }
            else
            {
                RectTransform prevChildRect = transform.GetChild(i-1).GetComponent<RectTransform>();
                currentX += prevChildRect.rect.width / 2 + childRect.rect.width / 2;
            }
            
            childRect.anchoredPosition = new Vector2(currentX, 0);
        }
    }

}
