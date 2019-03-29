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

    // Animation ids
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

        dead = false;

        target = null;
        navMeshAgent.SetDestination(transform.position);

        state = AgentState.Idle;
    }

    void Update()
    {
        // Check if still alive
        if (state != AgentState.Dead && !dead) {
            // Check if there is a target to follow
            if (target != null)
            {
                state = AgentState.Chasing;

                // Check if close enough to attack
                if (Vector3.Distance(target.transform.position, transform.position) < 2.5) {
                    Debug.Log("Attack");
                    Attack();
                }
            }
            else
            {
                // Stand still
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

    // Chase sets the nav mesh agent target and starts the running
    // animation.
    void Chase()
    {
        navMeshAgent.SetDestination(target.transform.position);
        navMeshAgent.stoppingDistance = 2;
        animController.SetFloat(speedHashId, 1);
    }

    // Idle changes the animation state 
    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
    }

    // Attack changes the animation state
    void Attack()
    {
        animController.SetTrigger(attackId);
    }

    // Dead updates the dead variable.
    public void Dead()
    {
        Debug.Log("Dead");
        dead = true;
        state = AgentState.Dead;
        navMeshAgent.SetDestination(transform.position);
    }
}