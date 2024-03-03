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
    public GameObject enemyStunVFX;

    private GameController gameController;
    private bool isPaused = false;
    private int spacePressCounter = 0;
    public int maxSpacePresses = 3;
    private float pauseEndTime = 0f;
    private int destPoint = 0;
    public TMP_Text spacePressCounterText;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        agent.autoBraking = false;
        UpdateSpacePressCounterText();

        if (enemyStunVFX != null)
        {
            enemyStunVFX.SetActive(false);
        }

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (gameController.winCanvas.activeSelf)
        {
            PauseEnemies();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && spacePressCounter < maxSpacePresses)
        {
            spacePressCounter++;
            UpdateSpacePressCounterText();
            isPaused = true;
            pauseEndTime = Time.time + 4f;
            agent.isStopped = true;
            if (enemyStunVFX != null)
            {
                enemyStunVFX.SetActive(true);
            }
        }

        if (isPaused && Time.time >= pauseEndTime)
        {
            isPaused = false;
            agent.isStopped = false;
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
                {
                    GotoNextPoint();
                }
            }

            if (agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void GotoNextPoint()
    {
        if (navPoint.Length == 0)
        {
            return;
        }
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

    public void UpdateSpacePressCounterUI()
    {
        UpdateSpacePressCounterText();
    }

    void PauseEnemies()
    {
        agent.isStopped = true;
    }
}
