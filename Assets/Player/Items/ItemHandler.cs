using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using Random = UnityEngine.Random;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemPref;
    [SerializeField] private GameObject moneyPref;
    public TilemapVisualizer vis;
 
    [SerializeField] private List<ItemData> allRegisteredItems = new();
    public static List<ItemData> AllRegisteredItems;
    
    private static GameObject _itemPref;
    private static GameObject _moneyPref;
    public static ItemHandler Self;

    private void Awake()
    {
        _itemPref = itemPref;
        _moneyPref = moneyPref;
        AllRegisteredItems = allRegisteredItems;
    }

    public static ItemData FindItemByName(string name)
    {
        foreach (var item in AllRegisteredItems)
        {
            if (item.itemName == name)
            {
                return item;
            }
        }

        return null;
    }
    public static ItemData FindItemByIndex(int i)
    {
        return i >= AllRegisteredItems.Count ? null : AllRegisteredItems[i];
    }
    

    public static void Drop(ItemData itemToDrop, Transform objectTransform)
    {
        GameObject item = Instantiate(_itemPref, objectTransform.position, objectTransform.rotation);
        Item itemScript = item.GetComponent<Item>();
        itemScript.itemData = itemToDrop;
        itemScript.circleCollider2D.radius = itemToDrop.pickUpRadius;
        itemScript.spriteRenderer.sprite = itemToDrop.icon;
    }

    public static void DropMoney(int moneyToDrop, Transform objectTransform, Room room)
    {
        int coinsAmount = Random.Range(1, 5);
        float averageAmount = (float)moneyToDrop / coinsAmount ;
        int[] money = new int[coinsAmount];
        while (coinsAmount > 1)
        {
            int a = (int)averageAmount;
            money[coinsAmount-1] = a;
            moneyToDrop -= a;
            coinsAmount--;
        }

        money[0] = moneyToDrop;
        HashSet<Vector2Int> usedPos = new();
        foreach (var i in money)
        {
            Vector3 pos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
            while (!room.FloorPos.Contains(new Vector2Int((int)(objectTransform.position.x+pos.x), (int)(objectTransform.position.y + pos.y))) || usedPos.Contains(new Vector2Int((int)pos.x, (int)pos.y)))
            {
                pos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
            }
            GameObject coin = Instantiate(_moneyPref, objectTransform.position + pos, objectTransform.rotation);
            usedPos.Add(new Vector2Int((int)pos.x, (int)pos.y));
            coin.GetComponent<MoneyScript>().moneyAmount = i;
        }
    }
}
