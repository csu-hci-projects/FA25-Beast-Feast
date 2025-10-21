using UnityEngine;
using UnityEngine.AI;

public class PassiveAi : MonoBehaviour
{
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float wanderTimer = 5f;
    [SerializeField] float avoidRadius = 8f;
    [SerializeField] float avoidStrength = 12f;

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

            if (distance <= avoidRadius)
            {
                Vector3 awayDir = (transform.position - player.position).normalized;
                Vector3 newPos = transform.position + awayDir * avoidStrength;

                if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, avoidStrength, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = GetRandomPosition(transform.position, wanderRadius, NavMesh.AllAreas);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
            }
        }

        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("Speed", speed);
    }

    public static Vector3 GetRandomPosition(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }
}
