using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public Rigidbody rigidBody;

    public float moveSpeed;
    private Vector2 direction;
    public InputActionReference move;
    [Header("Player Health Gestion")]
    public float maxHealth;
    [HideInInspector] public float currentHealth;
    public float regenerationValue;
    public float timeBeforeRegeneration;
    public float regenerationSpeed;
    public bool regeneration; // is regeneration allowed

    private float nextTimeToRegen;
    [HideInInspector] public bool isAlive = true;

    private void Update()
    {
        direction = move.action.ReadValue<Vector2>(); // Get player movement Inputs 
    }

    private void FixedUpdate()
    {

        rigidBody.linearVelocity = new Vector3(direction.x * moveSpeed, 0, direction.y * moveSpeed); // Player Movement
        if (regeneration) tryToRegenerate(); 


    }

    private void Start()
    {
        nextTimeToRegen = timeBeforeRegeneration;
        currentHealth = maxHealth;
    }

    private void CheckAlivenes()
    {
        if (currentHealth <= 0.0f)
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
        if (currentHealth < maxHealth && Time.time >= nextTimeToRegen)
        {
            currentHealth = Mathf.Min(currentHealth + regenerationValue, maxHealth);
            nextTimeToRegen = Time.time + (1 / regenerationSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        nextTimeToRegen = Time.time + timeBeforeRegeneration; // reset time before regenration is enabled
        CheckAlivenes();
    }

}
