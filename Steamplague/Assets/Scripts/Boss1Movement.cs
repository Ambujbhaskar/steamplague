using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : MonoBehaviour
{
    private Rigidbody2D worker;
    private Animator workerAnim;
    private SpriteRenderer workerSprite;
    private CapsuleCollider2D workerCollider;
    private Transform playerTransform;
    [SerializeField] private float velocity = 7f;
    [SerializeField] private float walkTrigger = 12f;
    [SerializeField] private float LattackTrigger = 12f;
    [SerializeField] private float HattackTrigger = 1f;

    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRate = 0.6f;
    private float nextAttackTime = 0f;
    private Transform activeAttackPoint;

    private enum MovementState { idle, walking, Hattack, Lattack };

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
        if (Time.time >= nextAttackTime)
        {
            if (workerAnim.GetInteger("state") == 3) {
                workerAnim.SetTrigger("Lattack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (workerAnim.GetInteger("state") == 2) {
                workerAnim.SetTrigger("Hattack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
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
        }
        else
        {
            activeAttackPoint = attackPointRight;
        }

        if (dist < walkTrigger && dist > -walkTrigger)
        {
            if (distX < LattackTrigger && distX > -LattackTrigger)
            {
                if (distX < HattackTrigger && distX > -HattackTrigger)
                {
                    if (distX < 0.1f && distX > -0.1f)
                    {
                        state = MovementState.idle;
                        dirX = 0f;
                    }
                    else
                    {
                        // star H attacking
                        state = MovementState.Hattack;
                        dirX *= 0.1f;
                    }
                }
                else
                {
                    // start L attacking
                    state = MovementState.Lattack;
                    dirX *= 0.3f;
                }
            }
            else
            {
                // start walking
                state = MovementState.walking;
                dirX *= 0.18f;
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

    private void Attack(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting: " + enemy.name);
            if (enemy.name != "Foreman")
                enemy.GetComponent<PlayerLife>().TakeDamage(damage);
        }
    }

    public void Knockback()
    {
        // no knockback to boss
    }

    private void OnDrawGizmosSelected()
    {
        if (activeAttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(activeAttackPoint.position, attackRange);
    }
}
