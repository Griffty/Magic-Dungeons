using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public bool hasPath;
    private bool _processing;
    
    private Enemy _enemy;
    private Seeker _seeker;
    private Player _player;
    
    private Path _path;
    [SerializeField] private int pathCount;
    private Vector2 _currentNode;

    [SerializeField] private Vector2 dirToNode;
    [SerializeField] private float distToNode;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    public bool playerInRangeOfAttack;
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _seeker = GetComponent<Seeker>();
        _player = FindObjectOfType<Player>();
    }
    private float _lastPathUpdateTime;
    private void Update()
    {
        playerInRangeOfAttack = Vector2.Distance(transform.position, _player.transform.position) < _enemy.enemyData.attackRange;
        if (_enemy.enemyType == EnemyType.Melee)
        {
            if (Time.time - _lastPathUpdateTime > _enemy.enemyData.pathUpdateInterval)
            {
                _lastPathUpdateTime = Time.time;
                MakePathToPlayer();
            }
        }
        else if(_enemy.enemyType == EnemyType.Range)
        {
            if (!hasPath)
            {
                if (Time.time - _lastPathUpdateTime > _enemy.enemyData.pathUpdateInterval)
                {
                    _lastPathUpdateTime = Time.time;
                    MakePathToRandomSpot();
                }
            }
        }
    }

    public void MakePathToRandomSpot()
    {
        if (!_processing)
        {
            Tilemap t = new Tilemap();
            
            _processing = true;
            var pos = transform.position;
            Vector3 newPos = TransformUtil.GetRandomPosAroundTarget(0, 10, pos, _enemy.room.FloorPos);
            _seeker.StartPath(pos, newPos, OnPathComplete);
        }
    }

    public void MakePathToSpotNearPlayer()
    {
        if (!_processing)
        {
            _processing = true;
            Vector3 newPos = TransformUtil.GetRandomPosAroundTarget(4, 8, _player.transform.position, _enemy.room.FloorPos);
            _seeker.StartPath(transform.position, newPos, OnPathComplete);
        }
    }
    
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            pathCount = 1;
            _currentNode = (Vector3)_path.path[pathCount].position;
            FindDistAndDirToNode();
        }
        _processing = false;
        hasPath = true;
    }

    private void FindDistAndDirToNode()
    {
        var pos = transform.position;
        dirToNode = TransformUtil.GetDirFromPos(pos, _currentNode);
        distToNode = Vector2.Distance(_currentNode, pos);
    }

    public void MoveToTarget(float speedMult)
    {
        FindDistAndDirToNode();
        if (distToNode < _enemy.enemyData.updateNodeDist)
        {
            SetNextNodeAsTarget();
        }
        
        Move(speedMult, dirToNode);
    }

    private void SetNextNodeAsTarget()
    {
        pathCount++;
        try
        {
            if (pathCount >= _path.path.Count-2)
            {
                hasPath = false;
            }
        
            _currentNode = (Vector3)_path.path[pathCount].position;
        }
        catch (Exception)
        {
            hasPath = false;
        }
        
        FindDistAndDirToNode();
    }

    private void Move(float speedMult, Vector2 dir)
    {
        if (dir.x == 0)
        {
            _enemy._spriteRenderer.flipX = Vector2.Distance(transform.position, _path.vectorPath[^1]) < 0;
        }
        else
        {
            _enemy._spriteRenderer.flipX = dir.x < 0;
        }
        _enemy._animator.SetBool(IsWalking, true);
        _enemy._rigidbody2D.velocity = dir.normalized * (_enemy.enemyData.moveSpeed * speedMult);
    }

    private void MakePathToPlayer()
    {
        _seeker.StartPath(transform.position, _player.transform.position, OnPathToPlayerComplete);
    }
    
    private void OnPathToPlayerComplete(Path p)
    {
        _path = p;
        pathCount = _path.path.Count > 0 ? 1 : 0;
        _currentNode = (Vector3)_path.path[pathCount].position;
        FindDistAndDirToNode();
        hasPath = true;
        _processing = false;
    }
    
    public void SetConstantVelocityTowardsTarget(float i)
    {
        _enemy._rigidbody2D.velocity = dirToNode.normalized * (_enemy.enemyData.moveSpeed * i);
    }
    
    public void MoveToPlayer()
    {
        if (!hasPath)
        {
            MakePathToPlayer();
        }
        MoveToTarget(1);
    }
}
