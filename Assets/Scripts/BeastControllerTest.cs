using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BeastControllerTest : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] bool rotate = true;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0, v);
        float mag = Mathf.Clamp01(input.magnitude);

        if (mag > 0.01f)
        {
            Vector3 dir = input.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            if (rotate)
            {
                transform.position += -transform.forward * moveSpeed * mag * Time.deltaTime;
            } else
            {
                transform.position += transform.forward * moveSpeed * mag * Time.deltaTime;
            }
            
        }

        animator.SetFloat("Speed", mag * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("AttackTrigger");
        }
    }
}
