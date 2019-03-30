using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<EnemyController>().target != null)
        {
            if (transform.parent.GetComponent<EnemyController>().target.GetComponent<HealthController>() != null)
            {
                if (transform.parent.GetComponent<EnemyController>().target.GetComponent<HealthController>().dead)
                {
                    transform.parent.GetComponent<EnemyController>().target = null;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<HealthController>() != null)
            {
                if (!other.gameObject.GetComponent<HealthController>().dead)
                {
                    Debug.Log("New Target");
                    transform.parent.GetComponent<EnemyController>().target = other.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(transform.parent.GetComponent<EnemyController>().target))
        {
            transform.parent.GetComponent<EnemyController>().target = null;
        }
    }

}
