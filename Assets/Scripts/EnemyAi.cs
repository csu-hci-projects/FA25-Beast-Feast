using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float wanderTimer = 5f;
    [SerializeField] float playerRadius = 8f;
    [SerializeField] float attackRadius = 2f;

    private NavMeshAgent agent;
    private float timer;
    private Transform player;
    private Animator animator;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= playerRadius)
            {
                agent.SetDestination(player.position);

                if (distance <= attackRadius)
                {
                    agent.isStopped = true;
                    animator.SetTrigger("AttackTrigger");
                }
                else
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = GetRandomPosition(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
            }
        }
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }

    public static Vector3 GetRandomPosition(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }
}
