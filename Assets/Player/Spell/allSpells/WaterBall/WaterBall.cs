using System.Collections;
using UnityEngine;

public class WaterBall : Spell
{
    private Rigidbody2D _rb;
    private CircleCollider2D _collider2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        ShootWaterBall();
        return base.CastSpell();
    }

    private void ShootWaterBall()
    {
        (Vector3 wPos, float wRot) = GetPositionAroundPlayerRelativeToMouse(); 
        
        GameObject waterBall = Instantiate(gameObject, wPos, Player.transform.rotation);
        waterBall.transform.localScale = new Vector3(0.4f + 0.6f * _power, 0.4f + 0.6f * _power, 0.4f + 0.6f * _power);
        waterBall.transform.RotateAround(wPos, Vector3.forward, wRot);
        waterBall.transform.SetParent(Player.transform);

        Animator animator = waterBall.GetComponent<Animator>();
        animator.enabled = true;
        waterBall.GetComponent<SpriteRenderer>().enabled = true;
        waterBall.GetComponent<WaterBall>().Dir = Dir;
        waterBall.GetComponent<WaterBall>().isMain = false;
        
        StartCoroutine(CastTime(0.5f, animator));
    }
    
    private IEnumerator CastTime(float time, Animator animator)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Fly", true);
    }
    
    private bool _isHolding = true;
    private float _power;
    private void FixedUpdate()
    {
        if (isMain)
        {
            return;
        }

        if (!Input.GetMouseButton(0) && _isHolding)
        {
            _isHolding = false;
            _collider2D.enabled = true;
        }

        if (transform.parent != SpellParent && _animator.GetBool("Fly"))
        {
            transform.parent = SpellParent;
        }
        
        if (_isHolding || !_animator.GetBool("Fly"))
        {
            (Vector3 bPos, float bRot) = GetPositionAroundPlayerRelativeToMouse(); 
    
            _spriteRenderer.flipY = bRot is < -90 or > 90 ;

            var trb = transform;
            trb.position = bPos;
            trb.eulerAngles = new Vector3(0, 0, bRot);
            
            if (_power < 1)
            {
                _power += 0.5f * Time.deltaTime;   
            }
            else
            {
                _power = 1;
            }
            
            trb.localScale = new Vector3(0.4f + 0.6f * _power, 0.4f + 0.6f * _power, 0.4f + 0.6f * _power);
            return;
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
            healthHandler.TakeDamage(spellData.spellDamage * _power, DamageType.Water, true);
        }
        bool flip = Dir.x < 0;

        var tr = transform;
        SpawnParticle(0.4f + 0.6f * _power, tr.position + new Vector3(Dir.x, Dir.y, 0)/2*_power, tr.rotation, flip);
        Destroy(gameObject);
    }
}