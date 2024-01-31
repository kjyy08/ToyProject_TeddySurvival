using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    public ParticleSystem effect;
    public float colRadius = 25f;
    public Transform[] movePoints;

    private Animator animator;
    private NavMeshAgent nav;
    private Vector3 targetPosition;
    private Vector3 targetDir;


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", true);
    }
}
