using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator playerAnim;
    private SpriteRenderer playerSprite;
    private CapsuleCollider2D playerCollider;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float horizontalVelocity = 10f;

    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private float attackRange = 0.5f;
    public int attackDamage = 40;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRate = 1f;
    private float nextAttackTime = 0f;
    private Transform activeAttackPoint;

    private enum MovementState { idle, running, jumping, falling };

    [SerializeField] AudioSource audioData;

    [SerializeField] private SoundManagerScript soundManager;


    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        audioData = GetComponent<AudioSource>();
        activeAttackPoint = attackPointLeft;
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                playerAnim.SetTrigger("attack");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, attackRange, playerLayer);
                bool didHit = false;

                foreach (Collider2D enemy in hitEnemies) {
                    if (enemy.name != "Player") {
                        didHit = true;
                    }
                }
                if (didHit) {
                    soundManager.playSound("player_hit");
                } else {
                    soundManager.playSound("player_swing");
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        float dirX = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(dirX * horizontalVelocity, player.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpVelocity);
            soundManager.playSound("player_jump");
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
            activeAttackPoint = attackPointRight;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            playerSprite.flipX = true;
            activeAttackPoint = attackPointLeft;
        }
        else
        {
            state = MovementState.idle;
        }

        if (vel.y > 0f && !IsGrounded())
        {
            state = MovementState.jumping;
        }
        else if (vel.y < 0f && !IsGrounded())
        {
            state = MovementState.falling;
        }

        playerAnim.SetInteger("state", (int)state);
    }

    private void Attack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, attackRange, playerLayer);
        foreach (Collider2D enemy in hitEnemies) {
            Debug.Log("Player is Hitting: "+ enemy.name);
            if (enemy.tag == "FrenziedWorker")
                enemy.GetComponent<FrenzyLife>().TakeDamage(attackDamage);
            else if (enemy.name == "Foreman")
                enemy.GetComponent<Boss1Life>().TakeDamage(attackDamage);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
    private void OnDrawGizmosSelected() {
        if (activeAttackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(activeAttackPoint.position, attackRange);
    }
}
