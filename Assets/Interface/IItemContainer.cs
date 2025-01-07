
public interface IItemContainer
{
    public ItemData GetSelectedItem();
    public void SetSelectedItem(ItemData itemData);
    public bool TryUseHere(ItemPlaceScript placeScript);
}
