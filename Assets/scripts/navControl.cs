using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navControl : MonoBehaviour
{
    public GameObject TargetAttack;
    public GameObject TargetFacing;
    private NavMeshAgent agent;

    public float clipSpeed = 1;
    public float walkSpeed = 1.5f;

    bool isWalking = true;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.speed = clipSpeed;
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
            agent.destination = TargetAttack.transform.position;
        else
        {
            agent.destination = transform.position;
            rotateTowardsTarget();
        }

        if(animator.speed != clipSpeed)
        {
            animator.speed = clipSpeed;
        }
        if (agent.speed != walkSpeed)
        {
            agent.speed = walkSpeed;
            clipSpeed = (2 * walkSpeed) / 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == TargetAttack.name)
        {
            isWalking = false;
            animator.SetTrigger("attack");
        }
    }

    void rotateTowardsTarget()
    {
        float stepSize = agent.angularSpeed * Time.deltaTime;

        Vector3 targetDir = TargetFacing.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        agent.transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == TargetAttack.name)
        {
            isWalking = true;
            animator.SetTrigger("walk");
        }
    }
}
