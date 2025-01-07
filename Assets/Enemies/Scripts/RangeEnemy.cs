using System.Collections;
using UnityEngine;

public class RangeEnemy: Enemy // make enemies scared of player ( if dist < 2 rebuild a path )
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    [SerializeField] private GameObject projectilePref;
    private void Start()
    {
        state = EnemyState.Patrolling;
        StartCoroutine(PrepareToAttack());
    }

    private IEnumerator PrepareToAttack()
    {
        yield return new WaitForSeconds(enemyData.attackCd);
        readyToAttack = true;
    }

    private void Update()
    {
        if (readyToAttack && CanSeeTarget(player.transform.position))
        {
            state = EnemyState.Attacking;
        }
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
                MoveToSpotNearPlayer();
                break;
        }
    }
    private void MoveToSpotNearPlayer()
    {
        if (_movement.hasPath && !IsCloseToPlayer())
        {
            _movement.MoveToTarget(1f);
        }
        else
        {
            _movement.MakePathToSpotNearPlayer();
        }
    }

    private bool _runningFrom;
    private bool IsCloseToPlayer()
    {
        if (_runningFrom)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 2f) return false;
            _runningFrom = false;
            return false;
        }
        if (Vector2.Distance(transform.position, player.transform.position) < 2f)
        {
            _runningFrom = true;
            return true;
        }
        return false;
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
        if (_isWaiting)
        {
            return; 
        }
        readyToAttack = false;
        StartCoroutine(MakeAttack());
    }

    private IEnumerator MakeAttack()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _isWaiting = true;
        _animator.SetBool(IsWalking, false);
        yield return new WaitForSeconds(enemyData.attackDelay);
        ShootProjectile();
        yield return new WaitForSeconds(enemyData.attackDelay*2);
        StartCoroutine(PrepareToAttack());
        _isWaiting = false;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = EnemyState.Moving;
    }

    private void ShootProjectile()
    {
        var position = transform.position;
        Vector2 dir = TransformUtil.GetDirFromPos(position, player.transform.position);
        float rot = TransformUtil.GetRotFromDir(dir);
        projectilePref.GetComponent<Projectile>().Shoot(projectilePref, (Vector2)(position) + dir, rot, dir, enemyData.damageOnAttack, 9);
    }
}
