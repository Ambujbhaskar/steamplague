using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator playerAnim;
    
    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        player = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("death", false);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("Taking Damage of: " + damage);

        playerAnim.SetTrigger("hurt");
        if (currentHealth <= 0) {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Ded");
        playerAnim.SetTrigger("death");
        FreezePlayer();
    }

    private void FreezePlayer() {
        player.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    private void UnfreezePlayer() {
        player.bodyType = RigidbodyType2D.Static;
        this.enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnfreezePlayer();
    }
}
