using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider2D;
    public Rigidbody2D rigidBody2D;
    
    public delegate void OnPickUp(ItemData itemData);

    public static event OnPickUp OnThisItemPickUp;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.TryGetComponent(out PlayerInventoryHandler handler))
            {
                if (handler.AddItem(itemData))
                {
                    OnThisItemPickUp?.Invoke(itemData);
                    Destroy(gameObject);
                }
            }
        }
    }
}