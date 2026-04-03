

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance (global access point)
    public static GameManager Instance;

    // Total game duration (in seconds)
    public float gameTime = 60f;

    // Runtime timer
    private float timer;

    // Prevents multiple GameOver calls
    private bool gameEnded = false;

    void Awake()
    {
        // Initialize singleton
        Instance = this;
    }

    void Start()
    {
        // Ensure game is unpaused on start/restart
        Time.timeScale = 1f;

        // Initialize timer
        timer = gameTime;

        // Reset collectible counters
        // (Important because they are static variables)
        CollectibleCube.collected = 0;
        CollectibleCube.total = 0;
    }

    void Update()
    {
        // Stop updating once game ends
        if (gameEnded) return;

        //  Countdown timer
        timer -= Time.deltaTime;

        //  Update UI
        UIManager.Instance.UpdateTimer(timer);
        UIManager.Instance.UpdateCubes(
            CollectibleCube.collected,
            CollectibleCube.total
        );

        //  LOSE CONDITION (time ran out)
        if (timer <= 0f)
        {
            GameOver(false);
        }

        //  WIN CONDITION (all cubes collected)
        if (CollectibleCube.collected >= CollectibleCube.total && CollectibleCube.total > 0)
        {
            GameOver(true);
        }
    }

    /// <summary>
    /// Handles game end state (win or lose)
    /// </summary>
    public void GameOver(bool isWin)
    {
        // Prevent multiple triggers
        gameEnded = true;

        // Pause game
        Time.timeScale = 0f;

        // Show appropriate UI
        if (isWin)
        {
            UIManager.Instance.ShowWin();
        }
        else
        {
            UIManager.Instance.ShowLose();
        }
    }

    /// <summary>
    /// Restarts the current scene
    /// </summary>
    public void Restart()
    {
        // Resume time before reloading
        Time.timeScale = 1f;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}