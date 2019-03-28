using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public enum AgentState
    {
        Idle = 0,
        Chasing,
        Dead
    }
    public AgentState state;
    public NavMeshAgent navMeshAgent;
    public Animator animController;
    private int speedHashId;

    public GameObject target;
    void Awake()
    {
        speedHashId = Animator.StringToHash("speed");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        target = null;
        navMeshAgent.SetDestination(transform.position);
        state = AgentState.Idle;
    }


    void Update()
    {
       
        if (state != AgentState.Dead) {
            if (target != null)
            {
                state = AgentState.Chasing;
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
                state = AgentState.Idle;
            }


            if (state == AgentState.Idle)
            {
                Idle();
                navMeshAgent.SetDestination(transform.position);
            }
            else if (state == AgentState.Chasing)
            {
                Chase();
            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
        
    }
    void Chase()
    {
        navMeshAgent.SetDestination(target.transform.position);
        Debug.Log("Target set to " + navMeshAgent.destination);
        navMeshAgent.stoppingDistance = 3;
        animController.SetFloat(speedHashId, 1);
    }
    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
    }
    void Patrol()
    {
        animController.SetFloat(speedHashId, 1.0f);
    }

    public void Dead()
    {
        state = AgentState.Dead;
    }
}