using System;
using UnityEngine;

public class ItemPlaceScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private ItemData itemData;
    public GameObject manager;
    private IItemContainer _itemContainer;
    public PlaceHolderType placeHolderType;

    private void Awake()
    {
        _itemContainer = manager.GetComponent<IItemContainer>();
    }

    public bool PutItem(ItemData itemData)
    {
        if (this.itemData != null)
        {
            return false;
        }

        if (placeHolderType != PlaceHolderType.Any)
        {
            if (itemData.itemType != placeHolderType)
            {
                return false;
            }
        }

        this.itemData = itemData;
        UpdateImage();
        return true;
    }

    public ItemData GetItem()
    {
        return itemData;
    }
    public bool ClearItem()
    {
        if (itemData == null)
        {
            return false;
        }

        itemData = null;
        UpdateImage();
        return true;
    }

    private void UpdateImage(Sprite sprite)
    {
        image.color = sprite == null ? new Color(image.color.r, image.color.g, image.color.b, 0) : new Color(image.color.r, image.color.g, image.color.b, 1);
        image.sprite = sprite;
    }
    
    public void UpdateImage()
    {
        image.color = itemData == null ? new Color(image.color.r, image.color.g, image.color.b, 0) : new Color(image.color.r, image.color.g, image.color.b, 1);
        if (itemData != null)
        {
            image.sprite = itemData.icon;   
        }
    }
    
    private readonly DoubleClick _doubleClick = new(0.7f);
    
    public void OnItemSelected()
    {
        if (_itemContainer.GetSelectedItem() != itemData)
        {
            _itemContainer.SetSelectedItem(itemData);
        }

        if (itemData == null) return;
        if (placeHolderType == PlaceHolderType.Any)
        {
            if (_doubleClick.Click())
            {
                if (!_itemContainer.TryUseHere(this))
                {
                    Debug.Log("This item cannot be used here");
                }
            }   
        }
        else
        {
            if (_doubleClick.Click())
            {
                if (!_itemContainer.TryUseHere(this))
                {
                    Debug.Log("This item cannot be used here");
                }
            }   
        }
    }

    private void OnItemSelectedTrade()
    {
        
    }

    private void OnItemSelectedInventory()
    {

    }

    public bool IsEmpty()
    {
        return itemData == null;
    }
}

public enum PlaceHolderType
{
    Any,
    Helmet,
    ChestPlate,
    Boots,
    Wand,
}
