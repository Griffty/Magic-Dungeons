using UnityEngine;

public class Destroyable : MonoBehaviour, IDestroyable
{
    public HealthHandler healthHandler;
    private static ItemHandler _itemHandler;
    public Room Room;
    public ItemData itemToDrop;
    public int moneyToDrop;

    private void Start()
    {
        if(Room == null) Debug.Log("We fucked room Up" + transform.position);
        _itemHandler = ItemHandler.Self;
    }

    private void Awake()
    {
        healthHandler = GetComponent<HealthHandler>();
    }

    public void At0Hp()
    {
        if (itemToDrop != null)
        {
            ItemHandler.Drop(itemToDrop, transform);   
        }

        if (moneyToDrop > 1)
        {
            ItemHandler.DropMoney(moneyToDrop, transform, Room);
        }
        Destroy(gameObject);
    }
}