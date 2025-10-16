using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BeastWander : MonoBehaviour
{
    public float walkSpeed = 1.2f;
    public float turnSpeed = 360f;
    public float wanderRadius = 4f;
    public float arriveThreshold = 0.2f;
    public float idleMin = 1f;
    public float idleMax = 4f;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            // idle
            animator.SetFloat("Speed", 0f);
            yield return new WaitForSeconds(Random.Range(idleMin, idleMax));

            // pick a random target on the XZ plane
            Vector2 r = Random.insideUnitCircle * wanderRadius;
            Vector3 target = transform.position + new Vector3(r.x, 0f, r.y);

            // move toward target
            while (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                    new Vector3(target.x, 0, target.z)) > arriveThreshold)
            {
                Vector3 dir = (target - transform.position);
                dir.y = 0;
                if (dir.sqrMagnitude < 0.001f) break;

                // rotate smoothly
                Quaternion goal = Quaternion.LookRotation(dir.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, goal, turnSpeed * Time.deltaTime);

                // move forward
                transform.position += transform.forward * walkSpeed * Time.deltaTime;
                animator.SetFloat("Speed", walkSpeed);

                yield return null;
            }
        }
    }
}
