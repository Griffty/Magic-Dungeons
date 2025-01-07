
using System.Collections.Generic;
using UnityEngine;

public class DarkSkull : Spell
{
    private Rigidbody2D _rb;
    private CircleCollider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [SerializeField] private Transform target; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }

        if (!GetTarget(out Transform target))
        {
            return -2;
        }
        
        ShootDarkSkull(target);
        return base.CastSpell();
    }

    private void ShootDarkSkull(Transform target)
    {
        (Vector3 fPos, float fRot) = GetPositionAroundPlayerRelativeToMouse(); 
        
        GameObject darkSkull = Instantiate(gameObject, fPos, Player.transform.rotation);
        darkSkull.transform.RotateAround(fPos, Vector3.forward, fRot);
        darkSkull.GetComponent<Animator>().enabled = true;
        darkSkull.GetComponent<SpriteRenderer>().enabled = true;
        darkSkull.GetComponent<Collider2D>().enabled = true;
        darkSkull.GetComponent<DarkSkull>().isMain = false;
        darkSkull.GetComponent<DarkSkull>().target = target;
    }

    private bool GetTarget(out Transform tr)
    {
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        List<GameObject> activeEnemies = new List<GameObject>();

        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                activeEnemies.Add(enemy.gameObject);
            }
        }
        
        GameObject closestGameObject = null;
        float smallestDistance = Mathf.Infinity;
        Vector2 worldPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
        
        foreach (var activeEnemyGameObject in activeEnemies) {
            float distance = Vector2.Distance(activeEnemyGameObject.transform.position, worldPosition);
            if (distance < smallestDistance) {
                smallestDistance = distance;
                closestGameObject = activeEnemyGameObject;
            }
        }

        if (closestGameObject == null)
        {
            tr = transform;
            return false;
        }
        if(Vector2.Distance(closestGameObject.transform.position, worldPosition) > 1.5f)
        {
            tr = transform;
            return false;
        }

        tr = closestGameObject.transform;
        return true;
    }

    private void FixedUpdate()
    {
        if (isMain)
        {
            return;
        }
        
        float sRot = GetDirToTarget();
        _spriteRenderer.flipY = sRot is < -90 or > 90;
        transform.eulerAngles = new Vector3(0, 0, sRot);
        _rb.velocity = Dir.normalized * spellData.projectileSpeed;
    }

    private float GetDirToTarget()
    {
        if (!target)
        {
            return Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        }
        Vector2 relativeTargetPos = target.position - transform.position;
        Dir = relativeTargetPos / Mathf.Max(Mathf.Abs(relativeTargetPos.x), Mathf.Abs(relativeTargetPos.y));
        
        return Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            return;
        }
        if (col.TryGetComponent(out HealthHandler healthHandler))
        {
            healthHandler.TakeDamage(spellData.spellDamage, DamageType.Dark, true);
        }
        bool flip = Dir.x < 0;
        var tr = transform;
        SpawnParticle(1, tr.position + new Vector3(Dir.x, Dir.y, 0)/2, tr.rotation, flip);
        Destroy(gameObject);
    }
}
