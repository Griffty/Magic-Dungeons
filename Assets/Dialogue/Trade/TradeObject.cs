
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/tradeObject")]
public class TradeObject: ScriptableObject
{
    public List<ItemData> itemData = new(8);
    public int money;

    public void AddMoney(int amount)
    {
        money += amount;
    }
    
    public void TakeMoney(int amount)
    {
        money -= amount;
    }

    public bool HasEmptySlot()
    {
        return itemData.Any(data => data == null);
    }
}
