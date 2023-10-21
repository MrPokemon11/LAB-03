using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navControlTopper : MonoBehaviour
{
    public GameObject TargetAttack;
    public GameObject TargetFacing;
    private NavMeshAgent agent;
    private Rigidbody body;

    public float clipSpeed = 1;
    public float walkSpeed = 1.5f;

    bool isWalking = true;
    private Animator animator;
    public float knockbackForce;

    bool stageOne = true;
    bool stageTwo = false;
    bool stageThree = false;

    int hitCount = 0;
    float stageTwoCounter = 0;

    public List<GameObject> waypoints;
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();

        animator.speed = clipSpeed;
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if(stageOne || stageThree)
            {
                agent.destination = TargetAttack.transform.position;
            }
            else if (stageTwo)
            {
                agent.destination = waypoints[waypointIndex].transform.position;
            }
            
        }
            
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
        if(other.name == waypoints[waypointIndex].name && stageTwo == true)
        {
            
            if(waypointIndex == waypoints.Count-1)
            {
                StartCoroutine("startStageThree");
                stageTwo = false;
                stageThree = true;
                
                return;
            }
            waypointIndex++;
        }

        
    }

    void topperOnHit()
    {
        StartCoroutine("topperHitFunction");
       
        //if(hitCount % 2 == 0)
        //{
            
            if(hitCount >= 3 && stageOne)
            {
                animator.SetTrigger("fall");
                StartCoroutine("startStageTwo");
             }

        if (hitCount >= 3 && stageThree)
        {
            animator.SetTrigger("fall");
            
        }
        //}


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
            //animator.SetTrigger("walk");
        }
    }

    IEnumerator startStageTwo()
    {
        yield return new WaitForSecondsRealtime(3f);
        animator.SetTrigger("getUp");
        yield return new WaitForSecondsRealtime(2f);
        stageOne = false;
        stageTwo = true;
        stageTwoStuff();
        StopCoroutine("startStageTwo");
    }

    //handles removing kinematics, adding spin, etc
    void stageTwoStuff()
    {
        body.isKinematic = false;
        body.angularVelocity = new Vector3(0, 1000, 0);
        //body.angularDrag = 0;
        body.velocity = new Vector3(Random.Range(5, 10), 0, Random.Range(5, 10));
        body.inertiaTensor = new Vector3(Random.Range(20, 60), 0, Random.Range(20, 60));
        //agent.enabled = false;
        walkSpeed = 5;
    }

    IEnumerator startStageThree()
    {
        yield return new WaitForSecondsRealtime(1f);
        //code
        stageThreeStuff();
        walkSpeed = 1.5f;
        SendMessageUpwards("TopperDown");
        StopCoroutine("StartStageThree");
    }

    //handles reapplying kinematics, removing spin, etc
    void stageThreeStuff()
    {
        body.isKinematic = true;
        agent.enabled = true;
        body.angularVelocity = new Vector3(0, 0, 0);
        walkSpeed = 1.5f;
        hitCount = 0;
    }

    public IEnumerator topperHitFunction()
    {
        animator.SetTrigger("hit");
        agent.transform.position = agent.transform.position + Vector3.forward * knockbackForce;
        hitCount++;
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine("topperHitFunction");
    }
}

