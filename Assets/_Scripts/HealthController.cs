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

    private int animiatorDiedID;

    private bool dead;

    public bool player;

    public GameObject scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = maxHealth;
        this.animiatorDiedID = Animator.StringToHash("death");
        this.dead = false;
        scoreUI = GameObject.Find("Score");
        if (player)
        {
            scoreUI.GetComponent<ScoreUI>().UpdateHealth(currentHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentHealth = 0;
            }
        }

        if (currentHealth <= 0)
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Damaged: " + currentHealth);
        //hitSound.Play();

        if (player)
        {
            scoreUI.GetComponent<ScoreUI>().UpdateHealth(currentHealth);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (player)
        {
            scoreUI.GetComponent<ScoreUI>().UpdateHealth(currentHealth);
        }
    }

    void Die()
    {
        if (switchSceneOnDeath != "") {
            SceneManager.LoadSceneAsync(switchSceneOnDeath);
        }

        Debug.Log("Died!");
        this.dead = true;

        // If the current player
        if (player)
        {
            Debug.Log("Player Dead");
            GameObject.Find("Main Camera").GetComponent<LevelController>().PlayerDeath(this.gameObject);
        }

        // If an enemy
        if (transform.GetComponent<EnemyController>() != null)
        {
            transform.GetComponent<EnemyController>().Dead();
            GameObject.Find("Main Camera").GetComponent<LevelController>().EnemyDeath();
        }

        if (this.GetComponent<Animator>() != null)
        {
            this.GetComponent<Animator>().SetTrigger(animiatorDiedID);
        }
    }
}
