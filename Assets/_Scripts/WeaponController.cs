using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public float damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Hit: " + col.gameObject.name);

        if (col.gameObject.tag == "Dummy")
        {
            Debug.Log("Hit Dummy: " + col.gameObject.name);
            col.gameObject.GetComponent<HealthController>().Damage(damage);
        }
    }
}
