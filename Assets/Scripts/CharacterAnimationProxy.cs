using UnityEngine;
using KinematicCharacterController.Examples;

[RequireComponent(typeof(Animator))] 
public class NewMonoBehaviourScript : MonoBehaviour
{

    public ExampleCharacterController CharacterController;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("forward", CharacterController.NormalizedForwardSpeed);
        _animator.SetBool("IsGrounded", CharacterController.IsGrounded);
        _animator.SetBool("JumpRequested", CharacterController.JumpRequested);
    }
}
