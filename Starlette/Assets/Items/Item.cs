using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

public  class Item : ScriptableObject
{


    [Header("Both")]
    public Sprite image;

    public virtual void Use(){

    }
}

