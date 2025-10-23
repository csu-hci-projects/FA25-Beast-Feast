using UnityEngine;
using UnityEngine.AI;

public class DirectedAgent : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false; // We’ll handle rotation manually
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // Move relative to camera direction
        if (Camera.main)
        {
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0;
            inputDir = (camForward.normalized * v + camRight.normalized * h).normalized;
        }

        if (inputDir.magnitude >= 0.1f)
        {
            agent.Move(inputDir * agent.speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDir), 0.15f);
        }

        // Animation blend (same parameter as before)
        float speed = inputDir.magnitude * agent.speed;
        animator.SetFloat("forwardSpeed", speed);
    }
}
