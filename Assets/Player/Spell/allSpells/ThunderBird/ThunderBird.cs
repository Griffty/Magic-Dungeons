using System.Collections;
using UnityEngine;

public class ThunderBird : Spell
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        ShootThunderBird();
        return base.CastSpell();
    }

    private void ShootThunderBird()
    {

        (Vector3 bPos, float bRot) = GetPositionAroundPlayerRelativeToMouse(); 
        GameObject thunderBird = Instantiate(gameObject, bPos, Player.transform.rotation);
        
        thunderBird.transform.RotateAround(bPos, Vector3.forward, bRot);
        thunderBird.transform.SetParent(Player.transform);

        Animator animator = thunderBird.GetComponent<Animator>();
        animator.enabled = true;
        
        
        thunderBird.GetComponent<SpriteRenderer>().enabled = true;
        thunderBird.GetComponent<ThunderBird>().Dir = Dir;
        thunderBird.GetComponent<ThunderBird>().isMain = false;
        
        StartCoroutine(CastTime(0.5f, animator, thunderBird.GetComponent<CircleCollider2D>()));
    }

    private IEnumerator CastTime(float time, Animator animator, Collider2D collider2D)
    {
        yield return new WaitForSeconds(time);
        collider2D.enabled = true;
        animator.SetBool("Fly", true);
        animator.gameObject.transform.SetParent(SpellParent);
    }
    private void FixedUpdate()
    {
        if (isMain)
        {
            return;
        }
        
        if (!_animator.GetBool("Fly"))
        {
            (Vector3 bPos, float bRot) = GetPositionAroundPlayerRelativeToMouse(); 

            _spriteRenderer.flipY = bRot is < -90 or > 90 ;

            var tr = transform;
            tr.position = bPos;
            tr.eulerAngles = new Vector3(0, 0, bRot);
        }
        
        if (_animator.GetBool("Fly"))
        {
            _rb.velocity = Dir.normalized * spellData.projectileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.isTrigger)
        {
            return;
        }
        
        if (col.TryGetComponent(out HealthHandler healthHandler))
        {
            healthHandler.TakeDamage(spellData.spellDamage, DamageType.Lightning, true);
        }
        
        bool flip = Dir.x < 0;
        var tr = transform;
        SpawnParticle(1, tr.position + new Vector3(Dir.x, Dir.y, 0)/2, tr.rotation, flip);
        if (!col.CompareTag("Enemy"))
        {
            Destroy(gameObject); 
        }
    }
}
