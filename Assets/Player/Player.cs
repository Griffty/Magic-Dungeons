using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerMagicHandler playerMagicHandler;
    public PlayerInventoryHandler playerInventoryHandler;
    public PlayerQuestManager playerQuestManager;
    public PlayerEventHandler playerEventHandler;
    public MovementHandler movementHandler;
    public HealthHandler healthHandler;
    public PlayerData playerData;
    
    
    private Vector2 _movement;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMagicHandler = GetComponent<PlayerMagicHandler>();
        healthHandler = GetComponent<HealthHandler>();
        playerInventoryHandler = GetComponent<PlayerInventoryHandler>();
        playerEventHandler = GetComponent<PlayerEventHandler>();
        playerQuestManager = GetComponent<PlayerQuestManager>();
        movementHandler = GetComponent<MovementHandler>();
    }

    private void Start()
    {
        healthHandler.SetMaxHealth(playerData.maxHealth, true);
    }
}




