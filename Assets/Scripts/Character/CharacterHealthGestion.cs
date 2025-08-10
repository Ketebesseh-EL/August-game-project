using UnityEngine;

public class CharacterHealthGestion : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float regeneration;
    public float timeBeforeRegeneration;
    public float regenerationSpeed;
    private float nextTimeToRegen;
    [HideInInspector] public bool isAlive = true;
    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckAlivenes();
    }
    private void Start()
    {
        nextTimeToRegen = timeBeforeRegeneration;
    }
    private void CheckAlivenes()
    {
        if (health <= 0.0f)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }
    }
    private void tryToRegenerate()
    {
        if (Time.time >= nextTimeToRegen)
        {
            health += regeneration;
            nextTimeToRegen = Time.time + (1 / regenerationSpeed);
        }
    }


    private void FixedUpdate()
    {
        tryToRegenerate();
    }
}
