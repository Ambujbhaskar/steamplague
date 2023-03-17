using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyLife : MonoBehaviour
{
    private Rigidbody2D zombie;
    private Animator zombieAnim;
    private Frenzy movement;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject scrapPrefab;
    
    [SerializeField] private SoundManagerScript soundManager;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        zombie = GetComponent<Rigidbody2D>();
        zombieAnim = GetComponent<Animator>();
        movement = GetComponent<Frenzy>();
        zombieAnim.SetBool("death", false);
    }

    public void TakeDamage(int damage)
    {
        movement.Knockback();
        currentHealth -= damage;
        Debug.Log("Frenzied Worker Taking Damage of: " + damage);

        zombieAnim.SetTrigger("hurt");
        soundManager.playSound("zombie_hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("zombie Ded");
        zombieAnim.SetTrigger("death");
        soundManager.playSound("zombie_death");
        Vector3 scrapPos = new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z);
        Instantiate(scrapPrefab, scrapPos, Quaternion.identity);
        FreezeZombie();
    }

    private void FreezeZombie()
    {
        zombie.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        GetComponent<Frenzy>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    private void UnfreezeZombie()
    {
        zombie.bodyType = RigidbodyType2D.Static;
        this.enabled = true;
        GetComponent<Frenzy>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void DestroyZombie() {
        Destroy(gameObject);
    }
}
