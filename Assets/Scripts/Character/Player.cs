using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    public Rigidbody rigidBody;

    public float moveSpeed;
    private Vector2 direction;
    public InputActionReference move;
    [Header("Player Combat Gestion")]
    public float maxHealth;
    [HideInInspector] public float currentHealth;
    public bool regeneration; // is regeneration allowed
    public float regenerationValue;
    public float timeBeforeRegeneration;
    public float regenerationSpeed;
    private float nextTimeToRegen;
    [HideInInspector] public bool isAlive = true;

    private void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {

        rigidBody.linearVelocity = new Vector3(direction.x * moveSpeed, 0, direction.y * moveSpeed);
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
        CheckAlivenes();
    }

}
