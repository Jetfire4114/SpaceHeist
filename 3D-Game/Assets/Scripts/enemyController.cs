using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyController : MonoBehaviour
{
    public Transform player;
    public float playerDistance;
    public float awareAI = 10f;
    public float AIMoveSpeed;
    public float damping = 6.0f;
    public Transform[] navPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;
    public static float enemyHealth;

    public GameObject enemyStunVFX; // Reference to the EnemyStunVFX GameObject

    private bool isPaused = false;
    private int spacePressCounter = 0;
    private const int maxSpacePresses = 2;
    private float pauseEndTime = 0f;
    private Vector3 pausedVelocity;
    private int destPoint = 0;

    public TMP_Text spacePressCounterText;

    void Start()
    {
        enemyHealth = 100;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        agent.autoBraking = false;
        UpdateSpacePressCounterText();

        // Ensure the EnemyStunVFX starts off deactivated
        if (enemyStunVFX != null)
        {
            enemyStunVFX.SetActive(false);
        }
    }

    void Update()
    {
        Debug.Log(enemyHealth);

        if (enemyHealth <= 0)
            Destroy(gameObject);

        if (Input.GetKeyDown(KeyCode.Space) && spacePressCounter < maxSpacePresses)
        {
            spacePressCounter++;
            UpdateSpacePressCounterText();
            isPaused = true;
            pauseEndTime = Time.time + 4f;
            // Pause NavMeshAgent movement
            agent.isStopped = true;
            // Activate EnemyStunVFX
            if (enemyStunVFX != null)
            {
                enemyStunVFX.SetActive(true);
            }
        }

        if (isPaused && Time.time >= pauseEndTime)
        {
            isPaused = false;
            // Resume NavMeshAgent movement
            agent.isStopped = false;
            // Deactivate EnemyStunVFX
            if (enemyStunVFX != null)
            {
                enemyStunVFX.SetActive(false);
            }
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

    void UpdateSpacePressCounterText()
    {
        if (spacePressCounterText != null)
        {
            spacePressCounterText.text = "S.T.U.N. Remaining: " + (maxSpacePresses - spacePressCounter);
        }
    }
}
