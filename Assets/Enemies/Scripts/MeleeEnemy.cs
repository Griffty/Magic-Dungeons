using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy: Enemy
{
    private static readonly int Walking = Animator.StringToHash("isWalking");

    private void Start()
    {
        state = EnemyState.Patrolling;
        readyToAttack = true;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Deactivated:
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                _rigidbody2D.velocity = Vector2.zero;
                break;
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Moving:
                MoveToPlayer();
                break;
        }
    }

    private void FixedUpdate()
    {
        var rigidbody2DVelocity = _rigidbody2D.velocity;
        if (rigidbody2DVelocity.x > 25)
        {
            rigidbody2DVelocity.x = 25;
        }
        if (rigidbody2DVelocity.y > 25)
        {
            rigidbody2DVelocity.y = 25;
        }

        _rigidbody2D.velocity = rigidbody2DVelocity;
    }

    private void MoveToPlayer()
    {
        if (_movement.playerInRangeOfAttack)
        {
            state = EnemyState.Attacking;
            return;
        }
        _movement.MoveToPlayer();
    }

    private void Patrol()
    {
        if (CanSeeTarget(player.transform.position))
        {
            state = EnemyState.Moving;
            _movement.hasPath = false;
            return;
        }
        if (_movement.hasPath)
        {
            _movement.MoveToTarget(0.3f);
        }
        else
        {
            _movement.MakePathToRandomSpot();
        }
    }
    
    private void Attack()
    {
        if (_isWaiting || !readyToAttack)
        {
            return; 
        }

        _rigidbody2D.velocity = Vector2.zero;
        StartCoroutine(MakeAttack());
    }

    private IEnumerator MakeAttack()
    {
        _isWaiting = true;
        _animator.SetBool(Walking, false);
        yield return new WaitForSeconds(enemyData.attackDelay);
        _animator.SetBool(Walking, true);
        _movement.SetConstantVelocityTowardsTarget(2.5f);
        yield return new WaitForSeconds(enemyData.attackDelay*3/5);
        _animator.SetBool(Walking, false);
        _movement.SetConstantVelocityTowardsTarget(0);
        yield return new WaitForSeconds(enemyData.attackDelay*2);
        _isWaiting = false;
        state = EnemyState.Moving;
    }
}