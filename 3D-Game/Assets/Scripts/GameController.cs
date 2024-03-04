using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject pauseCanvas;
    public GameObject player;
    public GameObject gameOverCanvas;
    public GameObject winCanvas;
    public GameObject escapeCanvas;
    public GameObject aboutMMCanvas;
    public GameObject optionsMenuCanvas;
    public GameObject enemyOne;
    public GameObject enemyTwo;
    public GameObject enemyThree;
    public GameObject gem;
    public GameObject winCollider;
    public enemyController enemyControllerScript;
    public AudioSource gemCollectAudio;
    public AudioSource backgroundMusic;
    public GameObject easyCanvas;
    public GameObject normalCanvas;
    public GameObject hardCanvas;
    public GameObject tazerCollect;

    private bool gamePaused = true;
    private PlayerMovement playerMovementScript;

    void Start()
    {
        PauseGame();
        mainMenuCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        winCanvas.SetActive(false);
        escapeCanvas.SetActive(false);
        aboutMMCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!gamePaused)
        {
            CheckCollisionWithEnemy(enemyOne);
            CheckCollisionWithEnemy(enemyTwo);
            CheckCollisionWithEnemy(enemyThree);
            CheckGemCollision();
            CheckWinCollision();
            CheckTazerCollision();
        }

        // Check if game is unpaused, Main Menu Canvas is not active, and neither WinCanvas nor GameOver canvas is active, then play background music
        if (!gamePaused && !mainMenuCanvas.activeSelf && !winCanvas.activeSelf && !gameOverCanvas.activeSelf)
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }
        else
        {
            // If the game is paused, Main Menu Canvas is active, or WinCanvas or GameOver canvas is active, stop playing background music
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
    }

    void TogglePause()
    {
        // Check if the main menu, GameOver canvas, Win canvas, Escape canvas, About Main Menu canvas, or Options Menu canvas is active
        if (mainMenuCanvas.activeSelf || gameOverCanvas.activeSelf || winCanvas.activeSelf || escapeCanvas.activeSelf || aboutMMCanvas.activeSelf || optionsMenuCanvas.activeSelf)
        {
            // If the Escape Canvas is active, deactivate it
            if (escapeCanvas.activeSelf)
            {
                escapeCanvas.SetActive(false);
            }
            else
            {
                // Don't toggle pause if any of the other canvases are active
                return;
            }
        }

        if (gamePaused && Time.timeScale == 0f) // If the game is already paused
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    void CheckCollisionWithEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < 1.0f) // Adjust the threshold as needed
            {
                Debug.Log("Player collided with " + enemy.name + "! Game Over!");
                gameOverCanvas.SetActive(true); // Activate the Game Over canvas
                PausePlayer(); // Pause the player's movements
            }
        }
    }

    void CheckGemCollision()
    {
        if (gem != null)
        {
            float distance = Vector3.Distance(player.transform.position, gem.transform.position);
            if (distance < 1.5f) // Adjust the threshold as needed
            {
                Debug.Log("Player collected the gem!");
                escapeCanvas.SetActive(true); // Activate the Escape canvas

                // Find all game objects with the tag "Gem" and destroy them
                GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
                foreach (GameObject gemObj in gems)
                {
                    Destroy(gemObj);
                }

                // Play the gem collect audio
                if (gemCollectAudio != null)
                {
                    gemCollectAudio.Play();
                }
            }
        }
    }

    void CheckWinCollision()
    {
        if (winCollider != null)
        {
            float distance = Vector3.Distance(player.transform.position, winCollider.transform.position);
            if (distance < 1.0f) // Adjust the threshold as needed
            {
                Debug.Log("Player collided with WinCollider!");
                winCanvas.SetActive(true); // Activate the Win canvas
                PausePlayer(); // Pause the player's movements
            }
        }
    }

    void CheckTazerCollision()
    {
        if (tazerCollect != null)
        {
            float distance = Vector3.Distance(player.transform.position, tazerCollect.transform.position);
            if (distance < 1.5f) // Adjust the threshold as needed
            {
                Debug.Log("Player collected the tazer!");

                // Find all game objects with the tag "Tazer" and destroy them
                GameObject[] tazers = GameObject.FindGameObjectsWithTag("Tazer");
                foreach (GameObject tazer in tazers)
                {
                    Destroy(tazer);
                }

                // Add your logic here for what happens when the player collects the tazer
            }
        }
    }


    public void StartGame()
    {
        // Deactivate Main Menu canvas and unpause the game
        mainMenuCanvas.SetActive(false);
        UnpauseGame();
    }


    void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        gamePaused = true;
        pauseCanvas.SetActive(true); // Activate the Pause canvas

        // Stop playing background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f; // Resume the game
        gamePaused = false;
        pauseCanvas.SetActive(false); // Deactivate the Pause canvas

        // Start playing background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    void PausePlayer()
    {
        // Disable the PlayerMovement script to pause the player's movements
        playerMovementScript.enabled = false;
    }

    public void ResumeFromPauseMenu()
    {
        UnpauseGame(); // Unpause the game when "RTMButton" is pressed
        // Re-enable the PlayerMovement script to resume the player's movements
        playerMovementScript.enabled = true;
    }

    public void ActivateAboutMMCanvas()
    {
        aboutMMCanvas.SetActive(true); // Activate the About Main Menu canvas
    }

    public void DeactivateAboutMMCanvas()
    {
        aboutMMCanvas.SetActive(false); // Deactivate the About Main Menu canvas
    }

    public void ActivateOptionsMenuCanvas()
    {
        optionsMenuCanvas.SetActive(true); // Activate the Options Menu canvas
    }

    public void SetEasyDifficulty()
    {
        enemyControllerScript.maxSpacePresses = 10; // Change maxSpacePresses to 5 for easy difficulty
        enemyControllerScript.UpdateSpacePressCounterUI(); // Update the space press counter UI
        playerMovementScript.speed = 9f; // Change player speed to 8 for easy difficulty

        normalCanvas.SetActive(false);
        hardCanvas.SetActive(false);
        easyCanvas.SetActive(true);

        optionsMenuCanvas.SetActive(false); // Deactivate the Options Menu canvas
    }

    public void SetNormalDifficulty()
    {
        enemyControllerScript.maxSpacePresses = 3; // Change maxSpacePresses to 5 for easy difficulty
        enemyControllerScript.UpdateSpacePressCounterUI(); // Update the space press counter UI
        playerMovementScript.speed = 5f; // Change player speed to default (5) for normal difficulty

        easyCanvas.SetActive(false);
        hardCanvas.SetActive(false);
        normalCanvas.SetActive(true);

        optionsMenuCanvas.SetActive(false); // Deactivate the Options Menu canvas
    }

    public void SetHardDifficulty()
    {
        enemyControllerScript.maxSpacePresses = 2; // Change maxSpacePresses to 5 for easy difficulty
        enemyControllerScript.UpdateSpacePressCounterUI(); // Update the space press counter UI
        playerMovementScript.speed = 4f; // Change player speed to 3 for hard difficulty

        easyCanvas.SetActive(false);
        normalCanvas.SetActive(false);
        hardCanvas.SetActive(true);

        optionsMenuCanvas.SetActive(false); // Deactivate the Options Menu canvas
    }

    public void ExitGame()
    {
        Application.Quit(); // This will close the game
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateEnemySpacePresses()
    {
        enemyControllerScript.UpdateSpacePressCounterUI();
    }

}

