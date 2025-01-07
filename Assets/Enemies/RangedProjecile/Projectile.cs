using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _collider2D;
    private Animator _animator;
    [SerializeField] private Vector2 dir;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    public void Shoot(GameObject pref, Vector2 pos, float rot, Vector2 dir, float damage, float speed)
    {
        GameObject projectile = Instantiate(pref, pos, Quaternion.Euler(0,0,rot));
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.dir = dir;
        projectileScript.damage = damage;
        projectileScript.speed = speed;
    }
    
    private void FixedUpdate()
    {
        _rb.velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<HealthHandler>().TakeDamage(damage, DamageType.Physic, false);
        }
        Destroy(gameObject);
    }
}
