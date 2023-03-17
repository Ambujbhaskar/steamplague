using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : MonoBehaviour
{
    private Rigidbody2D robotBoss;
    private Animator robotBossAnim;
    private SpriteRenderer robotBossSprite;
    private CapsuleCollider2D robotBossCollider;
    private Transform playerTransform;
    [SerializeField] private float velocity = 2f;
    [SerializeField] private float walkTrigger = 6f;
    [SerializeField] private float viewTrigger = 10f;
    [SerializeField] private float attackTrigger = 6f;

    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private SpriteRenderer projectileLeft;
    [SerializeField] private SpriteRenderer projectileRight;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRate = 0.2f;
    [SerializeField] private float knockbackVelocity = 0f;
    private float nextAttackTime = 0f;
    private Transform activeAttackPoint;
    private bool canAttack = false;

    private enum MovementState { idle, walking, running, attack };

    private void Start()
    {
        robotBoss = GetComponent<Rigidbody2D>();
        robotBossAnim = GetComponent<Animator>();
        robotBossSprite = GetComponent<SpriteRenderer>();
        robotBossCollider = GetComponent<CapsuleCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // attackPointLeft.position.x = -attackRange / 2;
        // attackPointRight.position.x = attackRange / 2;
        activeAttackPoint = attackPointLeft;
        // robotBossAnim.SetBool("entry", true);
        projectileLeft.enabled = false;
        projectileRight.enabled = false;
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            canAttack = true;    
            if (robotBossAnim.GetInteger("state") == 3)
            {
                robotBossAnim.SetTrigger("attack");
                nextAttackTime = Time.time + 1f / attackRate;
                canAttack = false;
            }
        }
        float dirX = 0;
        float distX = (playerTransform.position.x - transform.position.x);
        float dist = (playerTransform.position - transform.position).magnitude;

        // Debug.Log(dist + " " + distX);

        dirX = setState(dist, distX);
        robotBoss.velocity = new Vector2(dirX * velocity, robotBoss.velocity.y);
    }

    private float setState(float dist, float distX)
    {
        MovementState state;
        float dirX = 0; 
        if (distX < 0)
        {
            dirX = -1f;
            robotBossSprite.flipX = false;
            activeAttackPoint = attackPointLeft;
        }
        else if (distX > 0)
        {
            dirX = 1f;
            robotBossSprite.flipX = true;
            activeAttackPoint = attackPointRight;
        }
        else
        {
            activeAttackPoint = attackPointRight;
        }

        if ((dist > walkTrigger && dist < viewTrigger) || (dist < -walkTrigger && dist > -viewTrigger))
        {
            // start walking
            state = MovementState.walking;
            // dirX *= 0.3f;
            // if (distX < runTrigger && distX > -runTrigger)
            // {
            //     if (distX < attackTrigger && distX > -attackTrigger)
            //     {
            //         if (distX < 0.1f && distX > -0.1f) {
            //             state = MovementState.idle;
            //             dirX = 0f;
            //         }
            //         else {
            //             // start attacking
            //             state = MovementState.attack;
            //             dirX *= 0.1f;
            //         }
            //     }
            //     else
            //     {
            //         // start running
            //         state = MovementState.running;
            //         dirX *= 0.7f;
            //     }
            // }
            // else
            // {
            //     // start walking
            //     state = MovementState.walking;
            //     dirX *= 0.3f;
            // }
        }
        else if (dist < attackRange && dist > -attackRange)
        {
            // attack
            if (canAttack)
                state = MovementState.attack;
            else
                state = MovementState.idle;
            dirX *= 0f;
        }
        else
        {
            // idle
            state = MovementState.idle;
            dirX *= 0f;
        }
        robotBossAnim.SetInteger("state", (int)state);
        return dirX;
    }

    private void Attack() 
    {   
        if (playerTransform.position.x - transform.position.x < 0)
            projectileLeft.enabled = true;
        else
            projectileRight.enabled = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, attackRange / 2, playerLayer);
        foreach(Collider2D enemy in hitEnemies) {
            Debug.Log("RobotBoss is Hitting: " + enemy.name);
            if (enemy.name == "Player")
                enemy.GetComponent<PlayerLife>().TakeDamage(attackDamage);
        }
    }

    private void ProjectileDisappear()
    {
        projectileLeft.enabled = false;
        projectileRight.enabled = false;
    }

    public void Knockback() {
        float distX = (playerTransform.position.x - transform.position.x);
        if (distX > 0f) {
            robotBoss.velocity = new Vector2(-knockbackVelocity * velocity, robotBoss.velocity.y);
        } else if (distX < 0f) {
            robotBoss.velocity = new Vector2(-knockbackVelocity * velocity, robotBoss.velocity.y);
        }
    }

    private void OnDrawGizmosSelected() {
        if (activeAttackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(activeAttackPoint.position, attackRange / 2);
    }
}
