using UnityEngine;
using UnityEngine.UI; 

public class OxygenBar : MonoBehaviour
{
    public Image oxygenImage; 

    void Start()
    {
        if (oxygenImage != null)
        {
            oxygenImage.fillAmount = 1f; 
        }
    }

    public void HandleUpdate()
    {
        if (oxygenImage != null)
        {
            oxygenImage.fillAmount -= 0.01f * Time.deltaTime;

           
        }
    }
}
