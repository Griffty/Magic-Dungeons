
using System;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    private Collider2D _collider2D;
    public int moneyAmount;
    
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.TryGetComponent(out PlayerInventoryHandler inventoryHandler))
            {
                inventoryHandler.GiveMoney(moneyAmount);
                Destroy(gameObject);
            }
        }
    }
}