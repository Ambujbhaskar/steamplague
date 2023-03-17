using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    
    [SerializeField] private SoundManagerScript soundManager;
    private Rigidbody2D player;
    [SerializeField] GameObject healthBar;
    [SerializeField] Text scrapCountText;
    private Animator playerAnim;
    public Text deathText;
    private int scrapCount;
    public int attackIncrement = 2;
    
    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        player = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("death", false);
        scrapCount = 0;
    }

    public int GetHealth() {
        return currentHealth;
    }
    public void TakeDamage(int damage) {
        Debug.Log("Taking Damage of: " + damage);
        currentHealth -= damage;
        
        healthBar.transform.localScale = new Vector3((float)((float)currentHealth / (float)maxHealth), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        playerAnim.SetTrigger("hurt");
        soundManager.playSound("player_hit");
        if (currentHealth <= 0) {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Ded");
        playerAnim.SetTrigger("death");
        soundManager.playSound("player_dying");
        DisablePlayer();
        deathText.gameObject.SetActive(true);
    }    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Deathzone"))
        {
            Die();
        }
        if (collision.gameObject.CompareTag("Scrap"))
        {
            Destroy(collision.gameObject);
            soundManager.playSound("pickup_item");
            UpdateScraps();
        }
    }


    private void DisablePlayer() {
        
        player.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
    }
    private void EnablePlayer() {
        this.enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
    }

    private void DestroyPlayer() {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateScraps() {
        scrapCount++;
        scrapCountText.text = ("x " + scrapCount);
        if (scrapCount % 3 == 0) {
            PlayerMovement movement = GetComponent<PlayerMovement>();
            movement.attackDamage += attackIncrement;
        }
    }
}
