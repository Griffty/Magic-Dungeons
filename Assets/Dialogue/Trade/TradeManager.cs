using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TradeManager : MonoBehaviour, IDisplayable, IItemContainer
{
    [SerializeField] private GameObject tradeUI;
    private Player _player;
    private TradeObject _tradeObject;
    private ItemData _selectedItem;

    [SerializeField] private List<ItemPlaceScript> playerInv;
    [SerializeField] private List<ItemPlaceScript> traderInv;
    [SerializeField] private TextMeshProUGUI playerMoney;
    [SerializeField] private TextMeshProUGUI traderMoney;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemTitle;
    [SerializeField] private TextMeshProUGUI itemPricePlaceholder;

    [Space(30)]
    [Header("All items prices")]
    public List<ItemPrice> AllItemPrices;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public bool OpenDisplay()
    {
        if (tradeUI.activeInHierarchy)
        {
            return false;
        }
        
        tradeUI.SetActive(true);
        return true;
    }

    public bool CloseDisplay()
    {
        if (!tradeUI.activeInHierarchy)
        {
            return false;
        }
        tradeUI.SetActive(false);
        return true;
    }

    public void ShowTrade(TradeObject tradeObject)
    {
        _tradeObject = tradeObject;
        UpdateInventory();
        UpdateMoney();
        itemDescription.text = string.Empty;
        itemTitle.text = string.Empty;
        itemPricePlaceholder.text = 0+"";
    }

    private void UpdateInventory()
    {
        for (int i = 0; i < _player.playerInventoryHandler.itemPlaceHolders.Count; i++)
        {
            playerInv[i].ClearItem();
            playerInv[i].PutItem(_player.playerInventoryHandler.itemPlaceHolders[i].GetItem());
        }
        for (int i = 0; i < _tradeObject.itemData.Count; i++)
        {
            traderInv[i].ClearItem();
            traderInv[i].PutItem(_tradeObject.itemData[i]);
        }
    }

    private void UpdateMoney()
    {
        playerMoney.text = _player.playerInventoryHandler.GetMoney()+"";
        traderMoney.text = _tradeObject.money+"";
    }

    public ItemData GetSelectedItem()
    {
        return _selectedItem;
    }

    public void SetSelectedItem(ItemData itemData)
    {
        _selectedItem = itemData;
        UpdateDescriptionDisplay(_selectedItem);
    }

    private void UpdateDescriptionDisplay(ItemData itemData) // Add item price update
    {
        if (itemData == null)
        {
            itemTitle.text = "";
            itemDescription.text = "";
            itemPricePlaceholder.text = "";
            // statsText.text = "";
            // icon.color = new Color(0,0,0,0);
            // icon.sprite = null;
            return;
        }

        // if (itemData.GetType() == typeof(EquipmentData))
        // {
        //     statsText.text = GetStats((EquipmentData)itemData);
        // }
        itemTitle.text = itemData.itemName;
        itemDescription.text = itemData.itemDesc;
        GetItemPriceFromList(itemData, out ItemPrice price);
        itemPricePlaceholder.text = price.price+"";
        // icon.sprite = itemData.icon;
        // icon.color = Color.white;

    }

    public bool TryUseHere(ItemPlaceScript placeScript)
    {
        if (playerInv.Any(itemPlaceScript => itemPlaceScript == placeScript))
        {
            return TryToSell(placeScript);
        }
        if (traderInv.Any(itemPlaceScript => itemPlaceScript == placeScript))
        {
            return TryToBuy(placeScript);
        }

        return false;
    }

    private bool TryToBuy(ItemPlaceScript placeScript)
    {
        if (_player.playerInventoryHandler.GetMoney() >= GetItemPriceFromList(placeScript.GetItem(), out ItemPrice itemPrice))
        {
            if (_player.playerInventoryHandler.HasEmptySlot())
            {
                return BuyItem(placeScript, itemPrice);
            }
            Debug.Log("No free space");  
        }
        else
        {
            Debug.Log("Not enough money");  
        }
        return false;
    }

    private bool BuyItem(ItemPlaceScript placeScript, ItemPrice itemPrice)
    {
        _player.playerInventoryHandler.TakeMoney(itemPrice.price);
        if (!_player.playerInventoryHandler.AddItem(placeScript.GetItem()))
        {
            return false;
        }
        
        if (!itemPrice.infinite)
        {
            _tradeObject.itemData[_tradeObject.itemData.IndexOf(itemPrice.item)] = null;
        }
        
        _tradeObject.AddMoney(itemPrice.price);
        UpdateInventory();
        UpdateMoney();
        
        return true;
    }

    private bool TryToSell(ItemPlaceScript placeScript)
    {
        if (_tradeObject.money >= GetItemPriceFromList(placeScript.GetItem(), out ItemPrice itemPrice))
        {
            if (_tradeObject.HasEmptySlot())
            {
                return SellItem(placeScript, itemPrice);
            }
            Debug.Log("[Trader] No free space");
        }
        else
        {
            Debug.Log("[Trader] Not enough money");
        }
        
        return false;
    }

    private bool SellItem(ItemPlaceScript placeScript, ItemPrice itemPrice)
    {
        bool found = false;
        bool hasThisItem = false;
        if (itemPrice.infinite)
        {
            foreach (var t in _tradeObject.itemData)
            {
                if (t == placeScript.GetItem())
                {
                    hasThisItem = true;
                    found = true;
                }
                break;
            }
        }

        if (!hasThisItem)
        {
            for (int i = 0; i < _tradeObject.itemData.Count; i++)
            {
                if (_tradeObject.itemData[i] != null) continue;
                _tradeObject.itemData[i] = placeScript.GetItem();
                found = true;
                break;
            }
        }

        if (!found)
        {
            throw new Exception("WTF1");
        }
        
        found = false;
        foreach (var t in _player.playerInventoryHandler.itemPlaceHolders.Where(t => t.GetItem() == placeScript.GetItem()))
        {
            t.ClearItem();
            found = true;
            break;
        }
        if (!found)
        {
            throw new Exception("WTF2");
        }

        UpdateInventory();
        
        _tradeObject.TakeMoney(itemPrice.price);
        _player.playerInventoryHandler.GiveMoney(itemPrice.price);
        
        UpdateMoney();
        
        return true;
    }

    private int GetItemPriceFromList(ItemData itemData, out ItemPrice itemPrice)
    {
        foreach (var price in AllItemPrices)
        {
            if (price.item == itemData)
            {
                itemPrice = price;
                return price.price;
            }
        }

        throw new Exception("This item doesn't have price");
    }
    
}

[Serializable]
public class ItemPrice
{
    public ItemData item;
    public int price;
    public bool infinite;
}
