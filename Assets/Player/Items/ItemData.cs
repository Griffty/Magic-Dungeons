
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "BasicItem", menuName = "Custom/ItemData")]
public class ItemData: ScriptableObject
{
    public Sprite icon;
    public string itemName;
    [TextArea] public string itemDesc;
    public float pickUpRadius;
    public PlaceHolderType itemType;


    protected bool Equals(ItemData other)
    {
        return base.Equals(other) && itemName == other.itemName && itemDesc == other.itemDesc && itemType == other.itemType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ItemData)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), icon, itemName, itemDesc, pickUpRadius, (int)itemType);
    }
}