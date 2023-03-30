using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item Collections/Plain")]
public class PlainItemCollectionSO : ScriptableObject
{
    public List<ItemSO> items;
}
