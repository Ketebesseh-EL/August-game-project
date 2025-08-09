using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovements : MonoBehaviour
{

    public Rigidbody rigidBody;

    public float moveSpeed;
    private Vector2 direction;

    public InputActionReference move;


    private void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {

        rigidBody.linearVelocity = new Vector3(direction.x * moveSpeed, 0, direction.y * moveSpeed);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


}
