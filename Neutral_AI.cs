using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Neutral_AI : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator anim;
    private string state = "idle";
    private float wait = 0f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.speed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //idle
        if (state == "idle")
        {
            //pick a random place to walk to
            Vector3 randomPos = Random.insideUnitSphere * 10;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
            nav.SetDestination(navHit.position);
            state = "run";
            Debug.DrawLine(transform.position + randomPos, navHit.position, Color.red, NavMesh.AllAreas);
        }

        //run
        if (state == "run")
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isEating", false);
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                anim.SetBool("isRunning", false);
                state = "eat";
                wait = Random.Range(3f, 7f);
            }
        }

        //eat
        if (state == "eat")
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isEating", true);
            if (wait > 0f)
            {
                wait -= Time.deltaTime;
            }
            else
            {
                anim.SetBool("isEating", false);
                state = "idle";
            }
        }
    }
}
