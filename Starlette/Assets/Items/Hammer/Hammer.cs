using UnityEngine;
[CreateAssetMenu(menuName = "Items/Hammer")]

public class Hammer : Item
{
    public override void Use(){
        Debug.Log("this is Hammer");
    }
}
