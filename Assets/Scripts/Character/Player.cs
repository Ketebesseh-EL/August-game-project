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
    public float health;
    public float regeneration;
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

        tryToRegenerate();
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckAlivenes();
    }

}
