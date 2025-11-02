using UnityEngine;
[CreateAssetMenu(menuName = "Items/Tablet")]
public class Tablet : Item
{
    public override void Use()
    {
        GameController gameController = FindObjectOfType<GameController>();

        gameController.SetState("OnTablet");

        GameObject moka = GameObject.Find("Moka");
        moka.transform.GetChild(0).gameObject.SetActive(true);
    }
}
