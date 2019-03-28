using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public float damage = 10;
    public bool enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (!enemy)
        {
            if (col.gameObject.tag == "Dummy")
            {
                Debug.Log("Hit Dummy: " + col.gameObject.name);
                col.gameObject.GetComponent<HealthController>().Damage(damage);
            }

            if (col.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit Enemy: " + col.gameObject.name);
                col.gameObject.GetComponent<HealthController>().Damage(damage);
            }

        } else
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("Hit Player: " + col.gameObject.name);
                col.gameObject.GetComponent<HealthController>().Damage(damage);
            }
        }

        
    }
}
