

using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance for easy global access
    public static UIManager Instance;

    // Reference to timer UI text
    public TextMeshProUGUI timerText;

    // Reference to cube counter UI text
    public TextMeshProUGUI cubeText;

    // Panels for win and lose states
    public GameObject winPanel;
    public GameObject losePanel;

    void Awake()
    {
        // Initialize singleton
        Instance = this;
    }

    /// <summary>
    /// Updates the timer display
    /// </summary>
    public void UpdateTimer(float time)
    {
        // Ceil ensures we show whole seconds (e.g., 59 instead of 58.3)
        timerText.text = Mathf.Ceil(time).ToString();
    }

    /// <summary>
    /// Updates cube collection UI
    /// </summary>
    public void UpdateCubes(int collected, int total)
    {
        cubeText.text = $"Cubes: {collected}/{total}";
    }

    /// <summary>
    /// Shows win screen
    /// </summary>
    public void ShowWin()
    {
        winPanel.SetActive(true);
    }

    /// <summary>
    /// Shows lose screen
    /// </summary>
    public void ShowLose()
    {
        losePanel.SetActive(true);
    }
}