using System;
using UnityEngine;

public abstract class Enemy: MonoBehaviour, IDestroyable
{
    public Room room;
    public EnemyData enemyData;
    public HealthHandler healthHandler;
    public EnemyType enemyType;
    public EnemyStyle enemyStyle;

    public Rigidbody2D _rigidbody2D;
    protected internal Animator _animator;
    protected internal SpriteRenderer _spriteRenderer;
    protected EnemyMovement _movement;

    protected EnemyState state;
    protected Player player;
    
    protected bool _isWaiting;
    protected bool readyToAttack;

    public delegate void OnDeath(EnemyType type, EnemyStyle style);

    public static event OnDeath OnThisEnemyDeath;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        healthHandler = GetComponent<HealthHandler>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
    }

    internal int EnemyDanger { get; set; }

    protected bool CanSeeTarget(Vector2 target)
    {
        var pos = transform.position;

        Vector2 relativeTargetPos = target - new Vector2(pos.x, pos.y);
        Vector2 dir = relativeTargetPos / Mathf.Max(Mathf.Abs(relativeTargetPos.x), Mathf.Abs(relativeTargetPos.y));
        
        LayerMask layerMask = LayerMask.GetMask("Player", "Default");
        RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(pos.x, pos.y), dir, enemyData.lookDist, layerMask);
        if (!hit2D)
        {
            return false;
        }
        return hit2D.collider.CompareTag("Player");
    }

    public void MakeRare()
    {
        throw new NotImplementedException();
    }

    public void At0Hp()
    {
        OnThisEnemyDeath?.Invoke(enemyType, enemyStyle);
        room.ActiveEnemies.Remove(this);
        Destroy(gameObject);
    }
}

public enum EnemyState
{
    Deactivated,
    Patrolling,
    Moving,
    Attacking,
}
