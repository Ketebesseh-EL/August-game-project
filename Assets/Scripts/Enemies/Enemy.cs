using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Ground, Player;

    [HideInInspector] public Vector3 walkPoint;
    private bool walkPointSet;
    
    private bool alreadyAttacked;
    [HideInInspector] public bool playerInSightRange, playerInAttackRange;
    public bool showRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (enemyData.sightRange < enemyData.attackRange)
            Debug.LogError("Warning : sightRange < attackRange");

    }

    public virtual void FixedUpdate(){}
    

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            Rotate(walkPoint);
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }
    private void Rotate(Vector3 target)
    {
        Vector3 vec = target - transform.position;
        float rotationSpeed = 2*Mathf.PI; // Rotation speed before going to next walk point
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, vec, rotationSpeed * Time.deltaTime, 0.0f));
    }
    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);
        float randomZ = Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
            walkPointSet = true;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            
            Attack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemyData.timeBetweenAttacks);
        }
    }


    public abstract void Attack();


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        enemyData.health -= damage;
        if (enemyData.health <= 0)
            Invoke(nameof(DestroyEnnemy), 0.5f);
    }

    public void DestroyEnnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (showRange)
        {  
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyData.sightRange);  
        }
    }
}
