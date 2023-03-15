using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator playerAnim;
    private SpriteRenderer playerSprite;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float horizontalVelocity = 10f;

    private enum MovementState { idle, running, jumping, falling };

    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        // playerAnim.SetBool("entry", true);
    }

    private void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(dirX * horizontalVelocity, player.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpVelocity);
        }
        UpdateAnimation(dirX);
    }

    private void UpdateAnimation(float dirX)
    {
        Vector2 vel = player.velocity;

        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            playerSprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            playerSprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (vel.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (vel.y < -0.1f)
        {
            state = MovementState.falling;
        }

        playerAnim.SetInteger("state", (int)state);
    }

    // private void FinishEntry()
    // {
    //     playerAnim.SetBool("entry", false);
    //     player.bodyType = RigidbodyType2D.Dynamic;
    // }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}
