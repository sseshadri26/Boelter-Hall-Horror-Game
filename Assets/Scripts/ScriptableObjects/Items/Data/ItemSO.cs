using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class ItemSO : ScriptableObject
{
    public string itemName;

    [TextArea(3, 10)]
    public string description;
    public Sprite graphic;

}
