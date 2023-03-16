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
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float horizontalVelocity = 5f;

    private bool jumpPressed = false;
    // private bool leftStopped = false;
    // private bool rightStopped = false;

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
        // if (leftStopped && dirX > 0f)
        //     leftStopped = false;
        // if (rightStopped && dirX < 0f)
        //     rightStopped = false;
            
        // if (leftStopped && dirX < 0f)
        //     dirX = 0;
        // else if (rightStopped && dirX > 0f)
        //     dirX = 0;

        if (player.velocity.x > horizontalVelocity)
            player.velocity = new Vector2(player.velocity.x - 0.01f, player.velocity.y);
        else if (player.velocity.x < -1 * horizontalVelocity)
            player.velocity = new Vector2(player.velocity.x + 0.01f, player.velocity.y);
        else
            player.velocity = new Vector2(dirX * horizontalVelocity, player.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // leftStopped = false;
            // rightStopped = false;

            float xVel = player.velocity.x;
            float yVel = jumpVelocity;
            if (xVel >= 4.5f || xVel <= -4.5f)
                xVel = xVel + (xVel / 2);
            else if (xVel <= 1 || xVel >= -1)
                yVel = jumpVelocity + 0.5f;
            player.velocity = new Vector2(xVel, yVel);
            jumpPressed = true;
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

        if (vel.y > 0.1f && jumpPressed)
        {
            state = MovementState.jumping;
        }
        else if (vel.y < -0.2f)
        {
            state = MovementState.falling;
            jumpPressed = false;
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

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Terrain" || other.tag == "Zombie")
    //     {
    //         player.velocity = new Vector2(0, player.velocity.y);
    //         // print(player.transform.position.x + ", " + other.transform.position.x);
    //         // if (player.transform.position.x > other.transform.position.x)
    //         //     rightStopped = true;
    //         // else if (player.transform.position.x < other.transform.position.x)
    //         //     leftStopped = true;
    //         if (player.velocity.x > 0)
    //             rightStopped = true;
    //         else if (player.velocity.x < 0)
    //             leftStopped = true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.tag == "Terrain" || other.tag == "Zombie")
    //     {
    //         rightStopped = false;
    //         leftStopped = false;
    //     }
    // }
    
    void OnCollisionEnter2D(Collision2D coll) 
    {
        // Collider2D collider = coll.collider;

        // if(collider.tag == "Terrain")
        // { 
        //     Vector3 contactPoint = coll.contacts[0].point;
        //     Vector3 center = collider.bounds.center;

        //     bool right = contactPoint.x > center.x;
        //     bool top = contactPoint.y > center.y;

        //     if (contactPoint.x > center.x || contactPoint.x < center.x)
        //     {
        //         print("collided");
        //         player.velocity = new Vector2(0, player.velocity.y);
        //     }
        // }

        // if (coll.gameObject.tag == "Terrain")
        // {
        //     if ((this.transform.position.x - coll.transform.position.x) < 0)
        //     {
        //         print("hit left" + this.transform.position.x + ", " + coll.collider.transform.position.x);
        //         player.velocity = new Vector2(0, player.velocity.y);
        //     }
        //     else if ((this.transform.position.x - coll.transform.position.x) > 0)
        //     {
        //         print("hit right" + this.transform.position.x + ", " + coll.collider.transform.position.x);
        //         player.velocity = new Vector2(0, player.velocity.y);
        //     }
        // }

        // if (coll.gameObject.tag == "Zombie")
        // {
        //     // print("zombie hit");
        // }
    }
}
