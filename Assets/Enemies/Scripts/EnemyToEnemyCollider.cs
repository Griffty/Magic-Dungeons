using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToEnemyCollider : MonoBehaviour
{
    private readonly HashSet<Enemy> _enemies = new ();
    [SerializeField] private float repulsion;
    [SerializeField] private Vector2 dir;
    [SerializeField] private Vector2 force;
    [SerializeField] private float power;
    
    private void FixedUpdate()
    {
        foreach (var enemy in _enemies)
        {
            Vector2 pos = transform.position;
            Vector2 ePos = enemy.transform.position;
            dir = TransformUtil.GetDirFromPos(new Vector2(pos.x, pos.y), ePos);
            power = Mathf.Pow(0.5f/Vector2.Distance(pos, ePos), 4);
            force = power * repulsion * dir;
            enemy._rigidbody2D.velocity += force;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                _enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }
}
