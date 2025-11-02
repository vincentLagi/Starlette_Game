using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IfElseSlider : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI speedText;
    public Color color;
    public IfElseTask iet;
    float speed;
    void Awake()
    {
        image.fillAmount = 0f;
        float.TryParse(speedText.text, out speed);
    }
    void Start()
    {
        image.fillAmount = 0f;
        float.TryParse(speedText.text, out speed);
        //Debug.Log(speed);
    }
    public void StartFill()
    {
        float.TryParse(speedText.text, out speed);
        StartCoroutine(FillOverTime());
        
    }

    private IEnumerator FillOverTime()
    {
        while (image.fillAmount < 1f)
        {
            image.fillAmount += speed * Time.deltaTime * 2f;
            if (image.fillAmount >= 1f)
            {
                iet.AddResultColor(color);
            }
            yield return null; 
        }
        
    }

    public void SetSpeed(float sp)
    {
        this.speed = sp;
        //Debug.Log(speed);
    }
}
