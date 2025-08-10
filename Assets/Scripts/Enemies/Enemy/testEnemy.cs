using Unity.VisualScripting;
using UnityEngine;

public class testEnemy : Enemy
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        playerInSightRange = Physics.CheckSphere(transform.position, enemyData.sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, enemyData.attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    public override void Attack()
    {
        Player playerHealth = player.GetComponent<Player>();
        playerHealth.TakeDamage(enemyData.damage);
        Debug.Log("player health" + playerHealth.currentHealth);
    }
}
