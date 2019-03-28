using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text deaths;
    public int noOfDeaths;

    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        noOfDeaths = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDeath()
    {
        noOfDeaths++;
        deaths.text = "Deaths: " + noOfDeaths;
    }

    public void UpdateHealth(float health)
    {
        healthText.text = "Health: " + health;
    }
}
