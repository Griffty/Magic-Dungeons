using System;
using UnityEngine;

[Serializable]
public class GiveQuest : Quest
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int money;

    public GiveQuest(int index, string name, string desc, string obj, int amount, bool talkToComplete, ItemData item) : base(index, name, desc, obj, amount, talkToComplete)
    {
        if (item == null)
        {
            throw new Exception("Cant make this quest | INDEX: " + Index);
        }
        itemData = item;
    }
    
    public GiveQuest(int index, string name, string desc, string obj, int amount, bool talkToComplete, int money) : base(index, name, desc, obj, amount, talkToComplete)
    {
        if (money == 0)
        {
            throw new Exception("Cant make this quest | INDEX: " + Index);
        }
        this.money = money;
    }
    public override void OnAdd()
    {
        Item.OnThisItemPickUp += OnItemPickUp;
    }

    private void OnItemPickUp(ItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData != this.itemData)
            {
                currentAmount++;
            } 
        }
        
        BaseOnAction();
    }
}
