using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;
    public string switchSceneOnDeath = "";
    public AudioSource hitSound;

    public bool dead;

    // Animation IDs
    private int animiatorDiedID;
    private int animiatorDeadID;

    // player is true if this charactor is the controlled by the player
    public bool player;

    // scoreUI is the HUD
    public GameObject scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = maxHealth;
        this.dead = false;

        this.animiatorDiedID = Animator.StringToHash("death");
        this.animiatorDeadID = Animator.StringToHash("dead");

        scoreUI = GameObject.Find("Score");

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug tool to kill player
        if (player)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentHealth = 0;
            }
        }

        // Check if the chractor is dead
        if (currentHealth <= 0)
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    // Damage is used to reduce the health of the charactor
    public void Damage(float amount)
    {
        currentHealth -= amount;

        if (hitSound != null)
        {
            hitSound.Play();
        }

        // Update the UI if this is the player
        UpdateUI();
    }

    // Heal is used to increase the chractor's health
    public void Heal(int amount)
    {
        currentHealth += amount;

        UpdateUI();
    }

    // Die executes the actions that should take place in the
    // event of this chractor's death
    void Die()
    {
        // If the scene should change change it
        if (switchSceneOnDeath != "") {
            SceneManager.LoadSceneAsync(switchSceneOnDeath);
        }

        this.dead = true;

        // If the current player
        if (player)
        {
            GameObject.Find("Main Camera").GetComponent<LevelController>().PlayerDeath(this.gameObject);
        }

        // If an enemy
        if (transform.GetComponent<EnemyController>() != null)
        {
            transform.GetComponent<EnemyController>().Dead();

            GameObject.Find("Main Camera").GetComponent<LevelController>().EnemyDeath();
        }

        // Play death animation
        if (this.GetComponent<Animator>() != null)
        {
            this.GetComponent<Animator>().SetTrigger(animiatorDiedID);
            this.GetComponent<Animator>().SetBool(animiatorDeadID, true);
        }
    }

    // UpdateUI changes the current health displayed on the HUD
    // if the charactor is the current player
    void UpdateUI()
    {
        if (player)
        {
            scoreUI.GetComponent<ScoreUI>().UpdateHealth(currentHealth);
        }
    }
}
