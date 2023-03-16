using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    // private Rigidbody2D zombie;
    // private Animator zombieAnim;
    [SerializeField] private SpriteRenderer zombieSprite;
    [SerializeField] private BoxCollider2D zombieCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float range = 3f;
    [SerializeField] private float colliderDistance = 1f;

    private float cooldownTimer = Mathf.Infinity;
    private bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        // zombie = GetComponent<Rigidbody2D>();
        // zombieAnim = GetComponent<Animator>();
        // zombieSprite = GetComponent<SpriteRenderer>();
        // zombieCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                // TODO: Attack
                Attack();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {        
            print("enter");
            cooldownTimer = 0;
            canAttack = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // print("stay");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("exit");
            cooldownTimer = 0;
            canAttack = false;
        }
    }

    private bool PlayerInRange()
    {
        // int dirX = 1;
        // if (zombieSprite.flipX)
        //     dirX = -1;
        // RaycastHit2D hit = Physics2D.BoxCast(zombieCollider.bounds.center + (dirX * transform.right * range) * colliderDistance, new Vector3(zombieCollider.bounds.size.x * range, zombieCollider.bounds.size.y, zombieCollider.bounds.size.z), 0f, Vector2.left, 0f, playerLayer);
        // return (hit.collider != null);
        return false;
    }

    private void Attack()
    {
        print("Attacking player");
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     int dirX = 1;
    //     if (zombieSprite.flipX)
    //         dirX = -1;
    //     Gizmos.DrawWireCube(zombieCollider.bounds.center + (dirX * transform.right * range) * colliderDistance, new Vector3(zombieCollider.bounds.size.x * range, zombieCollider.bounds.size.y, zombieCollider.bounds.size.z));
    // }
}
