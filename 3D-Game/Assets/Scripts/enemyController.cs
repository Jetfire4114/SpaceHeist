using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public Transform player;
    public float playerDistance;
    public float awareAI = 10f;
    public float AIMoveSpeed;
    public float damping = 6.0f;
    public Transform[] navPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public int destPoint = 0;
    public Transform goal;
    public static float enemyHealth;

    private bool isPaused = false;
    private float pauseEndTime = 0f;
    private Vector3 pausedVelocity;

    void Start()
    {
        enemyHealth = 100;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        agent.autoBraking = false;
    }

    void Update()
    {
        Debug.Log(enemyHealth);

        if (enemyHealth <= 0)
            Destroy(gameObject);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = true;
            pauseEndTime = Time.time + 4f;
            // Pause NavMeshAgent movement
            agent.isStopped = true;
        }

        if (isPaused && Time.time >= pauseEndTime)
        {
            isPaused = false;
            // Resume NavMeshAgent movement
            agent.isStopped = false;
        }

        if (!isPaused)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            if (playerDistance < awareAI)
            {
                LookAtPlayer();
                Debug.Log("Seen");
            }

            if (playerDistance < awareAI)
            {
                if (playerDistance < 6f)
                {
                    Chase();
                }
                else
                    GotoNextPoint();
            }

            if (agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void GotoNextPoint()
    {
        if (navPoint.Length == 0)
            return;
        agent.destination = navPoint[destPoint].position;
        destPoint = (destPoint + 1) % navPoint.Length;
    }

    void Chase()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
    }
}
