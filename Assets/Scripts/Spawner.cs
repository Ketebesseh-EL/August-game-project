using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy enemy;
    public float sightRange;

    private bool playerInSightRange;
    private bool hasSpawned;
    private bool isEnemyAlive;
    public float timeBeforeRespawn;
    private Enemy spawnedEnemy;
    public bool showSightRange;
    public float maxTimeOutOfRange; // time that need to be elapsed before the enemy dispawn 
    private float timeOutOfRange;
    private bool isRespawning;

    private void Spawn()
    {
        spawnedEnemy = Instantiate(enemy, transform.position, transform.rotation);
        spawnedEnemy.transform.localScale = new Vector3(5, 5, 5);
        isEnemyAlive = true;
        hasSpawned = true;
    }
    private void Destroy()
    {
        spawnedEnemy.DestroyEnnemy();
        isEnemyAlive = false;
    }
    private IEnumerator Respawn()
    {
        isRespawning = true;
        yield return new WaitForSeconds(timeBeforeRespawn);

        Spawn();
        isRespawning = false;
    }
    private void FixedUpdate()
    {
        if (spawnedEnemy == null) isEnemyAlive = false;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, enemy.Player);
        if (playerInSightRange) timeOutOfRange = 0.0f;
        if (playerInSightRange && !hasSpawned) Spawn();
        if (playerInSightRange && !isEnemyAlive && !isRespawning) StartCoroutine(Respawn());
        if (!playerInSightRange && isEnemyAlive)
        {
            timeOutOfRange += Time.deltaTime;
        }
        if (!playerInSightRange && isEnemyAlive && timeOutOfRange >= maxTimeOutOfRange)
        {
            Destroy();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (showSightRange)
        {  
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}
