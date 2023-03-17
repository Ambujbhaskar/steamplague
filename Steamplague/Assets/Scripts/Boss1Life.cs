using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1Life : MonoBehaviour
{
    private Rigidbody2D zombie;
    private Animator zombieAnim;
    public Text WinText;
    public Button WinBtn;
    private Boss1Movement movement;
    [SerializeField] private int maxHealth = 300;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        zombie = GetComponent<Rigidbody2D>();
        zombieAnim = GetComponent<Animator>();
        movement = GetComponent<Boss1Movement>();
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
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Boss1Movement>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void DestroyZombie() {
        WinText.gameObject.SetActive(true);
        Cursor.visible = true;
        WinBtn.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
