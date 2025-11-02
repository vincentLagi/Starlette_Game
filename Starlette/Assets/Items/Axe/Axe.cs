using UnityEngine;
[CreateAssetMenu(menuName = "Items/Axe")]
public class AxeSO : Item
{
    public override void Use(){
        Debug.Log("this is Axe");
    }
}
