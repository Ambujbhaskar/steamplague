using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : MonoBehaviour
{
    private Rigidbody2D worker;
    private Animator workerAnim;
    private SpriteRenderer workerSprite;
    private CapsuleCollider2D workerCollider;
    private Transform playerTransform;
    [SerializeField] private float velocity = 7f;
    [SerializeField] private float runTrigger = 6f;
    [SerializeField] private float walkTrigger = 12f;
    [SerializeField] private float attackTrigger = 1f;

    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRate = 0.6f;
    [SerializeField] private float knockbackVelocity = 2f;
    private float nextAttackTime = 0f;
    private Transform activeAttackPoint;

    private enum MovementState { idle, walking, running, attack };

    private void Start()
    {
        worker = GetComponent<Rigidbody2D>();
        workerAnim = GetComponent<Animator>();
        workerSprite = GetComponent<SpriteRenderer>();
        workerCollider = GetComponent<CapsuleCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        activeAttackPoint = attackPointLeft;
        // workerAnim.SetBool("entry", true);
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime && workerAnim.GetInteger("state") == 3) {
            workerAnim.SetTrigger("attack");
            nextAttackTime = Time.time + 1f / attackRate;
        }
        float dirX = 0;
        float distX = (playerTransform.position.x - transform.position.x);
        float dist = (playerTransform.position - transform.position).magnitude;

        // Debug.Log(dist + " " + distX);

        dirX = setState(dist, distX);
        worker.velocity = new Vector2(dirX * velocity, worker.velocity.y);
    }

    private float setState(float dist, float distX)
    {
        MovementState state;
        float dirX = 0; 
        if (distX < 0)
        {
            dirX = -1f;
            workerSprite.flipX = true;
            activeAttackPoint = attackPointLeft;
        }
        else if (distX > 0)
        {
            dirX = 1f;
            workerSprite.flipX = false;
            activeAttackPoint = attackPointRight;
        } else {
            activeAttackPoint = attackPointRight;
        }

        if (dist < walkTrigger && dist > -walkTrigger)
        {
            if (distX < runTrigger && distX > -runTrigger)
            {
                if (distX < attackTrigger && distX > -attackTrigger)
                {
                    if (distX < 0.1f && distX > -0.1f) {
                        state = MovementState.idle;
                        dirX = 0f;
                    } else {
                        // start attacking
                        state = MovementState.attack;
                        dirX *= 0.1f;
                    }
                }
                else
                {
                    // start running
                    state = MovementState.running;
                    dirX *= 0.7f;
                }
            }
            else
            {
                // start walking
                state = MovementState.walking;
                dirX *= 0.3f;
            }
        }
        else
        {
            // idle
            state = MovementState.idle;
            dirX *= 0f;
        }
        workerAnim.SetInteger("state", (int)state);
        return dirX;
    }

    private void Attack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, attackRange, playerLayer);

        foreach(Collider2D enemy in hitEnemies) {
            Debug.Log("Hitting: "+ enemy.name);
            if (enemy.name != "FrenziedWorker")
                enemy.GetComponent<PlayerLife>().TakeDamage(attackDamage);
        }
    }

    public void Knockback() {
        float distX = (playerTransform.position.x - transform.position.x);
        if (distX > 0f) {
            worker.velocity = new Vector2(-knockbackVelocity * velocity, worker.velocity.y);
        } else if (distX < 0f) {
            worker.velocity = new Vector2(-knockbackVelocity * velocity, worker.velocity.y);
        }
    }

    private void OnDrawGizmosSelected() {
        if (activeAttackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(activeAttackPoint.position, attackRange);
    }
}
