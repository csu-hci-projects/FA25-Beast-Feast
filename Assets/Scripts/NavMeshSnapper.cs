using UnityEngine;
using UnityEngine.AI;

public class NavMeshSnapper : MonoBehaviour
{
    [SerializeField] float maxSample = 5f;
    [SerializeField] float snapOffset = 0.01f;

    private void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, maxSample, NavMesh.AllAreas))
        {
            agent.Warp(hit.position + Vector3.up * snapOffset);
        }
    }
}
