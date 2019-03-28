using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public enum AgentState
    {
        Idle = 0,
        Chasing,
        Dead,
        Attacking
    }
    public AgentState state;
    public NavMeshAgent navMeshAgent;
    public Animator animController;
    private int speedHashId;
    private int attackId;
    private bool dead;

    public GameObject target;
    void Awake()
    {
        speedHashId = Animator.StringToHash("speed");
        attackId = Animator.StringToHash("attack");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        target = null;
        navMeshAgent.SetDestination(transform.position);
        state = AgentState.Idle;
        dead = false;
    }


    void Update()
    {
       
        if (state != AgentState.Dead && !dead) {
            if (target != null)
            {
                state = AgentState.Chasing;

                if (Vector3.Distance(target.transform.position, transform.position) < 2.5) {
                    Debug.Log("Attack");
                    Attack();
                }
            }
            else
            {
                state = AgentState.Idle;
                navMeshAgent.SetDestination(transform.position);

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
            else if (state == AgentState.Attacking)
            {
                Attack();
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
        navMeshAgent.stoppingDistance = 2;
        animController.SetFloat(speedHashId, 1);
    }
    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
    }

    void Attack()
    {
        animController.SetTrigger(attackId);
    }

    public void Dead()
    {
        Debug.Log("Dead");
        dead = true;
        state = AgentState.Dead;
    }
}