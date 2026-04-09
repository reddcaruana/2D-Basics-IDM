using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerControls : MonoBehaviour
{
    [Header("Controls")]
    public float speed = 5f;
    public float jumpForce = 10f;
    
    [Header("Checks")]
    public Transform groundCheck;
    
    private Rigidbody2D myRigidbody;
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMove(InputValue value)
    {
        // Converts the input to
        // X and Y (Up/down and left/right)
        Vector2 input = value.Get<Vector2>();

        if (input.x != 0)
        {
            spriteRenderer.flipX = input.x < 0;
        }
        
        // move horizontally
        myRigidbody.linearVelocityX = input.x * speed;
    }

    private void OnJump(InputValue value)
    {
        // Only jump when the key is pressed
        if (value.isPressed && IsGrounded())
        {
            myRigidbody.linearVelocityY = jumpForce;
        }
    }
    
    // Will check for collisions with the ground
    private bool IsGrounded()
    {
        Vector2 size = new Vector2(0.2f, 0.1f);
        return Physics2D.OverlapBox(
            groundCheck.position,
            size,
            0);
    }
}
