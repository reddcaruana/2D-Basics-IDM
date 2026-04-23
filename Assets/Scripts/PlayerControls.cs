using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerControls : MonoBehaviour
{
    [Header("Controls")]
    public float speed = 5f;
    public float jumpForce = 10f;
    
    [Header("Checks")]
    public Transform groundCheck;

    [Header("References")]
    public ParticleSystem sandParticles;
    
    [Header("Sounds")]
    public AudioClip[] jumpSound;
    
    private int jumpsLeft = 2;

    private AudioSource myAudioSource;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // Convert the velocity to a + number
        float velocityX = Mathf.Abs(myRigidbody.linearVelocityX);
        myAnimator.SetFloat("Velocity X", velocityX);
        myAnimator.SetFloat("Velocity Y", myRigidbody.linearVelocityY);
        
        myAnimator.SetBool("Grounded", IsGrounded());
        myAnimator.SetInteger("Jumps Left", jumpsLeft);
        
        // If grounded & particles are stopped
        // start particles
        // If not grounded & particles playing
        // stop particles

        if (IsGrounded() && !sandParticles.isPlaying)
        {
            sandParticles.Play();
        }
        
        if (!IsGrounded() && sandParticles.isPlaying)
        {
            sandParticles.Stop();
        }
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
        if (IsGrounded())
        {
            jumpsLeft = 2;
        }
        
        // Only jump when the key is pressed
        if (value.isPressed && jumpsLeft > 0)
        {
            myRigidbody.linearVelocityY = jumpForce;
            jumpsLeft--;
            
            // Find a random index in the array
            int idx = Random.Range(0, jumpSound.Length);
            myAudioSource.PlayOneShot(jumpSound[idx]);
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
