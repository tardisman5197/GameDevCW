using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{

    public float maxHealth = 10;
    public float currentHealth;
    public string switchSceneOnDeath = "";

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    void Die()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        if (switchSceneOnDeath != "") {
            SceneManager.LoadSceneAsync(switchSceneOnDeath);
        }
        //this.transform.Rotate(new Vector3(90f, 0f));
    }
}
