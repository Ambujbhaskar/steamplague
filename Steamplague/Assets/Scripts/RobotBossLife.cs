using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossLife : MonoBehaviour
{
    private Rigidbody2D robotBoss;
    private Animator robotBossAnim;
    private RobotBoss movement;
    [SerializeField] private int maxHealth = 500;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        robotBoss = GetComponent<Rigidbody2D>();
        robotBossAnim = GetComponent<Animator>();
        movement = GetComponent<RobotBoss>();
        robotBossAnim.SetBool("death", false);
        robotBossAnim.SetBool("hurt", false);
    }

    public void TakeDamage(int damage)
    {
        // movement.Knockback();
        currentHealth -= damage;
        Debug.Log("Robot Boss Taking Damage of: " + damage);

        robotBossAnim.SetTrigger("hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("robotBoss Ded");
        robotBossAnim.SetTrigger("death");
        FreezerobotBoss();
    }

    private void FreezerobotBoss()
    {
        robotBoss.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        GetComponent<RobotBoss>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    private void UnfreezerobotBoss()
    {
        robotBoss.bodyType = RigidbodyType2D.Static;
        this.enabled = true;
        GetComponent<RobotBoss>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    // private void RestartLevel()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     UnfreezerobotBoss();
    // }
}
