using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    private BoxCollider coll;
    private NavMeshAgent agent;
    private Animator animator;

    private bool canWalk = true;
    public int maxWaitTime;
    private bool beingGrabbed = false;
    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent.destination = RandomNavSphere(transform.position, 5.0f, -1);
        agent.isStopped = false;
        animator.SetBool("Walking", true);
        canWalk = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!agent.enabled)
        {
            return;
        }
        //
        float dist = agent.remainingDistance; 
        Debug.Log("\nDistance: "+dist+"\nPath Status: "+agent.pathStatus+"\nRemaining Distance: "+agent.remainingDistance+"\n");
        if (dist != Mathf.Infinity && agent.remainingDistance == 0) // agent.pathStatus == NavMeshPathStatus.PathComplete &&
        {
            if (canWalk)
            {
                agent.destination = RandomNavSphere(transform.position, 5.0f, -1);
                agent.isStopped = false;
                animator.SetBool("Walking", true);
                canWalk = false;
            } else
            {
                animator.SetBool("Walking", false);
                StartCoroutine(waiter());
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (beingGrabbed)
        {
            return;
        }
        if (collision.transform.tag == "Ground")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            agent.enabled = true;
        }
    }

    IEnumerator waiter()
    {
        int wait_time = Random.Range(0, 2);
        yield return new WaitForSeconds(wait_time);
        canWalk = true;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        //
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    public void StartGrab()
    {
        agent.enabled = false;
        beingGrabbed = true;
        coll.isTrigger = true;
    }
    public void FinishGrab()
    {
        coll.isTrigger = false;
        GetComponent<Rigidbody>().isKinematic = false;
        beingGrabbed = false;
    }
}
