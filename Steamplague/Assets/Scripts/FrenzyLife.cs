using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyLife : MonoBehaviour
{
    private Rigidbody2D zombie;
    private Animator zombieAnim;
    private Frenzy movement;
    [SerializeField] private int maxHealth = 100;
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("zombie Ded");
        zombieAnim.SetTrigger("death");
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

    // private void RestartLevel()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     UnfreezeZombie();
    // }
}
