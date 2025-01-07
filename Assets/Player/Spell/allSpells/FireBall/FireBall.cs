using UnityEngine;
// using UnityEngine.Rendering.Universal;

public class FireBall : Spell
{
    private Rigidbody2D _rb;
    private CircleCollider2D _collider2D;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
    }
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        ShootFireBall();
        return base.CastSpell();
    }

    private void ShootFireBall()
    {
        (Vector3 fPos, float fRot) = GetPositionAroundPlayerRelativeToMouse(); 
        
        GameObject fireBall = Instantiate(gameObject, fPos, Player.transform.rotation);
        fireBall.transform.RotateAround(fPos, Vector3.forward, fRot);
        
        // fireBall.GetComponent<Light2D>().enabled = true;
        fireBall.GetComponent<Animator>().enabled = true;
        fireBall.GetComponent<SpriteRenderer>().enabled = true;
        fireBall.GetComponent<Collider2D>().enabled = true;
        fireBall.GetComponent<FireBall>().Dir = Dir;
        fireBall.GetComponent<FireBall>().isMain = false;
    }
    private void FixedUpdate()
    {
        if (isMain)
        {
            return;
        }
        _rb.velocity = Dir.normalized * spellData.projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {            
        if (col.CompareTag("Player") || col.isTrigger)
        {
            return;
        }
        if (col.TryGetComponent(out HealthHandler healthHandler))
        {
            healthHandler.TakeDamage(spellData.spellDamage, DamageType.Fire, true);
        }
        bool flip = Dir.x < 0;
        var tr = transform;
        SpawnParticle(1, tr.position + new Vector3(Dir.x, Dir.y, 0)/2, tr.rotation, flip);
        Destroy(gameObject);
    }
}