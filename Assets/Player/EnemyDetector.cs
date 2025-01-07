using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private Collider2D _collider2D;
    private Player _player;
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        foreach (var col in _colliders)
        {
            _player.healthHandler.TakeDamage(col.gameObject.GetComponent<Enemy>().enemyData.damageOnCollision, DamageType.Physic, false);
        }
    }

    private readonly HashSet<Collider2D> _colliders = new ();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                _colliders.Add(col);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _colliders.Remove(other);
            }
        }
    }
}
