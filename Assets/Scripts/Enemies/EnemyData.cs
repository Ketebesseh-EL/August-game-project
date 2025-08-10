using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    [Header("Enemy Stats")]
    public float health;
    public float damage;
    public float sightRange;
    public float attackRange;
    public float walkPointRange;
    public float timeBetweenAttacks;

}
