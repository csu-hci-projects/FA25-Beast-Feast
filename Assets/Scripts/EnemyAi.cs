using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] Transform player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] LayerMask playerLayer, groundLayer;

    private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] float walkPointRange;

    [SerializeField] float sightRange;
    bool playerInSightRange;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);

        if (!playerInSightRange)
        {
            Patrol();
        }
        if (playerInSightRange)
        {
            ChasePlayer();
        }
    }

    private void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            navMeshAgent.SetDestination(walkPoint);
        }
        Vector3 distance = transform.position - walkPoint;

        if (distance.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    private void SearchWalkPoint()
    {
        float randomZPoint = Random.Range(-walkPointRange, walkPointRange);
        float randomXPoint = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomXPoint, transform.position.y, transform.position.z + randomZPoint);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
