using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Custom/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed;
    public float maxHealth;
    public float attackDelay;
    public float lookDist;
    public float attackCd;
    public float attackRange;
    public float damageOnCollision;
    public float damageOnAttack;
    public double pathUpdateInterval;
    public float updateNodeDist;
}